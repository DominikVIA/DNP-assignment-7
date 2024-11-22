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
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\patty\\RiderProjects\\DNP_Assignment7\\EfcRepositories\\Reddit.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>().HasKey(c => c.Id);
            
            modelBuilder.Entity<Content>().Property(c => c.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Content>().ToTable("Contents");

            modelBuilder.Entity<Post>()
                .ToTable("Posts")
                .HasBaseType<Content>();

            modelBuilder.Entity<Comment>()
                .ToTable("Comments")
                .HasBaseType<Content>();

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.RespondingTo)
                .WithMany()
                .HasForeignKey(c => c.RespondingToId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Reaction>().HasKey(r => new { r.UserId, r.ContentId });
        }
    }
}