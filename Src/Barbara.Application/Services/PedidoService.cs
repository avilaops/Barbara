using Barbara.Application.DTOs;
using Barbara.Domain.Entities;
using Barbara.Infrastructure.Repositories;

namespace Barbara.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IRepository<Pedido> _pedidoRepository;
    private readonly IRepository<Cliente> _clienteRepository;
    private readonly IRepository<Produto> _produtoRepository;

    public PedidoService(
        IRepository<Pedido> pedidoRepository,
        IRepository<Cliente> clienteRepository,
        IRepository<Produto> produtoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<PedidoDto> GetByIdAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(id);
        return pedido == null ? null : MapToDto(pedido);
    }

    public async Task<IEnumerable<PedidoDto>> GetAllAsync()
    {
        var pedidos = await _pedidoRepository.GetAllAsync();
        return pedidos.Select(MapToDto);
    }

    public async Task<IEnumerable<PedidoDto>> GetByClienteIdAsync(Guid clienteId)
    {
        var pedidos = await _pedidoRepository.FindAsync(p => p.ClienteId == clienteId);
        return pedidos.Select(MapToDto);
    }

    public async Task<PedidoDto> CreateAsync(CreatePedidoDto dto)
    {
        // Validar cliente
        var cliente = await _clienteRepository.GetByIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new InvalidOperationException("Cliente não encontrado");

        // Criar pedido
        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            NumeroPedido = GerarNumeroPedido(),
            DataPedido = DateTime.UtcNow,
            Status = StatusPedido.Pendente,
            SubTotal = 0,
            ValorFrete = dto.ValorFrete ?? 0,
            Desconto = dto.Desconto ?? 0
        };

        // Processar itens
        decimal subtotal = 0;
        foreach (var itemDto in dto.Itens)
        {
            var produto = await _produtoRepository.GetByIdAsync(itemDto.ProdutoId);
            if (produto == null)
                throw new InvalidOperationException($"Produto {itemDto.ProdutoId} não encontrado");

            if (!produto.Ativo)
                throw new InvalidOperationException($"Produto {produto.Nome} não está disponível");

            var item = new ItemPedido
            {
                ProdutoId = itemDto.ProdutoId,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.Preco,
                Subtotal = produto.Preco * itemDto.Quantidade,
                Tamanho = itemDto.Tamanho,
                Cor = itemDto.Cor
            };

            pedido.Itens.Add(item);
            subtotal += item.Subtotal;
        }

        pedido.SubTotal = subtotal;
        pedido.Total = subtotal + pedido.ValorFrete - pedido.Desconto;

        var created = await _pedidoRepository.AddAsync(pedido);
        return MapToDto(created);
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");

        if (!Enum.TryParse<StatusPedido>(status, true, out var statusEnum))
            throw new InvalidOperationException($"Status '{status}' inválido");

        pedido.Status = statusEnum;
        await _pedidoRepository.UpdateAsync(id, pedido);
    }

    public async Task CancelarAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");

        if (pedido.Status == StatusPedido.Entregue)
            throw new InvalidOperationException("Não é possível cancelar pedido já entregue");

        pedido.Status = StatusPedido.Cancelado;
        await _pedidoRepository.UpdateAsync(id, pedido);
    }

    private static string GerarNumeroPedido()
    {
        return $"PED-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
    }

    private static PedidoDto MapToDto(Pedido pedido) => new()
    {
        Id = pedido.Id,
        ClienteId = pedido.ClienteId,
        NumeroPedido = pedido.NumeroPedido,
        DataPedido = pedido.DataPedido,
        Status = pedido.Status.ToString(),
        SubTotal = pedido.SubTotal,
        ValorFrete = pedido.ValorFrete,
        Desconto = pedido.Desconto,
        Total = pedido.Total,
        Itens = pedido.Itens.Select(i => new ItemPedidoDto
        {
            ProdutoId = i.ProdutoId,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Subtotal = i.Subtotal,
            Tamanho = i.Tamanho,
            Cor = i.Cor
        }).ToList()
    };
}
