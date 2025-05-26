using Application.DTOs;
using Application.Services.Abstract;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Concrete;

internal partial class ChainService : IChainService
{
    // The private members and constructor are in ChainServicePrivateMembers.cs


    public async Task CreateChainAsync(Guid userId, CreateChainDto dto)
    {
        logger.LogInformation("Creating chain for user {UserId}", userId);
        logger.LogInformation("Mapping CreateChainDto to Chain entity");
        var chain = mapper.Map<Chain>(dto);
        chain.UserId = userId;

        await chainRepository.AddAsync(chain);
        logger.LogInformation("Chain created successfully with ID {ChainId}", chain.Id);

    }

    public async Task DeleteChainAsync(Guid chainId)
    {
        logger.LogInformation("Deleting chain with ID {ChainId}", chainId);
        await GetChainOrThrowAsync(chainId);

        logger.LogInformation("Chain found, deleting chain with ID {ChainId}", chainId);
        await chainRepository.DeleteAsync(chainId);
    }

    public async Task<ResponseDto<ChainDto>> GetChainByIdAsync(Guid chainId)
    {
        logger.LogInformation("Getting chain with ID {ChainId}", chainId);
        Chain chain = await GetChainOrThrowAsync(chainId);

        logger.LogInformation("Chain found, returning ChainDto");
        logger.LogInformation("Mapping Chain entity to ChainDto");
        var chainDto = mapper.Map<ChainDto>(chain);
        
        return new ResponseDto<ChainDto>() 
        { 
            Data = chainDto,
            TotalCount = 1
        };
    }

    public async Task<ResponseDto<IEnumerable<ChainDto>>> GetChainsByCategoryIdAsync(ChainsRequestDto dto)
    {
        logger.LogInformation("Getting chains by category ID {CategoryId}", dto.Id);
        var (chains, totalCount) = await chainRepository
            .GetPublicChainsByCategoryAsync(dto.Id, dto.PageNumber, dto.PageSize);

        return ToResponse<ChainDto>(chains, totalCount);
    }


    public async Task<ResponseDto<IEnumerable<ChainDto>>> GetChainsByUserIdAsync(ChainsRequestDto dto)
    {
        logger.LogInformation("Getting chains by user ID {userId}", dto.Id);
        var (chains, totalCount) = await chainRepository
            .GetAllChainsByUserAsync(dto.Id, dto.PageNumber, dto.PageSize);

        return ToResponse<ChainDto>(chains, totalCount);
    }

    public async Task<ResponseDto<IEnumerable<ChainDto>>> GetPublicChainsByUserIdAsync(ChainsRequestDto dto)
    {
        logger.LogInformation("Getting chains by user ID {userId}", dto.Id);
        var (chains, totalCount) = await chainRepository
            .GetChainsByUserAsync(dto.Id, isPublic: true, dto.PageNumber, dto.PageSize);

        return ToResponse<ChainDto>(chains, totalCount);
    }

    public async Task IncreaseStreakAsync(Guid chainId)
    {
        Chain chain = await GetChainOrThrowAsync(chainId);

        logger.LogInformation("Increasing streak for chain with ID {ChainId}", chainId);

        chain.CurrentStreak++;
        chain.LongStreak = Math.Max(chain.CurrentStreak, chain.LongStreak);
        chain.IsBroken = false;
        
        await chainRepository.UpdateAsync(chain);
    }

    public async Task ResetStreakAsync(Guid chainId)
    {
        Chain chain = await GetChainOrThrowAsync(chainId);

        logger.LogInformation("Increasing streak for chain with ID {ChainId}", chainId);

        chain.CurrentStreak = 0;
        chain.IsBroken = true;
        
        await chainRepository.UpdateAsync(chain);
    }


    public async Task<ChainDto> UpdateChainAsync(UpdateChainDto dto)
    {
        logger.LogInformation("Updating chain with ID {ChainId}", dto.Id);
        var chain = await GetChainOrThrowAsync(dto.Id);

        logger.LogInformation("Mapping UpdateChainDto to Chain entity");
        mapper.Map(dto, chain);
        
        await chainRepository.UpdateAsync(chain);
        logger.LogInformation("Chain updated successfully with ID {ChainId}", chain.Id);

        return mapper.Map<ChainDto>(chain);
    }
}
