namespace CoworkingManagement.Application.Common.Models;

public class ErrorResponse
{
    public int Status { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }

    public ErrorResponse(int status, string message, string? details = null)
    {
        Status = status;
        Message = message;
        Details = details;
    }
}