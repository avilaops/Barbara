using Barbara.Application.Services;
using Barbara.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Barbara.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
    {
        var clientes = await _service.GetAllAsync();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> GetCliente(Guid id)
    {
        var cliente = await _service.GetByIdAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> CriarCliente(CreateClienteDto dto)
    {
        var cliente = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCliente(Guid id, UpdateClienteDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpPut("{id}/medidas")]
    public async Task<IActionResult> AtualizarMedidas(Guid id, MedidasDto dto)
    {
        await _service.UpdateMedidasAsync(id, dto);
        return NoContent();
    }

    [HttpPost("{id}/enderecos")]
    public async Task<ActionResult<EnderecoDto>> AdicionarEndereco(Guid id, CreateEnderecoDto dto)
    {
        var endereco = await _service.AddEnderecoAsync(id, dto);
        return CreatedAtAction(nameof(GetCliente), new { id }, endereco);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCliente(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
