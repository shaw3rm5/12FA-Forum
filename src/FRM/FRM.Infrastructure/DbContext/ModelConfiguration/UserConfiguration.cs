    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace Forum.Infrastructure.ModelConfiguration;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userBuilder)
        {
            userBuilder.ToTable("Users");
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
                .HasMany(u => u.Sessions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);
                
            userBuilder
                .Property(u => u.Salt)
                .HasMaxLength(120);
            userBuilder
                .Property(u =>  u.PasswordHash)
                .HasMaxLength(300);
            
            userBuilder
                .Property(c => c.UserName)
                .IsRequired()
                .HasMaxLength(20);
        }
    }