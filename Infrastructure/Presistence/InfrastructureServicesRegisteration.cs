using System.Data.Common;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Data;
using Presistence.Data.Contexts;
using Presistence.Data.Seeding;
using Presistence.Interceptors;
using Presistence.Repository;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Presistence
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((sp,options) =>
            {
                options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
                var config = sp.GetRequiredService<IConfiguration>();
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartRepository,CartRepository>();
            services.AddScoped<IDbInitializer,DbInitializer>();
            services.AddScoped<SoftDeleteInterceptor>();
            
            services.AddSingleton<IConnectionMultiplexer>((sp) => {
                var config = sp.GetRequiredService<IConfiguration>();
                return ConnectionMultiplexer.Connect(config.GetConnectionString("RedisConnnection")!);
            });

            services.AddSingleton<IDistributedLockFactory>((sp) =>
            {
                var ConnectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return RedLockFactory.Create(new []{new RedLockMultiplexer(ConnectionMultiplexer)});
            });

            services.AddStackExchangeRedisCache(op =>
            {
               op.Configuration = configuration.GetConnectionString("RedisConnnection"); 
            });
            return services;
        }
    }
}