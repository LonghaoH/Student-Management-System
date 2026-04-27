using Moq;
using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;
using SchoolApp.Api.Services;

namespace SchoolApp.Tests;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepo> _mockRepo;
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _mockRepo = new Mock<ICourseRepo>();
        _service = new CourseService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllCoursesAsync_WithCourses_ReturnsMappedDtos()
    {
        var courses = new List<Course>
        {
            new() { CourseId = 1, Title = "Intro to Computer Science", InstructorId = 1 },
            new() { CourseId = 2, Title = "Data Structures", InstructorId = 2 }
        };
        _mockRepo.Setup(r => r.GetAllCoursesAsync()).ReturnsAsync(courses);

        var result = await _service.GetAllCoursesAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Title == "Intro to Computer Science" && c.InstructorId == 1);
        Assert.Contains(result, c => c.Title == "Data Structures" && c.InstructorId == 2);
    }

    [Fact]
    public async Task GetCourseByIdAsync_CourseExists_ReturnsDto()
    {
        var course = new Course { CourseId = 1, Title = "Intro to Computer Science", InstructorId = 1 };
        _mockRepo.Setup(r => r.GetCourseByIdAsync(1)).ReturnsAsync(course);

        var result = await _service.GetCourseByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.CourseId);
        Assert.Equal("Intro to Computer Science", result.Title);
        Assert.Equal(1, result.InstructorId);
    }

    [Fact]
    public async Task GetCourseByIdAsync_CourseNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetCourseByIdAsync(99)).ReturnsAsync((Course?)null);

        var result = await _service.GetCourseByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateCourseAsync_ValidDto_ReturnsCreatedDto()
    {
        var dto = new CourseRequestDto { Title = "Intro to Computer Science", InstructorId = 1 };
        var saved = new Course { CourseId = 1, Title = "Intro to Computer Science", InstructorId = 1 };
        _mockRepo.Setup(r => r.AddCourseAsync(It.IsAny<Course>())).ReturnsAsync(saved);

        var result = await _service.CreateCourseAsync(dto);

        Assert.Equal(1, result.CourseId);
        Assert.Equal("Intro to Computer Science", result.Title);
        Assert.Equal(1, result.InstructorId);
    }

    [Fact]
    public async Task UpdateCourseAsync_CourseExists_ReturnsUpdatedDto()
    {
        var existing = new Course { CourseId = 1, Title = "Intro to Computer Science", InstructorId = 1 };
        var dto = new CourseRequestDto { Title = "Advanced Computer Science", InstructorId = 2 };
        _mockRepo.Setup(r => r.GetCourseByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateCourseAsync(existing)).ReturnsAsync(existing);

        var result = await _service.UpdateCourseAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Advanced Computer Science", result.Title);
        Assert.Equal(2, result.InstructorId);
    }

    [Fact]
    public async Task UpdateCourseAsync_CourseNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetCourseByIdAsync(99)).ReturnsAsync((Course?)null);

        var result = await _service.UpdateCourseAsync(99, new CourseRequestDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCourseAsync_CourseExists_ReturnsTrue()
    {
        var course = new Course { CourseId = 1, Title = "Intro to Computer Science", InstructorId = 1 };
        _mockRepo.Setup(r => r.GetCourseByIdAsync(1)).ReturnsAsync(course);
        _mockRepo.Setup(r => r.DeleteCourseAsync(course)).Returns(Task.CompletedTask);

        var result = await _service.DeleteCourseAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCourseAsync_CourseNotFound_ReturnsFalse()
    {
        _mockRepo.Setup(r => r.GetCourseByIdAsync(99)).ReturnsAsync((Course?)null);

        var result = await _service.DeleteCourseAsync(99);

        Assert.False(result);
    }
}
