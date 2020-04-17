using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CategoryTable(builder);

            ProductTable(builder);           
        }

        private ModelBuilder CategoryTable(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Category>().HasKey(p => p.Id);
            builder.Entity<Category>().Property(p => p.Id).IsRequired().HasColumnName("category_id");
            builder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(144).HasColumnName("name");
            builder.Entity<Category>().HasMany(p => p.Products).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);

            //builder.Entity<Category>().HasData
            //(
            //    new Category { Id = 100, Name = "Fruits and Vegetables" }, // Id set manually due to in-memory provider
            //    new Category { Id = 101, Name = "Dairy" }
            //);
            return builder;
        }

        private ModelBuilder ProductTable(ModelBuilder builder)
        {
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Product>().HasKey(p => p.Id);
            builder.Entity<Product>().Property(p => p.Id).HasColumnName("product_id");
            builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(144).HasColumnName("name");
            builder.Entity<Product>().Property(p => p.QuantityInPackage).IsRequired().HasColumnName("quantity_in_package");
            builder.Entity<Product>().Property(p => p.UnitOfMeasurement).IsRequired().HasColumnName("unit_measurement_id");
            builder.Entity<Product>().Property(p => p.CategoryId).IsRequired().HasColumnName("category_id");

            //builder.Entity<Product>().HasData
            //(
            //    new Product
            //    {
            //        Id = 100,
            //        Name = "Apple",
            //        QuantityInPackage = 1,
            //        UnitOfMeasurement = EUnitOfMeasurement.Unity,
            //        CategoryId = 100
            //    },
            //    new Product
            //    {
            //        Id = 101,
            //        Name = "Milk",
            //        QuantityInPackage = 2,
            //        UnitOfMeasurement = EUnitOfMeasurement.Liter,
            //        CategoryId = 101,
            //    }
            //);

            return builder;
        }
    }
}