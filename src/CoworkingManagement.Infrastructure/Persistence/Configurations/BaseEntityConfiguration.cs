using CoworkingManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoworkingManagement.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); 
        builder.Property(e => e.LastModifiedAt).IsRequired(false);
        builder.Property(e => e.CreatedBy).IsRequired(false);
        builder.Property(e => e.LastModifiedBy).IsRequired(false);
    }
}