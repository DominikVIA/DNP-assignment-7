using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Reaction> Reactions => Set<Reaction>();
        public DbSet<Content> Contents => Set<Content>();

        public AppContext(DbContextOptions<AppContext> options) : base(options) { }
        public AppContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source = Reddit.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Content>().HasKey(x => x.Id);
            modelBuilder.Entity<Content>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Post>().HasBaseType<Content>();
            modelBuilder.Entity<Comment>().HasBaseType<Content>();
            
            modelBuilder.Entity<Reaction>().HasKey(x => new {x.UserId, x.ContentId});
        }
    }
}