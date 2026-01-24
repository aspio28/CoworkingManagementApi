using CoworkingManagement.Domain.Common;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Domain.Exceptions;

namespace CoworkingManagement.Domain.Entities;

public class Reservation: BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = null!;
    public ReservationStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public Reservation(Guid userId, Guid roomId, ReservationStatus status, DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            throw new DomainException("End date must be after start date.");
        }
        Id = Guid.NewGuid();
        UserId = userId;
        RoomId = roomId;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
    } 
    public void Update(Guid? roomId, Guid? userId, DateTime? startDate, DateTime? endDate)
    {

        var startToValidate = startDate ?? this.StartDate;
        var endToValidate = endDate ?? this.EndDate;

        if (endToValidate <= startToValidate)
        {
            throw new DomainException("End date must be after start date.");
        }
        if (roomId.HasValue)
            RoomId = roomId.Value;
        if (userId.HasValue)
            UserId = userId.Value;
        if (startDate.HasValue)
            StartDate = startDate.Value;
        if (endDate.HasValue)
            EndDate = endDate.Value;
    }

    public void UpdateStatus(ReservationStatus status)
    {
        Status = status;
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.Cancelled)
            throw new DomainException("Reservation is already cancelled.");

        if (StartDate < DateTime.UtcNow)
            throw new DomainException("Cannot cancel a reservation that has already started or passed.");

        Status = ReservationStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }
}