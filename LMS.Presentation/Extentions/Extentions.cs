using Microsoft.AspNetCore.Mvc;
using Presistence;
using Shared.ErrorModels;
using Services;
using Scalar.AspNetCore;
using LMS.Presentation.Middlewares;
using Domain.Contracts;
using Domain.Entities.Identity;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Presistence.Data.Contexts;

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

            return services;
        }

        public static async Task<WebApplication> ConfigureWebApplicationMiddlewaresAsync(this WebApplication app)
        {

            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
                app.MapGet("/", () => Results.Redirect("/scalar/v1"));
            }

            using var scope = app.Services.CreateScope();
            var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.IdentityInitializeAsync();
            
            app.UseHttpsRedirection();
            app.MapControllers();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
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
    }
}