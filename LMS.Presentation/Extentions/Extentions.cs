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
                app.MapScalarApiReference();
                app.MapGet("/", () => Results.Redirect("/scalar/v1"));
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
                                                                    .Select(m => new VlidationMessage
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.TokenKey))
                };
            });
        }

        private static JwtOptions ConfigureAllJwtOptions(this IServiceCollection services)
        {
            var jwtOptions = new JwtOptions
            {
                TokenKey = Environment.GetEnvironmentVariable("TokenKey")!,
                Issuer = Environment.GetEnvironmentVariable("API_BASE_URL")!,
                Audience = Environment.GetEnvironmentVariable("JwtAudiance")!,
                DurationInDays = double.Parse(Environment.GetEnvironmentVariable("JwtDuration")!)
            };

            services.AddSingleton<IOptions<JwtOptions>>(new OptionsWrapper<JwtOptions>(jwtOptions));

            return jwtOptions;
        }
    }
}