using blogest.application.Interfaces.repositories;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Mapping;
using blogest.infrastructure.persistence;
using blogest.infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using blogest.infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using blogest.infrastructure.Identity;
using MediatR;

namespace blogest.infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            DotNetEnv.Env.Load();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPostsCommandRepository, PostsCommandRepository>();
            services.AddScoped<IPostsQueryRepository,PostsQueryRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICommentsCommandRepository, CommentsCommandRepository>();
            
            services.AddDbContext<BlogCommandContext>(options =>
            {
                var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
                var dbUser = Environment.GetEnvironmentVariable("DB_USER");
                var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
                var connectionString = $"Server={dbServer};Database=blogestCommand;User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";
                options.UseSqlServer(connectionString,
                x => x.MigrationsAssembly("blogest.infrastructure"));
            });

            services.AddDbContext<BlogQueryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("blogest.infrastructure"));
            });
            
            services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<BlogCommandContext>()
            .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(MappingProfile).Assembly, typeof(MappingApplicationProfile).Assembly);

            return services;
        }
    }
}
