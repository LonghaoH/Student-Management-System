using Moq;
using SchoolApp.Api.Data;
using SchoolApp.Api.DTOs;
using SchoolApp.Api.Models;
using SchoolApp.Api.Services;

namespace SchoolApp.Tests;

public class InstructorServiceTests
{
    private readonly Mock<IInstructorRepo> _mockRepo;
    private readonly InstructorService _service;

    public InstructorServiceTests()
    {
        _mockRepo = new Mock<IInstructorRepo>();
        _service = new InstructorService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllInstructorsAsync_WithInstructors_ReturnsMappedDtos()
    {
        var instructors = new List<Instructor>
        {
            new() { InstructorId = 1, Name = "Dr. Sarah Johnson" },
            new() { InstructorId = 2, Name = "Prof. Michael Chen" }
        };
        _mockRepo.Setup(r => r.GetAllInstructorsAsync()).ReturnsAsync(instructors);

        var result = await _service.GetAllInstructorsAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, i => i.Name == "Dr. Sarah Johnson");
        Assert.Contains(result, i => i.Name == "Prof. Michael Chen");
    }

    [Fact]
    public async Task GetInstructorByIdAsync_InstructorExists_ReturnsDto()
    {
        var instructor = new Instructor { InstructorId = 1, Name = "Dr. Sarah Johnson" };
        _mockRepo.Setup(r => r.GetInstructorByIdAsync(1)).ReturnsAsync(instructor);

        var result = await _service.GetInstructorByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.InstructorId);
        Assert.Equal("Dr. Sarah Johnson", result.Name);
    }

    [Fact]
    public async Task GetInstructorByIdAsync_InstructorNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetInstructorByIdAsync(99)).ReturnsAsync((Instructor?)null);

        var result = await _service.GetInstructorByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateInstructorAsync_ValidDto_ReturnsCreatedDto()
    {
        var dto = new InstructorRequestDto { Name = "Dr. Sarah Johnson" };
        var saved = new Instructor { InstructorId = 1, Name = "Dr. Sarah Johnson" };
        _mockRepo.Setup(r => r.AddInstructorAsync(It.IsAny<Instructor>())).ReturnsAsync(saved);

        var result = await _service.CreateInstructorAsync(dto);

        Assert.Equal(1, result.InstructorId);
        Assert.Equal("Dr. Sarah Johnson", result.Name);
    }

    [Fact]
    public async Task UpdateInstructorAsync_InstructorExists_ReturnsUpdatedDto()
    {
        var existing = new Instructor { InstructorId = 1, Name = "Dr. Sarah Johnson" };
        var dto = new InstructorRequestDto { Name = "Dr. Sarah Johnson Updated" };
        _mockRepo.Setup(r => r.GetInstructorByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateInstructorAsync(existing)).ReturnsAsync(existing);

        var result = await _service.UpdateInstructorAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Dr. Sarah Johnson Updated", result.Name);
    }

    [Fact]
    public async Task UpdateInstructorAsync_InstructorNotFound_ReturnsNull()
    {
        _mockRepo.Setup(r => r.GetInstructorByIdAsync(99)).ReturnsAsync((Instructor?)null);

        var result = await _service.UpdateInstructorAsync(99, new InstructorRequestDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteInstructorAsync_InstructorExists_ReturnsTrue()
    {
        var instructor = new Instructor { InstructorId = 1, Name = "Dr. Sarah Johnson" };
        _mockRepo.Setup(r => r.GetInstructorByIdAsync(1)).ReturnsAsync(instructor);
        _mockRepo.Setup(r => r.DeleteInstructorAsync(instructor)).Returns(Task.CompletedTask);

        var result = await _service.DeleteInstructorAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteInstructorAsync_InstructorNotFound_ReturnsFalse()
    {
        _mockRepo.Setup(r => r.GetInstructorByIdAsync(99)).ReturnsAsync((Instructor?)null);

        var result = await _service.DeleteInstructorAsync(99);

        Assert.False(result);
    }
}
