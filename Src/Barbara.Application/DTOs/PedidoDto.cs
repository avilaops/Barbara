namespace Barbara.Application.DTOs;

public class PedidoDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string NumeroPedido { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal ValorFrete { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = new();
}

public class CreatePedidoDto
{
    public Guid ClienteId { get; set; }
    public List<CreateItemPedidoDto> Itens { get; set; } = new();
    public decimal? ValorFrete { get; set; }
    public decimal? Desconto { get; set; }
}

public class ItemPedidoDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Tamanho { get; set; }
    public string? Cor { get; set; }
}

public class CreateItemPedidoDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string? Tamanho { get; set; }
    public string? Cor { get; set; }
}

public class UpdateStatusDto
{
    public string Status { get; set; } = string.Empty;
}
