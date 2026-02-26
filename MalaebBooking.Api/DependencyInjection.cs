using FluentValidation;
using MalaebBooking.Application;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Abstractions.Repositories;
using MalaebBooking.Infrastructure;
using MalaebBooking.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

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
            .AddFluentValidationConfiguration();

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
        return services;
    }

    // ================================
    // Repositories Registration
    // ================================
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ISportTypeRepository, SportTypeRepository>();
        return services;
    }

    // ================================
    // Application Services
    // ================================
    private static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<ISportTypeService, SportTypeService>();
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
}