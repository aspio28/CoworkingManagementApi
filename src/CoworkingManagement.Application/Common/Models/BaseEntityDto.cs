namespace CoworkingManagement.Application.Common.Models;

public record BaseEntityDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}