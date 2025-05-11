using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services.Concrete;

internal partial class ChainService
{
    private readonly ILogger<ChainService> logger;
    private readonly IMapper mapper;
    private readonly IChainRepository chainRepository;
    
    public ChainService(ILogger<ChainService> logger, IChainRepository chainRepository, IMapper mapper)
    {
        this.logger = logger;
        this.chainRepository = chainRepository;
        this.mapper = mapper;
    }

    private ResponseDto<IEnumerable<T>> ToResponse<T>(IEnumerable<Chain> chains, int totalCount)
    {
        logger.LogInformation("Chains found, mapping to ChainDto");
        var chainsDto = mapper.Map<IEnumerable<T>>(chains);

        logger.LogInformation("Mapping completed, returning response");
        return new ResponseDto<IEnumerable<T>>()
        {
            Data = chainsDto,
            TotalCount = totalCount
        };
    }

    private async Task<Chain> GetChainOrThrowAsync(Guid chainId)
    {
        return await chainRepository.GetByIdAsync(chainId)
            ?? throw new NotFoundException(nameof(Chain), chainId);
    }
}
