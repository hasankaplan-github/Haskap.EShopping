using Haskap.EShopping.Domain;

namespace Modules.Catalog.Domain.CategoryAggregate;

public class ProductCategory : Entity
{
    private ProductCategory() { }

    public ProductCategory(Guid id)
        : base(id) { }

    public Guid CategoryId { get; set; }
    public Guid ProductId { get; set; }
}
