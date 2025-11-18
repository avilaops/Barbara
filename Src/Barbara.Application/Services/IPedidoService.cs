using Barbara.Application.DTOs;

namespace Barbara.Application.Services;

public interface IPedidoService
{
    Task<PedidoDto> GetByIdAsync(Guid id);
    Task<IEnumerable<PedidoDto>> GetAllAsync();
    Task<IEnumerable<PedidoDto>> GetByClienteIdAsync(Guid clienteId);
    Task<PedidoDto> CreateAsync(CreatePedidoDto dto);
    Task UpdateStatusAsync(Guid id, string status);
    Task CancelarAsync(Guid id);
}
