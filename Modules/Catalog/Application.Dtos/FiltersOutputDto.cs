namespace Modules.Catalog.Application.Dtos;

public class FiltersOutputDto
{
    public IReadOnlyList<ProductAttributeOutputDto> AvailableAttributes { get; set; }
    public decimal MinPriceValue { get; set; }
    public decimal MaxPriceValue { get; set; }
}
