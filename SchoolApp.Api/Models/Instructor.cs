namespace SchoolApp.Api.Models;

public class Instructor
{
    public int InstructorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Course> Courses { get; set; } = new List<Course>();
}