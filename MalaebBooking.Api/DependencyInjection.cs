using FluentValidation;
using MalaebBooking.Application;
using MalaebBooking.Infrastructure.Mail;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Domain.Entities;
using MalaebBooking.Infrastructure;
using MalaebBooking.Infrastructure.Authentication;
using MalaebBooking.Infrastructure.Mail;
using MalaebBooking.Infrastructure.Persistence;
using MalaebBooking.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;
using MalaebBooking.Api.Middleware;

namespace MalaebBooking.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddInfrastructureLayer(configuration)
            .AddRepositories()
            .AddApplicationServices()
            .AddPresentation()
            .AddSwaggerDocumentation()
            .AddMapsterConfiguration()
            .AddFluentValidationConfiguration()
            .AddAuthenticationConfiguration(configuration);



        return services;
    }

    // ================================
    // Infrastructure Layer
    // ================================
    private static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.Configure<MailSetting>(configuration.GetSection(nameof(MailSetting)));
        services.AddHttpContextAccessor();

        return services;
    }

    // ================================
    // Repositories Registration
    // ================================
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ISportTypeRepository, SportTypeRepository>();
        services.AddScoped<IStadiumRepository, StadiumRepository>();
        services.AddScoped<IStadiumImageRepository, StadiumImageRepository>();
        services.AddScoped<ITimeSlotRepository ,  TimeSlotRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();


        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<IStadiumImageService, StadiumImageService>();


        return services;
    }

    // ================================
    // Application Services
    // ================================
    private static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<ISportTypeService, SportTypeService>();
        services.AddScoped<IStadiumService, StadiumService>();
        services.AddScoped<ITimeSlotService, TimeSlotService>();
        services.AddScoped<IBookingService,BookingService>();

        return services;
    }

    // ================================
    // Controllers + API Explorer
    // ================================
    private static IServiceCollection AddPresentation(
        this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        return services;
    }

    // ================================
    // Swagger Configuration
    // ================================
    private static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services)
    {
        services.AddSwaggerGen();
        return services;
    }

    // ================================
    // Mapster Configuration
    // ================================
    private static IServiceCollection AddMapsterConfiguration(
        this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(AssemblyReference).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }

    // ================================
    // FluentValidation Configuration
    // ================================
    private static IServiceCollection AddFluentValidationConfiguration(
        this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(
            typeof(AssemblyReference).Assembly);

        return services;
    }

    // ================================
    // AddAuthentication Configuration
    // ================================

    private static IServiceCollection AddAuthenticationConfiguration(
     this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(settings.Key))
            };
        });

        return services;
    }
}