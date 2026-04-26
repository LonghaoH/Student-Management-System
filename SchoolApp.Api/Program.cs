using Microsoft.EntityFrameworkCore;
using SchoolApp.Api.Data;
using SchoolApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Once we've created our DbContext class, we've added our connection string (with password!) to 
// appsettings.development.json, and we brought our models in (or created them!)
// we register our dbcontext with the builder.
builder.Services.AddDbContext<AppDbContext>(options =>
    // Here is where we tell EF Core 2 things: 
    //  1. What type of db provider are we using, for me its MS SQL Server
    //  2. Where is the server? (connection string)
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

// Student stuff
builder.Services.AddScoped<IStudentRepo, StudentRepo>(); // Adding the data layer class
builder.Services.AddScoped<IStudentService, StudentService>(); // Adding the service layer class

// Instructor stuff
builder.Services.AddScoped<IInstructorRepo, InstructorRepo>(); // Data layer class
builder.Services.AddScoped<IInstructorService, InstructorService>(); // Service layer class

// Course stuff
builder.Services.AddScoped<ICourseRepo, CourseRepo>(); // Data layer class
builder.Services.AddScoped<ICourseService, CourseService>(); // Service layer class


// Once we have things like our DbContext, our Services, etc 
// We will register them here, using builder.Services (or some specialty methods for things
// like a dbcontext)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Serves the JSON at /swagger/v1/swagger.json
    app.UseSwagger();

    // Serves the Swagger UI at /swagger
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
