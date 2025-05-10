using Application.DTOs;
using Application.Services.Abstract;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Application.Services.Concrete;

internal class ChainEntryService : IChainEntryService
{
    private readonly ILogger<ChainService> logger;
    private readonly IMapper mapper;
    private readonly IChainEntryRepository chainEntryRepository;

    public ChainEntryService(ILogger<ChainService> logger, IChainEntryRepository chainEntryRepository, IMapper mapper)
    {
        this.logger = logger;
        this.chainEntryRepository = chainEntryRepository;
        this.mapper = mapper;
    }

    private async Task<ChainEntry> GetChainEntryOrThrowAsync(Guid id)
    {
        return await chainEntryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(ChainEntry), id);
    }

    private ResponseDto<TDto> ConvertToResponseDto<TEntity, TDto>(TEntity data, int totalCount)
    {
        logger.LogInformation("Mapping chain entries to DTOs");
        var dataDto = mapper.Map<TDto>(data);

        return new ResponseDto<TDto>()
        {
            Data = dataDto,
            TotalCount = totalCount
        };
    }

    public async Task CreateChainEntryAsync(CreateChainEntryDto dto)
    {
        logger.LogInformation("Creating chain entry with data: {Data}", dto);
        var chainEntry = mapper.Map<ChainEntry>(dto);
        
        await chainEntryRepository.AddAsync(chainEntry);
        logger.LogInformation("Chain entry created with ID: {Id}", chainEntry.Id);
    }

    public async Task DeleteChainEntryAsync(Guid id)
    {
        logger.LogInformation("Deleting chain entry with ID: {Id}", id);
        await GetChainEntryOrThrowAsync(id);

        await chainEntryRepository.DeleteAsync(id);
        logger.LogInformation("Chain entry deleted with ID: {Id}", id);
    }

    public async Task<ResponseDto<IEnumerable<ChainEntryDto>>> GetAllChainEntriesByChainIdAsync(ChainsRequestDto dto)
    {
        logger.LogInformation("Getting all chain entries for chain ID: {ChainId}", dto.Id);
        var (chainEntries, totalCount) = await chainEntryRepository
            .GetByChainIdAsync(dto.Id, dto.PageNumber, dto.PageSize);

        return ConvertToResponseDto< IEnumerable<ChainEntry>, IEnumerable<ChainEntryDto> >(chainEntries, totalCount);
    }


    public async Task<ResponseDto<ChainEntryDto>> GetChainEntryByIdAsync(Guid id)
    {
        logger.LogInformation("Getting chain entry with ID: {Id}", id);
        var chainEntry = await GetChainEntryOrThrowAsync(id);

        return ConvertToResponseDto<ChainEntry, ChainEntryDto>(chainEntry, 1);
    }
}
