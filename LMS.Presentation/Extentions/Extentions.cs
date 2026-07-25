using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presistence;
using Shared.ErrorModels;
using Services;
using Scalar.AspNetCore;
using LMS.Presentation.Middlewares;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Presistence.Data.Contexts;
using ServicesAbstraction.Auth;
using System.Text;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LMS.Presentation.Extentions
{
    public static class Extentions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            var jwtOptions = services.ConfigureAllJwtOptions();
            services.AddWebServices();
            services.AddInfrastructureServices(configuration);
            services.ApplyApplicationServices();
            services.ModifyApiBehaviourOptions();
            services.AddIdentityConfigurations();
            services.AddAuthnticateOptions(jwtOptions);
            services.AddOpenApiOpetions();

           


            return services;
        }

        public static async Task<WebApplication> ConfigureWebApplicationMiddlewaresAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.IdentityInitializeAsync();


            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference( opt =>
                {
                    opt.AddPreferredSecuritySchemes("Bearer");
                });
                app.MapGet("/", () => Results.Redirect("/scalar/v1"));
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();  
            app.UseAuthorization();  
            app.MapControllers();
            return app;
        }



        private static void AddWebServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
        }
        private static void AddIdentityConfigurations(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
               opt.User.RequireUniqueEmail = true;
               opt.Password.RequiredLength = 8;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }
        private static void ModifyApiBehaviourOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(cf =>
                            cf.InvalidModelStateResponseFactory = (actionContext) =>
                            {
                                var Errors = actionContext.ModelState.Where(ms => ms.Value!.Errors.Any())
                                                                    .Select(m => new ValidationMessage
                                                                    {
                                                                        Field = m.Key,
                                                                        Errors = m.Value!.Errors.Select(e => e.ErrorMessage)
                                                                    }).ToList();

                                var response = new ValidationErrorResponse
                                {
                                    Errors = Errors
                                };
                                return new BadRequestObjectResult(response);
                            }
                        );
        }
        private static void AddAuthnticateOptions(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = "Bearer";
                opt.DefaultChallengeScheme= "Bearer";
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.TokenKey)),

                    RoleClaimType = ClaimTypes.Role,

                    

                    
                };
            });
        }
        private static void AddOpenApiOpetions(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>(); 
                    document.Components.SecuritySchemes!.Add("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });
                    return Task.CompletedTask;
                });

                options.AddOperationTransformer((operation ,context, CancellationToken) =>
                {
                    var hasAuthorize = context.Description.ActionDescriptor.EndpointMetadata
                                    .OfType<AuthorizeAttribute>()
                                    .Any();
                    
                    if(!hasAuthorize) return Task.CompletedTask;

                    operation.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Security.Add(new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer")] = []
                        });

                    return Task.CompletedTask;   
                });
            });
        }
        private static JwtOptions ConfigureAllJwtOptions(this IServiceCollection services)
        {
            var tokenKey = Environment.GetEnvironmentVariable("TokenKey")!;
            if (string.IsNullOrEmpty(tokenKey) || tokenKey.Length < 32)
                throw new InvalidOperationException("TokenKey environment variable must be set and be at least 32 characters.");

            var durationStr = Environment.GetEnvironmentVariable("JwtDuration");
            var durationInDays = string.IsNullOrEmpty(durationStr) ? 15.0 : double.Parse(durationStr);

            var jwtOptions = new JwtOptions
            {
                TokenKey = tokenKey,
                Issuer = Environment.GetEnvironmentVariable("API_BASE_URL")!,
                Audience = Environment.GetEnvironmentVariable("JwtAudiance")!,
                DurationInDays = durationInDays
            };

            services.AddSingleton<IOptions<JwtOptions>>(new OptionsWrapper<JwtOptions>(jwtOptions));

            return jwtOptions;
        }
    
    
    }
}