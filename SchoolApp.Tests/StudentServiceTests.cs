using Moq;
using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;
using SchoolApp.Api.Services;

namespace SchoolApp.Tests;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepo> _mockRepo;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _mockRepo = new Mock<IStudentRepo>();
        _service = new StudentService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllStudentsAsync_WithStudents_ReturnsMappedDtos()
    {
        var students = new List<Student>
        {
            new() { StudentId = 1, Name = "Alice Thompson", Email = "alice@school.com" },
            new() { StudentId = 2, Name = "Bob Martinez", Email = "bob@school.com" }
        };
        _mockRepo.Setup(r => r.GetAllStudentsAsync()).ReturnsAsync(students);

        var result = await _service.GetAllStudentsAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.Name == "Alice Thompson" && s.Email == "alice@school.com");
        Assert.Contains(result, s => s.Name == "Bob Martinez" && s.Email == "bob@school.com");
    }

    [Fact]
    public async Task GetStudentByIdAsync_StudentExists_ReturnsDto()
    {
        var student = new Student { StudentId = 1, Name = "Alice Thompson", Email = "alice@school.com" };
        _mockRepo.Setup(r => r.GetStudentByIdAsync(1)).ReturnsAsync(student);

        var result = await _service.GetStudentByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.StudentId);
        Assert.Equal("Alice Thompson", result.Name);
        Assert.Equal("alice@school.com", result.Email);
    }

    [Fact]
    public async Task GetStudentByIdAsync_StudentNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetStudentByIdAsync(99)).ReturnsAsync((Student?)null);

        var result = await _service.GetStudentByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateStudentAsync_ValidDto_ReturnsCreatedDto()
    {
        var dto = new StudentRequestDto { Name = "Alice Thompson", Email = "alice@school.com" };
        var saved = new Student { StudentId = 1, Name = "Alice Thompson", Email = "alice@school.com" };
        _mockRepo.Setup(r => r.AddStudentAsync(It.IsAny<Student>())).ReturnsAsync(saved);

        var result = await _service.CreateStudentAsync(dto);

        Assert.Equal(1, result.StudentId);
        Assert.Equal("Alice Thompson", result.Name);
        Assert.Equal("alice@school.com", result.Email);
    }

    [Fact]
    public async Task UpdateStudentAsync_StudentExists_ReturnsUpdatedDto()
    {
        var existing = new Student { StudentId = 1, Name = "Alice Thompson", Email = "alice@school.com" };
        var dto = new StudentRequestDto { Name = "Alice Updated", Email = "updated@school.com" };
        _mockRepo.Setup(r => r.GetStudentByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateStudentAsync(existing)).ReturnsAsync(existing);

        var result = await _service.UpdateStudentAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Alice Updated", result.Name);
        Assert.Equal("updated@school.com", result.Email);
    }

    [Fact]
    public async Task UpdateStudentAsync_StudentNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetStudentByIdAsync(99)).ReturnsAsync((Student?)null);

        var result = await _service.UpdateStudentAsync(99, new StudentRequestDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_StudentExists_ReturnsTrue()
    {
        var student = new Student { StudentId = 1, Name = "Alice Thompson", Email = "alice@school.com" };
        _mockRepo.Setup(r => r.GetStudentByIdAsync(1)).ReturnsAsync(student);
        _mockRepo.Setup(r => r.DeleteStudentAsync(student)).Returns(Task.CompletedTask);

        var result = await _service.DeleteStudentAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_StudentNotFound_ReturnsFalse()
    {
        _mockRepo.Setup(r => r.GetStudentByIdAsync(99)).ReturnsAsync((Student?)null);

        var result = await _service.DeleteStudentAsync(99);

        Assert.False(result);
    }
}
