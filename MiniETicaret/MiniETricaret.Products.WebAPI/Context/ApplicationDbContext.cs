using Microsoft.EntityFrameworkCore;
using MiniETricaret.Products.WebAPI.Models;

namespace MiniETricaret.Products.WebAPI.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.Property(p => p.Price).HasColumnType("money");
        });
    }
}