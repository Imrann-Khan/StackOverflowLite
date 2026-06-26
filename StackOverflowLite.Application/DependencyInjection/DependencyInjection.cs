using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackOverflowLite.Application.Core.Behaviors;

namespace StackOverflowLite.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // must be Executing not Calling

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof( ValidationBehavior<,>));

        return services;
    }
}