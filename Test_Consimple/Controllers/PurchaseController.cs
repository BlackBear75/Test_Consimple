using Microsoft.AspNetCore.Mvc;
using Test_Consimple.Models.PurchaseModels;
using Test_Consimple.Service;

namespace Test_Consimple.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    
    [HttpGet("recent-buyers")]
    public async Task<IActionResult> GetRecentBuyers([FromQuery] int days)
    {
        if (days < 0)
        {
            return BadRequest("The 'days' parameter must be a positive integer or 0.");
        }
        var clients = await _purchaseService.GetRecentBuyersAsync(days);
        return Ok(clients);
    }

    
    [HttpGet("{clientId}/popular-categories")]
    public async Task<IActionResult> GetPopularCategories(Guid clientId)
    {
        var categories = await _purchaseService.GetPopularCategoriesAsync(clientId);
        return Ok(categories);
    }
    
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var purchases = await _purchaseService.GetAllAsync();
        return Ok(purchases);
    }

    [HttpGet("{id}/GetById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var purchase = await _purchaseService.GetByIdAsync(id);
            return Ok(purchase);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseRequest request)
    {
        await _purchaseService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}/Update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseRequest request)
    {
        try
        {
            await _purchaseService.UpdateAsync(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}/Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _purchaseService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}