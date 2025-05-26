using Application.DTOs;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<CategoryDto>> GetAllActive()
    {
        var categories = await categoryService.GetAllAsync();

        return Ok(categories);
    }
}
