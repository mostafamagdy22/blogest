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
using blogest.application.Interfaces.repositories.Saves;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace blogest.infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPostsCommandRepository, PostsCommandRepository>();
            services.AddScoped<IPostsQueryRepository, PostsQueryRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICommentsCommandRepository, CommentsCommandRepository>();
            services.AddScoped<ICommentsQueryRepository, CommentsQueryRepository>();
            services.AddScoped<ICategoriesCommandRepository, CategoriesCommandRepository>();
            services.AddScoped<ICategoriesQueryRepository, CategoriesQueryRepository>();
            services.AddScoped<ILikesCommandRepository, LikesCommandRepository>();
            services.AddScoped<ILikesQueryRepository, LikesQueryRepository>();
            services.AddScoped<IAuthorizationHandler, IsPostAuthorHandler>();
            services.AddScoped<IImageStorageService, CloudinaryStorageService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISavesCommandRepository, SavesCommandRepository>();
            services.AddScoped<ISearchService, ElasticSearchService>();

            var elasticConfig = new ElasticSearchConfig();
            configuration.GetSection("Elasticsearch").Bind(elasticConfig);

            var settings = new ElasticsearchClientSettings(new Uri(elasticConfig.Url))
            .CertificateFingerprint(Environment.GetEnvironmentVariable("CERTIFICATE"))
            .Authentication(new BasicAuthentication("elastic", Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD")));

            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");

            Console.WriteLine("DB_NAME = " + dbName);
            Console.WriteLine("DB_USER = " + dbUser);
            Console.WriteLine("DB_PASSWORD = " + dbPassword);
            Console.WriteLine("DB_SERVER = " + dbServer);


            var connectionString = $"Server={dbServer}; Database={dbName}; User Id={dbUser}; Password={dbPassword}; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;";

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
