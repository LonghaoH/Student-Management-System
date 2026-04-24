using Microsoft.EntityFrameworkCore;
using SchoolApp.Api.Models;

namespace SchoolApp.Api.Data;

public class AppDbContext : DbContext
{
    // DbContextOptions is injected by ASP.NET's DI container via Program.cs (AddDbContext).
    // This is what carries the connection string and provider (SQL Server).
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets represent database tables. EF Core uses these to generate the schema
    // during migrations and to query/save data at runtime
    public DbSet<Student> Students => Set<Student>();

    public DbSet<Instructor> Instructors => Set<Instructor>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();


    // We're going to override a method that comes in from DbContext
    // to tell it where to find (or create) our database

    // I pulled in my old DbContext, but we no longer need to use OnConfiguring
    // We will supply the connection string + database type inside of Program.cs
    // not in this AppDbContext class

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder
    //         .UseSqlite("Data Source = ProductCatalog.db")
    //         .LogTo(Console.WriteLine, LogLevel.Information);
    // }

    // Level 1 Config: Conventions
    // Level 2 Config: Data Annotations
    // Level 3 Config: Fluent API inside of your DbContext class
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ______________ Enrollment _________________

        // Enrollment uses a composite key instead of a single auto-increment ID.
        // This enforces uniqueness at the database level - a student cannot be enrolled
        // in the same course more than once.
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        // Enrollment -> Student (many-to-one)
        // Cascade delete means if a student is deleted, their enrollments are also deleted.
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enrollment -> Course (many-to-one)
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);


        // _______________ Course ________________

        // Course -> Instructor (many-to-one)
        // Restrict means EF will throw a DbUpdateException if user tries to delete 
        // an instructor that still has courses assigned. This will be handled in 
        // InstructorController with a 409 Conflict
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);


        // ____________ Column Constraints ______________
        // These are reflected in the migration-generated SQL (example: NVARCHAR(100) NOT NULL).

        modelBuilder.Entity<Student>()
            .Property(s => s.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Student>()
            .Property(s => s.Email).HasMaxLength(200).IsRequired();

        modelBuilder.Entity<Instructor>()
            .Property(i => i.Name).HasMaxLength(100).IsRequired();

        modelBuilder.Entity<Course>()
            .Property(c => c.Title).HasMaxLength(200).IsRequired();

        modelBuilder.Entity<Enrollment>()
            .Property(e => e.Grade).HasMaxLength(1);

    }
}