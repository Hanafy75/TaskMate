using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMate.Domain.Entities;

namespace TaskMate.Infrastructure.Persistence.Configurations
{
    internal class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Description)
                .HasMaxLength(300);

            builder.Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(b => b.User)
                .WithMany(u => u.Boards)
                .HasForeignKey(b => b.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); // we gonna handle removing boards in code when user is deleted


            builder.HasOne(b => b.Project)
                .WithMany(u => u.Boards)
                .HasForeignKey(b => b.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
