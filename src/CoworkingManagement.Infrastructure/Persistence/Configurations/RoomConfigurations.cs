using CoworkingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoworkingManagement.Infrastructure.Persistence.Configurations;

public class RoomConfigurations : BaseEntityConfiguration<Room>
{
    public override void Configure(EntityTypeBuilder<Room> builder)
    {
        base.Configure(builder);
        builder.ToTable("Rooms");

        builder.Property(r => r.Capacity).IsRequired();

        builder.Property(r => r.Location).IsRequired();
    }
}