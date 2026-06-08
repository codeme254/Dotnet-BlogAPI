using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            // Configure the ID column
            entity.HasKey(col => col.UserId);

            // Configure index on the username column
            entity.HasIndex(col => col.Username)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username");

            // configure index on the email column
            entity.HasIndex(col => col.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

            // Basic validation
            entity.Property(col => col.Username)
            .IsRequired()
            .HasMaxLength(100);

            entity.Property(col => col.Email)
            .IsRequired();

            entity.Property(col => col.PasswordHash)
            .IsRequired();
        });
    }
}