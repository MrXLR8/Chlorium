using DiscrodBot;

using Microsoft.EntityFrameworkCore;

namespace DiscordBot
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GuessedWord> GuessedWord { get; set; }
        public object Enviroment { get; private set; }

        public DatabaseContext()
        {
           // Database.EnsureDeleted();
          Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(System.Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb"));
        }
    }
}