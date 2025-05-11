using Application.DTOs;

namespace Application.Services.Abstract;

public interface IChainService
{
    Task<ResponseDto<ChainDto>> GetChainByIdAsync(Guid chainId);
    Task<ResponseDto<IEnumerable<ChainDto>>> GetChainsByUserIdAsync(ChainsRequestDto dto);
    Task<ResponseDto<IEnumerable<ChainDto>>> GetPublicChainsByUserIdAsync(ChainsRequestDto dto);
    Task<ResponseDto<IEnumerable<ChainDto>>> GetChainsByCategoryIdAsync(ChainsRequestDto dto);

    Task CreateChainAsync(CreateChainDto dto);
    Task<ChainDto> UpdateChainAsync(UpdateChainDto dto);
    Task DeleteChainAsync(Guid chainId);
    Task IncreaseStreakAsync(Guid chainId);
    Task ResetStreakAsync(Guid chainId);
}
