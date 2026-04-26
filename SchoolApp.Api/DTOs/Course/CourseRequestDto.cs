namespace SchoolApp.Api.DTOs;

// Used as the request body for POST and PUT endpoints.
public class CourseRequestDto
{
    public string Title { get; set; } = string.Empty;
    public int InstructorId { get; set; }
}
