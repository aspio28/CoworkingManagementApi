using CoworkingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoworkingManagement.Infrastructure.Persistence.Configurations;

public class ReservationConfigurations : BaseEntityConfiguration<Reservation>
{
    public override void Configure(EntityTypeBuilder<Reservation> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Reservations");
        
        builder.HasOne(r => r.User)
               .WithMany(u => u.Reservations)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Room)
               .WithMany(ro => ro.Reservations)
               .HasForeignKey(r => r.RoomId)
               .OnDelete(DeleteBehavior.Restrict);
               
        builder.Property(r => r.Status).IsRequired().HasConversion<string>();
        builder.Property(r => r.CancelledAt).IsRequired(false);

        builder.Property(r => r.StartDate).IsRequired();
        builder.Property(r => r.EndDate).IsRequired();

        builder.Property(r => r.CreatedAt)
        .IsRequired()
        .HasDefaultValueSql("CURRENT_TIMESTAMP"); 

        builder.Property(r => r.CreatedBy)
            .IsRequired(true);
    }
}
