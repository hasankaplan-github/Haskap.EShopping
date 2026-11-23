using NpgsqlTypes;

namespace Modules.Catalog.Domain.ProductAggregate;

public class ProductSearchView
{
    public Guid ProductId { get; set; }
    public NpgsqlTsVector SearchVectorTurkish { get; set; }
}
