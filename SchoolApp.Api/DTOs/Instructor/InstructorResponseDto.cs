namespace SchoolApp.Api.DTOs;

// Returned by GET endpoints.
public class InstructorResponseDto
{
    public int InstructorId { get; set; }
    public string Name { get; set; } = string.Empty;
}
