using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;
using TaskMate.Infrastructure.Persistence;
using TaskMate.Infrastructure.Repositories;
using TaskMate.Infrastructure.Services;

namespace TaskMate.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBoardRepository,BoardRepository>();
            services.AddScoped<IProjectRepository,ProjectRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }
    }
}
