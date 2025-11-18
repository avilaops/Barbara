using Barbara.Application.Services;
using Barbara.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Barbara.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidosController(IPedidoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos([FromQuery] Guid? clienteId)
    {
        var pedidos = clienteId.HasValue
            ? await _service.GetByClienteIdAsync(clienteId.Value)
            : await _service.GetAllAsync();

        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDto>> GetPedido(Guid id)
    {
        var pedido = await _service.GetByIdAsync(id);
        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoDto>> CriarPedido(CreatePedidoDto dto)
    {
        var pedido = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(Guid id, [FromBody] UpdateStatusDto dto)
    {
        await _service.UpdateStatusAsync(id, dto.Status);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelarPedido(Guid id)
    {
        await _service.CancelarAsync(id);
        return NoContent();
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidosPorCliente(Guid clienteId)
    {
        var pedidos = await _service.GetByClienteIdAsync(clienteId);
        return Ok(pedidos);
    }
}
