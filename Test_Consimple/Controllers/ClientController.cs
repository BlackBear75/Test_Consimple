using Microsoft.AspNetCore.Mvc;
using Test_Consimple.Models.ClientModels;
using Test_Consimple.Service;

namespace Test_Consimple.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clients = await _clientService.GetAllAsync();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var client = await _clientService.GetByIdAsync(id);
            return Ok(client);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
    {
        await _clientService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientRequest request)
    {
        try
        {
            await _clientService.UpdateAsync(id, request);
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
            await _clientService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}