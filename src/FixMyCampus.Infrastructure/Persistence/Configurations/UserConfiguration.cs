using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Full Name
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        // Email
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        // Unique Email
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // Password Hash
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        // Role
        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        // Created At
        builder.Property(u => u.CreatedAt)
            .IsRequired();
    }
}