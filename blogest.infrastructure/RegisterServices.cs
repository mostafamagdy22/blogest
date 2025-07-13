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
using blogest.application.Interfaces.repositories.Categories;
using blogest.application.Interfaces.repositories.Likes;
using blogest.domain.Constants;
using blogest.infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using blogest.infrastructure.Configuration;
using Hangfire;
using Microsoft.Data.SqlClient;
using Serilog;

namespace blogest.infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            DotNetEnv.Env.Load();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPostsCommandRepository, PostsCommandRepository>();
            services.AddScoped<IPostsQueryRepository, PostsQueryRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICommentsCommandRepository, CommentsCommandRepository>();
            services.AddScoped<ICommentsQueryRepository, CommentsQueryRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesCommandRepository>();
            services.AddScoped<ILikesCommandRepository, LikesCommandRepository>();
            services.AddScoped<ILikesQueryRepository, LikesQueryRepository>();
            services.AddScoped<IAuthorizationHandler, IsPostAuthorHandler>();
            services.AddScoped<IImageStorageService, CloudinaryStorageService>();
            services.AddScoped<IEmailService, EmailService>();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connectionString = $"Server={dbServer};Database=blogestCommand;User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";

            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(() => new SqlConnection(connectionString));
            });
            services.AddHangfireServer();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.CandEditPost, policy =>
                        policy.Requirements.Add(new IsPostAuthorRequirement()));
            });

            services.AddDbContext<BlogCommandContext>(options =>
            {
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

                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<BlogCommandContext>()
            .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(MappingProfile).Assembly, typeof(MappingApplicationProfile).Assembly);

            return services;
        }
    }
}
