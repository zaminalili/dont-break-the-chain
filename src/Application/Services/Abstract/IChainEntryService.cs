using Application.DTOs;

namespace Application.Services.Abstract;

public interface IChainEntryService
{
    Task CreateChainEntryAsync(CreateChainEntryDto dto);
    Task DeleteChainEntryAsync(Guid id);
    Task<ResponseDto<ChainEntryDto>> GetChainEntryByIdAsync(Guid id);
    Task<ResponseDto<IEnumerable<ChainEntryDto>>> GetAllChainEntriesByChainIdAsync(ChainsRequestDto dto);
 
}
