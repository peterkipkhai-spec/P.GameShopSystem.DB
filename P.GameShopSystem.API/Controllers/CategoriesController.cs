using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.GameShopSystem.API.Contracts.Categories;
using P.GameShopSystem.DB.Models;

namespace P.GameShopSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(GameShopDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories.AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(c.CategoryId, c.Name, c.Description))
            .ToListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateAsync(UpsertCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = new Category { Name = request.Name.Trim(), Description = request.Description?.Trim() };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetAllAsync), new { id = category.CategoryId }, new CategoryDto(category.CategoryId, category.Name, category.Description));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> UpdateAsync(int id, UpsertCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FindAsync([id], cancellationToken);
        if (category is null) return NotFound();
        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new CategoryDto(category.CategoryId, category.Name, category.Description));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FindAsync([id], cancellationToken);
        if (category is null) return NotFound();
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
