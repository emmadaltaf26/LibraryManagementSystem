using FluentValidation;
using Library.API.Application.Behaviours;
using Library.Application.Common.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Library.API.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Library.Application.DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(Library.Application.DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Library Management System API",
                Version = "v1",
                Description = "A .NET 9.0 Web API for Library Management using Clean Architecture, CQRS, and Entity Framework Core",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Emmad Altaf",
                    Email = "emmadaltaf26@gmail.com"
                }
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LibraryDbConnection")));
        return services;
    }

    public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
