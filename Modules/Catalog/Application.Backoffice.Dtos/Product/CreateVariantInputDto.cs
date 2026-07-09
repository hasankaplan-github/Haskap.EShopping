using Haskap.DddBase.Application.Dtos.Common;

namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class CreateVariantInputDto
{
    public Guid ProductId { get; set; }
    public Guid? ColorAttributeId { get; set; }
    public Guid? SizeAttributeId { get; set; }
    public decimal PriceValue { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public int StockQuantityValue { get; set; }
    public List<FileInputDto> PhotoFiles { get; set; } = new();
}
