using Microsoft.EntityFrameworkCore;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

public class EnrollmentRepo : IEnrollmentRepo
{
    // Take in our DbContext as a dependency
    // so we can use it to access our database
    private readonly AppDbContext _context;

    public EnrollmentRepo(AppDbContext context)
    {
        _context = context;
    }

    // Method to get all enrollments from the database
    // Our repos should be Model/Entity specific.
    public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
    {
        // All business logic is handled inside the service layer
        // All the repo/data layer cares about is grabbing objects from the database.
        return await _context.Enrollments.ToListAsync();
    }

    // Finds a specific enrollment record in our database via its composite PK.
    public async Task<Enrollment?> GetEnrollmentByIdAsync(int studentId, int courseId)
    {
        // Enrollment has no single PK - we pass both keys to FindAsync in the same
        // order they are defined in the composite key (StudentId, CourseId).
        // If it doesn't find anything, it returns null.
        return await _context.Enrollments.FindAsync(studentId, courseId);
    }

    // Adds a new enrollment to the database
    public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        // I want to have my repo method return the newly created record.
        // Unlike other entities, Enrollment has no auto-generated PK -
        // but we still return the object so the service can map it to a DTO.
        return enrollment;
    }

    // Make an update to an enrollment in the database.
    public async Task<Enrollment?> UpdateEnrollmentAsync(Enrollment enrollment)
    {
        // The entity was fetched by this same context instance, so EF Core is already
        // tracking it. Mutating it in the service and calling SaveChangesAsync here
        // is enough — no need to call Update() explicitly.
        await _context.SaveChangesAsync();
        return enrollment;
    }

    // Delete an enrollment from the database.
    public async Task DeleteEnrollmentAsync(Enrollment enrollment)
    {
        // First, we mark the entity/row's state as Deleted in EF Core's change tracker
        _context.Enrollments.Remove(enrollment);

        // Then, we call SaveChangesAsync() to execute the DELETE SQL operation
        await _context.SaveChangesAsync();
    }
}
