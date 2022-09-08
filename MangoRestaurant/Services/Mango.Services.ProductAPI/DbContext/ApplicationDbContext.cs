namespace Mango.Services.ProductAPI.DbContext;

using Mango.Services.ProductAPI.Models;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 1,
            Name = "Samosa",
            Price = 15,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://res.cloudinary.com/dy9d7zulj/image/upload/v1662635022/14_ujel9e.jpg",
            CategoryName = "Appetizer"
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 2,
            Name = "Paneer Tikka",
            Price = 13.99,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://res.cloudinary.com/dy9d7zulj/image/upload/v1662635022/12_n2lks9.jpg",
            CategoryName = "Appetizer"
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 3,
            Name = "Sweet Pie",
            Price = 10.99,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://res.cloudinary.com/dy9d7zulj/image/upload/v1662635022/11_nln8cx.jpg",
            CategoryName = "Dessert"
        });

        modelBuilder.Entity<Product>().HasData(new Product
        {
            ProductId = 4,
            Name = "Pav Bhaji",
            Price = 15,
            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
            ImageUrl = "https://res.cloudinary.com/dy9d7zulj/image/upload/v1662635022/13_qgccth.jpg",
            CategoryName = "Entree"
        });
    }
}
