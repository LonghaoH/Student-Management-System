using Microsoft.EntityFrameworkCore;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

public class StudentRepo : IStudentRepo
{
    // Take in our DbContext as a dependency
    // so we can use it to access our database
    private readonly AppDbContext _context;

    public StudentRepo(AppDbContext context)
    {
        _context = context;
    }

    // Method to get all students from the database
    // Our repos should be Model/Entity specific.
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        // All business logic is handled inside the service layer
        // All the repo/data layer cares about is grabbing objects from the database.
        return await _context.Students.ToListAsync();
    }

    // Finds a specific student record in our database via its PK
    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        // We can use FindAsync() to have EF do a SELECT by the PK
        // If it doesn't find anything, it returns null
        return await _context.Students.FindAsync(id);
    }

    // Adds a new student to the database
    public async Task<Student> AddStudentAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // I want to have my repo method return the newly created record.
        // Including the generated PK, any timestamps that are created automatically, etc.
        // When we call SaveChangesAsync() EF Core updates the local object
        // in our case categoryToAdd, to reflect what was created or updated in the DB
        return student;
    }

    // Make an update to a student in the database.
    public async Task<Student?> UpdateStudentAsync(Student student)
    {
        // The entity was fetched by this same context instance, so EF Core is already
        // tracking it. Mutating it in the service and calling SaveChangesAsync here
        // is enough — no need to call Update() explicitly.
        await _context.SaveChangesAsync();
        return student;
    }

    // Delete a student from the database.
    public async Task DeleteStudentAsync(Student student)
    {
        // First, we mark the entity/row's state as Deleted in EF Core's change tracker
        _context.Students.Remove(student);

        // Then, we call SaveChangesAsync() to execute the DELETE SQL operation
        await _context.SaveChangesAsync();
    }
}
