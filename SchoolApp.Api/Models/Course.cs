namespace SchoolApp.Api.Models;

public class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int InstructorId { get; set; }

    public Instructor? Instructor { get; set; }

    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}