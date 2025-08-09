using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using TaskMate.Application.Interfaces;
using TaskMate.Application.Services;
using MediatR;

namespace TaskMate.Application.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg=> cfg.RegisterServicesFromAssemblies(typeof(ServiceCollectionExtensions).Assembly));

            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            services.AddScoped<IAuthService, AuthService>();

            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

        }
    }
}
