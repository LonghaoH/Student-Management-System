namespace SchoolApp.Api.DTOs;

// Used as the request body for POST endpoints.
// StudentId and CourseId together form the composite key for an enrollment.
// Grade is optional - an enrollment can be created before a grade is assigned.
public class EnrollmentRequestDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string? Grade { get; set; }
}
