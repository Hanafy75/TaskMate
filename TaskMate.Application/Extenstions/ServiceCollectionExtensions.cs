using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;

namespace TaskMate.Application.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg=> cfg.RegisterServicesFromAssemblies(typeof(ServiceCollectionExtensions).Assembly));

            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

        }
    }
}
