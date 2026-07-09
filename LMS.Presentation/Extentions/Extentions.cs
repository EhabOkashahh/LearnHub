using Microsoft.AspNetCore.Mvc;
using Presistence;
using Shared.ErrorModels;
using Services;
using Scalar.AspNetCore;
using LMS.Presentation.Middlewares;

namespace LMS.Presentation.Extentions
{
    public static class Extentions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services)
        {
            services.AddWebServices();
            services.AddInfrastructureServices();
            services.ApplyApplicationServices();
            services.ModifyApiBehaviourOptions();

            return services;
        }

        public static WebApplication ConfigureWebApplicationMiddlewares(this WebApplication app)
        {

            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.MapGet("/", () => Results.Redirect("/scalar/v1"));

            }

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