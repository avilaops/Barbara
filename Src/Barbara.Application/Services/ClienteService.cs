using Barbara.Application.DTOs;
using Barbara.Domain.Entities;
using Barbara.Infrastructure.Repositories;

namespace Barbara.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IRepository<Cliente> _repository;

    public ClienteService(IRepository<Cliente> repository)
    {
        _repository = repository;
    }

    public async Task<ClienteDto> GetByIdAsync(Guid id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        return cliente == null ? null : MapToDto(cliente);
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
    {
        var clientes = await _repository.GetAllAsync();
        return clientes.Select(MapToDto);
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
    {
        // Validações
        if (await EmailExistsAsync(dto.Email))
            throw new InvalidOperationException("Email já cadastrado");

        if (await CpfExistsAsync(dto.CPF))
            throw new InvalidOperationException("CPF já cadastrado");

        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            CPF = dto.CPF,
            DataCadastro = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(cliente);
        return MapToDto(created);
    }

    public async Task UpdateAsync(Guid id, UpdateClienteDto dto)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente {id} não encontrado");

        cliente.Nome = dto.Nome;
        cliente.Telefone = dto.Telefone;

        await _repository.UpdateAsync(id, cliente);
    }

    public async Task DeleteAsync(Guid id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente {id} não encontrado");

        await _repository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var cliente = await _repository.GetByIdAsync(id);
        return cliente != null;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var clientes = await _repository.FindAsync(c => c.Email == email);
        return clientes.Any();
    }

    public async Task<bool> CpfExistsAsync(string cpf)
    {
        var clientes = await _repository.FindAsync(c => c.CPF == cpf);
        return clientes.Any();
    }

    public async Task UpdateMedidasAsync(Guid id, MedidasDto dto)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente {id} não encontrado");

        cliente.Altura = dto.Altura;
        cliente.Peso = dto.Peso;
        cliente.Manequim = dto.Manequim;

        await _repository.UpdateAsync(id, cliente);
    }

    public async Task<EnderecoDto> AddEnderecoAsync(Guid id, CreateEnderecoDto dto)
    {
        var cliente = await _repository.GetByIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente {id} não encontrado");

        var endereco = new Endereco
        {
            CEP = dto.CEP,
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Principal = dto.Principal
        };

        // Se for principal, desmarcar outros
        if (endereco.Principal)
        {
            foreach (var e in cliente.Enderecos)
                e.Principal = false;
        }

        cliente.Enderecos.Add(endereco);
        await _repository.UpdateAsync(id, cliente);

        return new EnderecoDto
        {
            CEP = endereco.CEP,
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Complemento = endereco.Complemento,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            Principal = endereco.Principal
        };
    }

    private static ClienteDto MapToDto(Cliente cliente) => new()
    {
        Id = cliente.Id,
        Nome = cliente.Nome,
        Email = cliente.Email,
        Telefone = cliente.Telefone,
        CPF = cliente.CPF,
        DataCadastro = cliente.DataCadastro,
        Altura = cliente.Altura,
        Peso = cliente.Peso,
        Manequim = cliente.Manequim,
        Enderecos = cliente.Enderecos.Select(e => new EnderecoDto
        {
            CEP = e.CEP,
            Logradouro = e.Logradouro,
            Numero = e.Numero,
            Complemento = e.Complemento,
            Bairro = e.Bairro,
            Cidade = e.Cidade,
            Estado = e.Estado,
            Principal = e.Principal
        }).ToList()
    };
}
