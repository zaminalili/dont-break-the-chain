using API.Attributes;
using Application.DTOs;
using Application.Services.Abstract;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    [ValidateModel]
    public class UsersController(IChainService chainService, IChainEntryService chainEntryService, IImageValidator imageValidator) : ControllerBase
    {
        [HttpPost]
        [Route("{userId:guid}/chains")]
        public async Task<IActionResult> Create([FromRoute] Guid userId, [FromBody] CreateChainDto request)
        {
            request.UserId = userId;
            await chainService.CreateChainAsync(request);

            return Ok("Chain created successfully");
        }

        [HttpGet]
        [Route("{userId:guid}/chains")]
        public async Task<ActionResult<ResponseDto<IEnumerable<ChainDto>>>> GetChainsByUserId([FromRoute] Guid userId, [FromQuery] ChainsRequestDto request)
        {
            request.Id = userId;
            var response = await chainService.GetChainsByUserIdAsync(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("{userId:guid}/chains/{chainId}")]
        public async Task<ActionResult<ResponseDto<ChainDto>>> GetChainById([FromRoute] Guid userId, [FromRoute] Guid chainId)
        {
            var response = await chainService.GetChainByIdAsync(chainId);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{userId:guid}/chains/{chainId}")]
        public async Task<IActionResult> DeleteChain([FromRoute] Guid userId, [FromRoute] Guid chainId)
        {
            await chainService.DeleteChainAsync(chainId);
            return Ok("Chain deleted successfully");
        }

        [HttpPut]
        [Route("{userId:guid}/chains/{chainId}")]
        public async Task<ActionResult<ChainDto>> UpdateChain([FromRoute] Guid userId, [FromRoute] Guid chainId, [FromBody] UpdateChainDto request)
        {
            request.Id = chainId;
            var response = await chainService.UpdateChainAsync(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("{userId:guid}/chains/{chainId}/check-in")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CheckInResponseDto>> CheckIn([FromForm] CheckInDto dto)
        {

            var isMatch = await imageValidator.IsMatchAsync(dto.Image, dto.CategoryName);

            var response = new CheckInResponseDto
            {
                CheckinStatus = isMatch
            };

            if (isMatch == false)
                return BadRequest(response);


            await chainService.IncreaseStreakAsync(dto.ChainId);
            await chainEntryService.CreateChainEntryAsync(new CreateChainEntryDto
            {
                ChainId = dto.ChainId,
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
