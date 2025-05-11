using Xunit;
using Domain.Repositories;
using MapsterMapper;
using Moq;
using Microsoft.Extensions.Logging;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.Concrete.Tests;

public class ChainServiceTests
{
    private readonly Mock<IChainRepository> mockChainRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILogger<ChainService>> mockLogger;
    private readonly ChainService service;

    public ChainServiceTests()
    {
        mockChainRepo = new Mock<IChainRepository>();
        mockMapper = new Mock<IMapper>();
        mockLogger = new Mock<ILogger<ChainService>>();
        service = new ChainService(mockLogger.Object, mockChainRepo.Object, mockMapper.Object);
    }


    private static void InitilizeTestData(out CreateChainDto dto, out Chain chain)
    {
        Guid userId = Guid.NewGuid();
        Guid categoryId = Guid.NewGuid();

        dto = new CreateChainDto
        {
            UserId = userId,
            Title = "Read Books",
            Description = "A chain for reading books",
            IsPublic = true,
            CategoryId = categoryId,
        };
        chain = new Chain
        {
            UserId = userId,
            Title = "Read Books",
            Description = "A chain for reading books",
            IsPublic = true,
            CategoryId = categoryId,
        };
    }

    [Fact]
    public async Task CreateChainAsync_ShouldMapAndAddChain()
    {
        // Arrange

        CreateChainDto dto;
        Chain chain;
        InitilizeTestData(out dto, out chain);

        mockMapper.Setup(m => m.Map<Chain>(dto)).Returns(chain);

        // Act
        await service.CreateChainAsync(dto);

        // Assert
        mockMapper.Verify(m => m.Map<Chain>(dto), Times.Once);
        mockChainRepo.Verify(r => r.AddAsync(chain), Times.Once);
    }

    [Fact]
    public async Task CreateChainAsync_WhenAddAsyncFails_ThrowsException()
    {
        // Arrange

        CreateChainDto dto;
        Chain chain;
        InitilizeTestData(out dto, out chain);

        mockMapper.Setup(m => m.Map<Chain>(dto)).Returns(chain);
        mockChainRepo.Setup(r => r.AddAsync(It.IsAny<Chain>())).ThrowsAsync(new Exception("Database error"));

        // Act & Assert

        var ex = await Xunit.Assert.ThrowsAsync<Exception>(() => service.CreateChainAsync(dto));

        Xunit.Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task DeleteChainAsync_ShouldDeleteChain()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        mockChainRepo.Setup(r => r.DeleteAsync(chainId)).Returns(Task.CompletedTask);
        
        // Act
        await service.DeleteChainAsync(chainId);
        
        // Assert
        mockChainRepo.Verify(r => r.DeleteAsync(chainId), Times.Once);
    }

    [Fact]
    public async Task DeleteChainAsync_WhenDeleteAsyncFails_ThrowsException()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        mockChainRepo.Setup(r => r.DeleteAsync(chainId)).ThrowsAsync(new Exception("Database error"));
        
        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<Exception>(() => service.DeleteChainAsync(chainId));
        Xunit.Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task GetChainByIdAsync_ShouldReturnChainDto()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        var chain = new Chain { Id = chainId, Title = "Test Chain" };
        var chainDto = new ChainDto { Id = chainId, Title = "Test Chain" };
        mockChainRepo.Setup(r => r.GetByIdAsync(chainId)).ReturnsAsync(chain);
        mockMapper.Setup(m => m.Map<ChainDto>(chain)).Returns(chainDto);
        
        // Act
        var result = await service.GetChainByIdAsync(chainId);
        
