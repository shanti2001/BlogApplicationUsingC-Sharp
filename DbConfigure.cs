using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApplication.Models;

namespace com.blogApplication.BlogApplication2.entity
{
    public class DbConfigure : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }


       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string serverName = "DESKTOP-UQEVJ76";
            string databaseName = "BlogApplication";
            string connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;TrustServerCertificate=true;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}