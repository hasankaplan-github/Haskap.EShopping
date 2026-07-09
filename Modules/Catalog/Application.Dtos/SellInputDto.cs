namespace Modules.Catalog.Application.Dtos;

public class SellInputDto
{
    public List<(Guid ProductId, Guid VariantId, int Quantity)> Items { get; set; } = [];
}
