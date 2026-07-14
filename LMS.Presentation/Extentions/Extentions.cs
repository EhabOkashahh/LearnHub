using Microsoft.AspNetCore.Mvc;
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
using System.Text;

namespace LMS.Presentation.Extentions
{
    public static class Extentions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebServices();
            services.AddInfrastructureServices(configuration);
            services.ApplyApplicationServices();
            services.ModifyApiBehaviourOptions();
            services.AddIdentityConfigurations();
            services.AddAuthnticateOptions();

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
        private static void AddAuthnticateOptions(this IServiceCollection services)
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
                    ValidIssuer = Environment.GetEnvironmentVariable("API_BASE_URL"),

                    ValidateAudience = true,
                    ValidAudience = Environment.GetEnvironmentVariable("JwtAudiance"),

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TokenKey")!))
                };
            });
        }
    }
}