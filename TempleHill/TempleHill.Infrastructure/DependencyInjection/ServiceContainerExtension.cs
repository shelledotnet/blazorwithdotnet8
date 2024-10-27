using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleHill.Application.Extensions.Identity;
using TempleHill.Application.Interfaces;
using TempleHill.Infrastructure.DataAccess;
using TempleHill.Infrastructure.Repository;

namespace TempleHill.Infrastructure.DependencyInjection;

public static class ServiceContainerExtension
{

    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,IConfiguration config)
    {
        services.AddDbContext<THDbContext>(options => options.UseSqlServer(config.GetConnectionString("TempleHillConnect")), ServiceLifetime.Scoped);
        //please note ensure to create a class that inherit IDesignTimeDbContextFactory<THDbContext>



        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<THDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

        services.AddAuthorizationBuilder()
                .AddPolicy("AdministrationPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("Admin", "Manager");

                }).AddPolicy("ManagerPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("Manager");

                })
                .AddPolicy("UserPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("User");
                });

        services.AddCascadingAuthenticationState();
        services.AddScoped<IAccountRepository, AccountRepository>();
        return services;

    
    
    
    }


}
