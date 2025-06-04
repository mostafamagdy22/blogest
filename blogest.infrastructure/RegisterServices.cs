using blogest.infrastructure.persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace blogest.infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<BlogCommandContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Default"));
			});

            services.AddDbContext<BlogQueryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

			return services;
        }
    }
}
