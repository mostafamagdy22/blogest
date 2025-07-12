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
            string dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connectionString = $"Server={dbServer};Database=blogestCommand;User Id={dbUser};Password={password};TrustServerCertificate=True;";


            Console.WriteLine("== Connection String from ENV ==");
            Console.WriteLine(connectionString ?? "NULL");
            if (string.IsNullOrEmpty(password))
                throw new InvalidOperationException(blogest.domain.Constants.ErrorMessages.BadRequest);

            optionsBuilder.UseSqlServer(connectionString, b =>
            b.MigrationsAssembly("blogest.infrastructure"));

            return new BlogCommandContext(optionsBuilder.Options);
        }
    }
}