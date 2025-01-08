using Microsoft.AspNetCore.Mvc;
using Test_Consimple.Models.PurchaseItemModels;
using Test_Consimple.Service;

namespace Test_Consimple.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseItemController : ControllerBase
{
    private readonly IPurchaseItemService _purchaseItemService;

    public PurchaseItemController(IPurchaseItemService purchaseItemService)
    {
        _purchaseItemService = purchaseItemService;
    }

    [HttpGet("{purchaseId}")]
    public async Task<IActionResult> GetAll(Guid purchaseId)
    {
        var purchaseItems = await _purchaseItemService.GetAllAsync(purchaseId);
        return Ok(purchaseItems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var purchaseItem = await _purchaseItemService.GetByIdAsync(id);
            return Ok(purchaseItem);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{purchaseId}")]
    public async Task<IActionResult> Create(Guid purchaseId, [FromBody] CreatePurchaseItemRequest request)
    {
        await _purchaseItemService.CreateAsync(purchaseId, request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseItemRequest request)
    {
        try
        {
            await _purchaseItemService.UpdateAsync(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _purchaseItemService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}