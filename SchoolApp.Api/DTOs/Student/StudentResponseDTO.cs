namespace SchoolApp.Api.DTOs;

// Returned by GET endpoints.
public class StudentResponseDto
{
    public int StudentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
