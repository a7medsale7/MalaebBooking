using MalaebBooking.Domain.Abstractions;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MalaebBooking.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1️⃣ إعداد الـ DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // 2️⃣ إعداد Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // إعدادات الباسورد
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;

            // إعدادات الحساب
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;

        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddHybridCache();


        return services;
    }

    // 3️⃣ إضافة Serilog بشكل منفصل
    public static IServiceCollection AddSerilogLogging(
     this IServiceCollection services,
     IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        return services;
    }
}