        // Assert
        Xunit.Assert.Equal(chainDto, result.Data);
    }

    [Fact]
    public async Task GetChainByIdAsync_WhenChainNotFound_ThrowsNotFoundException()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        mockChainRepo.Setup(r => r.GetByIdAsync(chainId)).ReturnsAsync((Chain)null!);
        
        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<NotFoundException>(() => service.GetChainByIdAsync(chainId));
        Xunit.Assert.Equal($"{nameof(Chain)} with id: {chainId} not found.", ex.Message);
    }

    [Fact]
    public async Task GetChainsByCategoryIdAsync_ShouldReturnChains()
    {
        // Arrange
        var dto = new ChainsRequestDto { Id = Guid.NewGuid(), PageNumber = 1, PageSize = 10 };
        var chains = new List<Chain> { new Chain { Id = Guid.NewGuid(), Title = "Test Chain" } };
        var totalCount = chains.Count;
        var chainDtos = new List<ChainDto> { new ChainDto { Id = chains[0].Id, Title = "Test Chain" } };
        mockChainRepo.Setup(r => r.GetPublicChainsByCategoryAsync(dto.Id, dto.PageNumber, dto.PageSize))
            .ReturnsAsync((chains, totalCount));
        mockMapper.Setup(m => m.Map<IEnumerable<ChainDto>>(chains)).Returns(chainDtos);
        
        // Act
        var result = await service.GetChainsByCategoryIdAsync(dto);
        
        // Assert
        Xunit.Assert.Equal(totalCount, result.TotalCount);
        Xunit.Assert.Equal(chainDtos, result.Data);
    }

    [Fact]
    public async Task GetChainsByCategoryIdAsync_WhenGetPublicChainsByCategoryAsyncFails_ThrowsException()
    {
        // Arrange
        var dto = new ChainsRequestDto { Id = Guid.NewGuid(), PageNumber = 1, PageSize = 10 };
        mockChainRepo.Setup(r => r.GetPublicChainsByCategoryAsync(dto.Id, dto.PageNumber, dto.PageSize))
            .ThrowsAsync(new Exception("Database error"));
        
        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<Exception>(() => service.GetChainsByCategoryIdAsync(dto));
        Xunit.Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task GetChainsByUserIdAsync_ShouldReturnChains()
    {
        // Arrange
        var dto = new ChainsRequestDto { Id = Guid.NewGuid(), PageNumber = 1, PageSize = 10 };
        var chains = new List<Chain> { new Chain { Id = Guid.NewGuid(), Title = "Test Chain" } };
        var totalCount = chains.Count;
        var chainDtos = new List<ChainDto> { new ChainDto { Id = chains[0].Id, Title = "Test Chain" } };
        mockChainRepo.Setup(r => r.GetAllChainsByUserAsync(dto.Id, dto.PageNumber, dto.PageSize))
            .ReturnsAsync((chains, totalCount));
        mockMapper.Setup(m => m.Map<IEnumerable<ChainDto>>(chains)).Returns(chainDtos);
        
        // Act
        var result = await service.GetChainsByUserIdAsync(dto);
        
        // Assert
        Xunit.Assert.Equal(totalCount, result.TotalCount);
        Xunit.Assert.Equal(chainDtos, result.Data);
    }

    [Fact]
    public async Task GetChainsByUserIdAsync_WhenGetAllChainsByUserAsyncFails_ThrowsException()
    {
        // Arrange
        var dto = new ChainsRequestDto { Id = Guid.NewGuid(), PageNumber = 1, PageSize = 10 };
        mockChainRepo.Setup(r => r.GetAllChainsByUserAsync(dto.Id, dto.PageNumber, dto.PageSize))
            .ThrowsAsync(new Exception("Database error"));
        
        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<Exception>(() => service.GetChainsByUserIdAsync(dto));
        Xunit.Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task GetPublicChainsByUserIdAsync_ShouldReturnChains()
    {
        // Arrange
        var dto = new ChainsRequestDto { Id = Guid.NewGuid(), PageNumber = 1, PageSize = 10 };
        var chains = new List<Chain> { new Chain { Id = Guid.NewGuid(), Title = "Test Chain" } };
        var totalCount = chains.Count;
        var chainDtos = new List<ChainDto> { new ChainDto { Id = chains[0].Id, Title = "Test Chain" } };
        mockChainRepo.Setup(r => r.GetChainsByUserAsync(dto.Id, true, dto.PageNumber, dto.PageSize))
            .ReturnsAsync((chains, totalCount));
        mockMapper.Setup(m => m.Map<IEnumerable<ChainDto>>(chains)).Returns(chainDtos);
        
        // Act
        var result = await service.GetPublicChainsByUserIdAsync(dto);
        
        // Assert
        Xunit.Assert.Equal(totalCount, result.TotalCount);
        Xunit.Assert.Equal(chainDtos, result.Data);
    }

    [Fact]
    public async Task GetPublicChainsByUserIdAsync_WhenGetChainsByUserAsyncFails_ThrowsException()
    {
        // Arrange
        var dto = new ChainsRequestDto { Id = Guid.NewGuid(), PageNumber = 1, PageSize = 10 };
        mockChainRepo.Setup(r => r.GetChainsByUserAsync(dto.Id, true, dto.PageNumber, dto.PageSize))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<Exception>(() => service.GetPublicChainsByUserIdAsync(dto));
        Xunit.Assert.Equal("Database error", ex.Message);
    }

    [Fact]
    public async Task IncreaseStreakAsync_ShouldUpdateChain()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        var chain = new Chain { Id = chainId, CurrentStreak = 1, LongStreak = 2, IsBroken = true };
        mockChainRepo.Setup(r => r.GetByIdAsync(chainId)).ReturnsAsync(chain);

        // Act
        await service.IncreaseStreakAsync(chainId);

        // Assert
        Xunit.Assert.Equal(2, chain.CurrentStreak);
        Xunit.Assert.Equal(2, chain.LongStreak);
        Xunit.Assert.False(chain.IsBroken);
        mockChainRepo.Verify(r => r.UpdateAsync(chain), Times.Once);
    }

    [Fact]
    public async Task IncreaseStreakAsync_WhenChainNotFound_ThrowsNotFoundException()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        mockChainRepo.Setup(r => r.GetByIdAsync(chainId)).ReturnsAsync((Chain)null!);
        
        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<NotFoundException>(() => service.IncreaseStreakAsync(chainId));
        Xunit.Assert.Equal($"{nameof(Chain)} with id: {chainId} not found.", ex.Message);
    }

    [Fact]
    public async Task ResetStreakAsync_ShouldUpdateChain()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        var chain = new Chain { Id = chainId, CurrentStreak = 5, LongStreak = 10, IsBroken = false };
        mockChainRepo.Setup(r => r.GetByIdAsync(chainId)).ReturnsAsync(chain);
        
        // Act
        await service.ResetStreakAsync(chainId);
        
        // Assert
        Xunit.Assert.Equal(0, chain.CurrentStreak);
        Xunit.Assert.Equal(10, chain.LongStreak);
        Xunit.Assert.True(chain.IsBroken);
        mockChainRepo.Verify(r => r.UpdateAsync(chain), Times.Once);
    }

    [Fact]
    public async Task ResetStreakAsync_WhenChainNotFound_ThrowsNotFoundException()
    {
        // Arrange
        Guid chainId = Guid.NewGuid();
        mockChainRepo.Setup(r => r.GetByIdAsync(chainId)).ReturnsAsync((Chain)null!);

        // Act & Assert
        var ex = await Xunit.Assert.ThrowsAsync<NotFoundException>(() => service.ResetStreakAsync(chainId));
        Xunit.Assert.Equal($"{nameof(Chain)} with id: {chainId} not found.", ex.Message);
    }

}