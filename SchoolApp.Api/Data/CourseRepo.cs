using Microsoft.EntityFrameworkCore;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

public class CourseRepo : ICourseRepo
{
    // Take in our DbContext as a dependency
    // so we can use it to access our database
    private readonly AppDbContext _context;

    public CourseRepo(AppDbContext context)
    {
        _context = context;
    }

    // Method to get all courses from the database
    // Our repos should be Model/Entity specific.
    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        // All business logic is handled inside the service layer
        // All the repo/data layer cares about is grabbing objects from the database.
        return await _context.Courses.ToListAsync();
    }

    // Finds a specific course record in our database via its PK.
    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        // We can use FindAsync() to have EF do a SELECT by the PK
        // If it doesn't find anything, it returns null
        return await _context.Courses.FindAsync(id);
    }

    // Adds a new course to the database
    public async Task<Course> AddCourseAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // I want to have my repo method return the newly created record.
        // Including the generated PK, any timestamps that are created automatically, etc.
        // When we call SaveChangesAsync() EF Core updates the local object
        // in our case course, to reflect what was created or updated in the DB
        return course;
    }

    // Make an update to a course in the database.
    public async Task<Course?> UpdateCourseAsync(Course course)
    {
        // The entity was fetched by this same context instance, so EF Core is already
        // tracking it. Mutating it in the service and calling SaveChangesAsync here
        // is enough — no need to call Update() explicitly.
        await _context.SaveChangesAsync();
        return course;
    }

    // Delete a course from the database.
    public async Task DeleteCourseAsync(Course course)
    {
        // First, we mark the entity/row's state as Deleted in EF Core's change tracker
        _context.Courses.Remove(course);

        // Then, we call SaveChangesAsync() to execute the DELETE SQL operation
        await _context.SaveChangesAsync();
    }
}
