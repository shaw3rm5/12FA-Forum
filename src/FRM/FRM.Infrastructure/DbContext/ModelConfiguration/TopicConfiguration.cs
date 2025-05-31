using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.ModelConfiguration;

public class TopicConfiguration :  IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> topicBuilder)
    {
        topicBuilder.ToTable("Topics");
        topicBuilder
            .HasKey(x => x.Id);
        topicBuilder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnType("varchar(100)");
        
        topicBuilder
            .HasOne(t => t.Author)
            .WithMany(t => t.Topics)
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Cascade); // topic has one creator(user)
        
        topicBuilder
            .HasOne(t => t.Forum)
            .WithMany(f => f.Topics)
            .HasForeignKey(a => a.ForumId)
            .OnDelete(DeleteBehavior.Cascade); // topic has one forum
        
        topicBuilder
            .HasMany(t => t.Comments)
            .WithOne(t => t.Topic)
            .HasForeignKey(t => t.TopicId); // topic has many comments

    }
}