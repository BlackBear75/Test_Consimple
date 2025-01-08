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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var purchases = await _purchaseService.GetAllAsync();
        return Ok(purchases);
    }

    [HttpGet("{id}")]
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseRequest request)
    {
        await _purchaseService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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