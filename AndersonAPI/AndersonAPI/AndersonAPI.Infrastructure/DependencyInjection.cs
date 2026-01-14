using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Common.Interfaces;
using AndersonAPI.Domain.Repositories;
using AndersonAPI.Infrastructure.Persistence;
using AndersonAPI.Infrastructure.Repositories;
using AndersonAPI.Infrastructure.Services;
using Intent.RoslynWeaver.Attributes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Infrastructure.DependencyInjection.DependencyInjection", Version = "1.0")]

namespace AndersonAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {

                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });

                //IntentIgnore
                options.LogTo(message => System.Diagnostics.Debug.WriteLine(message), LogLevel.Information);

                options.UseLazyLoadingProxies();
            });
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddTransient<IApplicationIdentityUserRepository, ApplicationIdentityUserRepository>();
            services.AddTransient<ICompanyProfileRepository, CompanyProfileRepository>();
            services.AddScoped<IDomainEventService, DomainEventService>();
            return services;
        }
    }
}