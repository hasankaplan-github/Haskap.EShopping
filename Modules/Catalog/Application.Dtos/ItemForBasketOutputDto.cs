using Haskap.EShopping.Application.Dtos.Common;

namespace Modules.Catalog.Application.Dtos;

public class ItemForBasketOutputDto
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public string ProductName { get; set; }
    public string VariantSkuValue { get; set; }
    public string SlugValue { get; set; }
    public IList<Guid> CategoryIds { get; set; }
    public ProductPictureDto PrimaryPicture { get; set; }
    public MoneyOutputDto Price { get; set; }
    public int Quantity { get; set; }
    public MoneyOutputDto Total { get; set; }
    public MoneyOutputDto TotalWithDiscount { get; set; }
}
