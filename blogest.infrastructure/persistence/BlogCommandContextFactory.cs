using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using DotNetEnv;
namespace blogest.infrastructure.persistence
{
    public class BlogCommandContextFactory : IDesignTimeDbContextFactory<BlogCommandContext>
    {
        public BlogCommandContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<BlogCommandContext>();

            Env.Load();
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");

            var connectionString = $"Server={dbServer}; Database={dbName}; User Id={dbUser}; Password={dbPassword}; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;";


            Console.WriteLine("== Connection String from ENV ==");
            Console.WriteLine(connectionString ?? "NULL");

            optionsBuilder.UseSqlServer(connectionString, b =>
            b.MigrationsAssembly("blogest.infrastructure"));

            return new BlogCommandContext(optionsBuilder.Options);
        }
    }
}