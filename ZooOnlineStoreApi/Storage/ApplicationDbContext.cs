using Microsoft.EntityFrameworkCore;
using ZooOnlineStoreApi.Models;

namespace ZooOnlineStoreApi.Storage
{
    public class ApplicationDbContext: DbContext
    {
        //entites
        public required DbSet<Address> Addresses { get; set; }
        public required DbSet<Admin> Admins { get; set; }
        public required DbSet<Category> Categories { get; set; }
        public required DbSet<Feedback> Feedbacks { get; set; }
        public required DbSet<Order> Orders { get; set; }
        public required DbSet<OrderItem> OrderItems { get; set; }
        public required DbSet<PetType> PetTypes { get; set; }
        public required DbSet<Product> Products { get; set; }
        public required DbSet<ProductImage> ProductImages { get; set; }
        public required DbSet<User> Users { get; set; }
        public required DbSet<Payment> Payments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string useConnection = config["UseConnection"] ?? "DefaultConnection";
            string? connectionString = config.GetConnectionString(useConnection);
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация уникальности
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.UUID).IsUnique();
                entity.HasIndex(u => u.Phone).IsUnique();
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(a => a.Login).IsUnique();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.HasIndex(p => p.Name).IsUnique();
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasIndex(p => p.ImageName).IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(o => o.OrderNumber).IsUnique();
            });
        }
    }
}
