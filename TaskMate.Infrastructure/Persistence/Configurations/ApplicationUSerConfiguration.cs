using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMate.Domain.Entities;

namespace TaskMate.Infrastructure.Persistence.Configurations
{
    internal class ApplicationUSerConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(20)
                .IsRequired();

            builder.OwnsMany(u => u.RefreshTokens);
        }
    }
}
