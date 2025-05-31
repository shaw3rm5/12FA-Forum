using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.ModelConfiguration;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> sessionBuilder)
    {
        sessionBuilder.ToTable("Sessions");
        sessionBuilder
            .HasKey(s => s.Id);
    }
}