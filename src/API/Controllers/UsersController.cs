using API.Attributes;
using Application.DTOs;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("v1/[controller]/{userId:guid}")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    public class UsersController(IChainService chainService, IChainEntryService chainEntryService) : ControllerBase
    {
        [HttpPost]
        [Route("chains")]
        public async Task<IActionResult> Create([FromRoute] Guid userId, [FromBody] CreateChainDto request)
        {
            request.UserId = userId;
            await chainService.CreateChainAsync(request);

            return Ok("Chain created successfully");
        }

        [HttpGet]
        [Route("chains")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ChainDto>>>> GetChainsByUserId([FromRoute] Guid userId, [FromQuery] ChainsRequestDto request)
        {
            request.Id = userId;
            var response = await chainService.GetChainsByUserIdAsync(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("chains/{chainId}")]
        public async Task<ActionResult<ResponseDto<ChainDto>>> GetChainById([FromRoute] Guid userId, [FromRoute] Guid chainId)
        {
            var response = await chainService.GetChainByIdAsync(chainId);
            return Ok(response);
        }

        [HttpDelete]
        [Route("chains/{chainId}")]
        public async Task<IActionResult> DeleteChain([FromRoute] Guid userId, [FromRoute] Guid chainId)
        {
            await chainService.DeleteChainAsync(chainId);
            return Ok("Chain deleted successfully");
        }

        [HttpPut]
        [Route("chains/{chainId}")]
        public async Task<ActionResult<ChainDto>> UpdateChain([FromRoute] Guid userId, [FromRoute] Guid chainId, [FromBody] UpdateChainDto request)
        {
            request.Id = chainId;
            var response = await chainService.UpdateChainAsync(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("chains/{chainId}/check-in")]
        public async Task<IActionResult> CheckIn([FromRoute] Guid userId, [FromRoute] Guid chainId, [FromBody] CheckInDto checkInDto)
        {
            await chainService.IncreaseStreakAsync(chainId);
            await chainEntryService.CreateChainEntryAsync(new CreateChainEntryDto
            {
                ChainId = chainId,
                Date = checkInDto.Date,
            });

            return Ok("Check-in successfully");
        }
    }
}
