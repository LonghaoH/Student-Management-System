namespace SchoolApp.Api.DTOs;

// Returned by GET endpoints.
// Both StudentId and CourseId are included since together they uniquely identify an enrollment.
public class EnrollmentResponseDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string? Grade { get; set; }
}
