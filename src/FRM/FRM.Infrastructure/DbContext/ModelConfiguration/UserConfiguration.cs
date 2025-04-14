using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.ModelConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userBuilder)
    {
        userBuilder
            .HasKey(u => u.Id);
        
        userBuilder
            .HasMany(c => c.Comments)
            .WithOne(c => c.Author)
            .OnDelete(DeleteBehavior.Cascade);
        
        userBuilder
            .HasMany(u => u.Topics)
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.AuthorId);
        
        userBuilder
            .Property(c => c.UserName)
            .IsRequired()
            .HasMaxLength(20);
    }
}