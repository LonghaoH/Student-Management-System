namespace SchoolApp.Api.DTOs;

// Returned by GET endpoints.
public class CourseResponseDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int InstructorId { get; set; }
}
