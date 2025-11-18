using Barbara.Application.DTOs;

namespace Barbara.Application.Services;

public interface IClienteService
{
    Task<ClienteDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ClienteDto>> GetAllAsync();
    Task<ClienteDto> CreateAsync(CreateClienteDto dto);
    Task UpdateAsync(Guid id, UpdateClienteDto dto);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> CpfExistsAsync(string cpf);
    Task UpdateMedidasAsync(Guid id, MedidasDto dto);
    Task<EnderecoDto> AddEnderecoAsync(Guid id, CreateEnderecoDto dto);
}
