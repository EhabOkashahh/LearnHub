using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Contexts;
using Services;
using Services.Mapping;
using ServicesAbstraction;
using ServicesAbstraction.Categories;
using ServicesAbstraction.Courses;
using Scalar.AspNetCore;
using Presistence.Data;
using Services.Specifications;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using LMS.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CoursesProfile).Assembly));
builder.Services.AddScoped<ICoursesService, CourseService>();
builder.Services.AddScoped<ICategoriesService, CategoryService>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddOpenApi();


var app = builder.Build();


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

app.Run();
