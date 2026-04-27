using Moq;
using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;
using SchoolApp.Api.Services;

namespace SchoolApp.Tests;

public class EnrollmentServiceTests
{
    private readonly Mock<IEnrollmentRepo> _mockRepo;
    private readonly EnrollmentService _service;

    public EnrollmentServiceTests()
    {
        _mockRepo = new Mock<IEnrollmentRepo>();
        _service = new EnrollmentService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllEnrollmentsAsync_WithEnrollments_ReturnsMappedDtos()
    {
        var enrollments = new List<Enrollment>
        {
            new() { StudentId = 1, CourseId = 1, Grade = "A" },
            new() { StudentId = 2, CourseId = 1, Grade = null }
        };
        _mockRepo.Setup(r => r.GetAllEnrollmentsAsync()).ReturnsAsync(enrollments);

        var result = await _service.GetAllEnrollmentsAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, e => e.StudentId == 1 && e.CourseId == 1 && e.Grade == "A");
        Assert.Contains(result, e => e.StudentId == 2 && e.CourseId == 1 && e.Grade == null);
    }

    [Fact]
    public async Task GetEnrollmentByIdAsync_EnrollmentExists_ReturnsDto()
    {
        var enrollment = new Enrollment { StudentId = 1, CourseId = 1, Grade = "A" };
        _mockRepo.Setup(r => r.GetEnrollmentByIdAsync(1, 1)).ReturnsAsync(enrollment);

        var result = await _service.GetEnrollmentByIdAsync(1, 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.StudentId);
        Assert.Equal(1, result.CourseId);
        Assert.Equal("A", result.Grade);
    }

    [Fact]
    public async Task GetEnrollmentByIdAsync_EnrollmentNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetEnrollmentByIdAsync(99, 99)).ReturnsAsync((Enrollment?)null);

        var result = await _service.GetEnrollmentByIdAsync(99, 99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEnrollmentAsync_ValidDto_ReturnsCreatedDto()
    {
        var dto = new EnrollmentRequestDto { StudentId = 1, CourseId = 1, Grade = "A" };
        var saved = new Enrollment { StudentId = 1, CourseId = 1, Grade = "A" };
        _mockRepo.Setup(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>())).ReturnsAsync(saved);

        var result = await _service.CreateEnrollmentAsync(dto);

        Assert.Equal(1, result.StudentId);
        Assert.Equal(1, result.CourseId);
        Assert.Equal("A", result.Grade);
    }

    [Fact]
    public async Task CreateEnrollmentAsync_WithoutGrade_ReturnsCreatedDtoWithNullGrade()
    {
        var dto = new EnrollmentRequestDto { StudentId = 1, CourseId = 1, Grade = null };
        var saved = new Enrollment { StudentId = 1, CourseId = 1, Grade = null };
        _mockRepo.Setup(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>())).ReturnsAsync(saved);

        var result = await _service.CreateEnrollmentAsync(dto);

        Assert.Equal(1, result.StudentId);
        Assert.Equal(1, result.CourseId);
        Assert.Null(result.Grade);
    }

    [Fact]
    public async Task UpdateEnrollmentAsync_EnrollmentExists_ReturnsUpdatedDto()
    {
        var existing = new Enrollment { StudentId = 1, CourseId = 1, Grade = null };
        var dto = new EnrollmentRequestDto { StudentId = 1, CourseId = 1, Grade = "B" };
        _mockRepo.Setup(r => r.GetEnrollmentByIdAsync(1, 1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateEnrollmentAsync(existing)).ReturnsAsync(existing);

        var result = await _service.UpdateEnrollmentAsync(1, 1, dto);

        Assert.NotNull(result);
        Assert.Equal("B", result.Grade);
    }

    [Fact]
    public async Task UpdateEnrollmentAsync_EnrollmentNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetEnrollmentByIdAsync(99, 99)).ReturnsAsync((Enrollment?)null);

        var result = await _service.UpdateEnrollmentAsync(99, 99, new EnrollmentRequestDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteEnrollmentAsync_EnrollmentExists_ReturnsTrue()
    {
        var enrollment = new Enrollment { StudentId = 1, CourseId = 1, Grade = "A" };
        _mockRepo.Setup(r => r.GetEnrollmentByIdAsync(1, 1)).ReturnsAsync(enrollment);
        _mockRepo.Setup(r => r.DeleteEnrollmentAsync(enrollment)).Returns(Task.CompletedTask);

        var result = await _service.DeleteEnrollmentAsync(1, 1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteEnrollmentAsync_EnrollmentNotFound_ReturnsFalse()
    {
        _mockRepo.Setup(r => r.GetEnrollmentByIdAsync(99, 99)).ReturnsAsync((Enrollment?)null);

        var result = await _service.DeleteEnrollmentAsync(99, 99);

        Assert.False(result);
    }
}
