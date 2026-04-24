namespace SchoolApp.Api.Models;

public class Enrollment
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public Student? Student { get; set; }

    public Course? Course { get; set; }

    public string? Grade { get; set; }
}