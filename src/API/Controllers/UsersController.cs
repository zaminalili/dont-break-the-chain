using API.Attributes;
using Application.DTOs;
using Application.Services.Abstract;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace API.Controllers
{
    [EnableRateLimiting("FixedPolicy")]
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    public class UsersController(IChainService chainService, IChainEntryService chainEntryService, IImageValidator imageValidator) : ControllerBase
    {
        [HttpPost("{userId:guid}/chains")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromRoute] Guid userId, 
            [FromBody] CreateChainDto request)
        {

            await chainService.CreateChainAsync(userId, request);

            return CreatedAtAction("GetChainsByUserIdAsync", new { Id = userId});
        }


        [HttpGet("{userId:guid}/chains")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ChainDto>>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<IEnumerable<ChainDto>>>> GetChainsByUserId(
            [FromRoute] Guid userId, 
            [FromQuery] ChainsRequestDto request)
        {
            request.Id = userId;
            var response = await chainService.GetChainsByUserIdAsync(request);
            return Ok(response);
        }

        
        [HttpGet("{userId:guid}/chains/{chainId}")]
        [ProducesResponseType(typeof(ResponseDto<ChainDto>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<ChainDto>>> GetChainById(
            [FromRoute] Guid userId, 
            [FromRoute] Guid chainId)
        {
            var response = await chainService.GetChainByIdAsync(chainId);
            return Ok(response);
        }

        
        [HttpDelete("{userId:guid}/chains/{chainId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteChain(
            [FromRoute] Guid userId, 
            [FromRoute] Guid chainId)
        {
            await chainService.DeleteChainAsync(chainId);
            return Ok("Chain deleted successfully");
        }

        
        [HttpPut("{userId:guid}/chains/{chainId}")]
        [ProducesResponseType(typeof(ChainDto), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChainDto>> UpdateChain(
            [FromRoute] Guid userId, 
            [FromRoute] Guid chainId, 
            [FromBody] UpdateChainDto request)
        {
            request.Id = chainId;
            var response = await chainService.UpdateChainAsync(request);
            return Ok(response);
        }

        
        [HttpPost(("{userId:guid}/chains/{chainId}/check-in"))]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CheckInResponseDto), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(CheckInResponseDto), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CheckInResponseDto>> CheckIn(
            [FromRoute] Guid userId,
            [FromRoute] Guid chainId,
            [FromForm] CheckInDto dto)
        {

            var isMatch = await imageValidator.IsMatchAsync(dto.Image, dto.CategoryName);

            var response = new CheckInResponseDto
            {
                CheckinStatus = isMatch
            };

            if (isMatch == false)
                return BadRequest(response);


            await chainService.IncreaseStreakAsync(chainId);
            await chainEntryService.CreateChainEntryAsync(new CreateChainEntryDto
            {
                ChainId = chainId,
                Date = dto.Date,
                Note = dto.Note,
            });

            return Ok(response);
        }

        
        [HttpGet("current")]
        public ActionResult<CurrentUserDto> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return new CurrentUserDto
            {
                Id = userId,
                Email = email,
                UserName = userName,
            };
        }
    }
}
