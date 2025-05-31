using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.ModelConfiguration;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> commentBuilder)
    {
        commentBuilder.ToTable("Comments");
        
        commentBuilder
            .HasKey(c => c.Id);
        
        commentBuilder
            .HasOne(c => c.Topic)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TopicId);

        commentBuilder
            .HasOne(c => c.Author)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.UserId);
    }
}