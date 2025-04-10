using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.ModelConfiguration;

public class ForumConfiguration : IEntityTypeConfiguration<Forum>
{
    public void Configure(EntityTypeBuilder<Forum> builder)
    {
        builder
            .HasKey(f => f.Id);
        
        builder
            .HasMany(f => f.Topics)
            .WithOne(t => t.Forum)
            .HasForeignKey(t => t.ForumId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}