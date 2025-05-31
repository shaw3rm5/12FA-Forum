using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.ModelConfiguration;

public class ForumConfiguration : IEntityTypeConfiguration<Domain.Models.Forum>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Forum> forumBuilder)
    {
        forumBuilder.ToTable("Forums");
        forumBuilder
            .HasKey(f => f.Id);
        
        forumBuilder
            .HasMany(f => f.Topics)
            .WithOne(t => t.Forum)
            .HasForeignKey(t => t.ForumId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}