using Microsoft.EntityFrameworkCore;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

public class InstructorRepo : IInstructorRepo
{
    // Take in our DbContext as a dependency
    // so we can use it to access our database
    private readonly AppDbContext _context;

    public InstructorRepo(AppDbContext context)
    {
        _context = context;
    }

    // Method to get all instructors from the database
    // Our repos should be Model/Entity specific.
    public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
    {
        // All business logic is handled inside the service layer
        // All the repo/data layer cares about is grabbing objects from the database.
        return await _context.Instructors.ToListAsync();
    }

    // Finds a specific instructor record in our database via its PK.
    public async Task<Instructor?> GetInstructorByIdAsync(int id)
    {
        // We can use FindAsync() to have EF do a SELECT by the PK
        // If it doesn't find anything, it returns null
        return await _context.Instructors.FindAsync(id);
    }

    // Adds a new instructor to the database
    public async Task<Instructor> AddInstructorAsync(Instructor instructor)
    {
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        // I want to have my repo method return the newly created record.
        // Including the generated PK, any timestamps that are created automatically, etc.
        // When we call SaveChangesAsync() EF Core updates the local object
        // to reflect what was created or updated in the DB
        return instructor;
    }

    // Make an update to an instructor in the database.
    public async Task<Instructor?> UpdateInstructorAsync(Instructor instructor)
    {
        // The entity was fetched by this same context instance, so EF Core is already
        // tracking it. Mutating it in the service and calling SaveChangesAsync here
        // is enough — no need to call Update() explicitly.
        await _context.SaveChangesAsync();
        return instructor;
    }

    // Delete an instructor from the database.
    public async Task DeleteInstructorAsync(Instructor instructor)
    {
        // First, we mark the entity/row's state as Deleted in EF Core's change tracker
        _context.Instructors.Remove(instructor);

        // Then, we call SaveChangesAsync() to execute the DELETE SQL operation
        await _context.SaveChangesAsync();
    }
}