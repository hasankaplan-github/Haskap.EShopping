using Haskap.DddBase.Application.Contracts;
using Modules.Catalog.Application.Dtos;

namespace Modules.Catalog.Application.Contracts;

public interface ICatalogService : IUseCaseService
{
    Task<ProductDto> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductDto> GetProductBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<SearchOutputDto> SearchAsync(SearchInputDto searchInput, CancellationToken cancellationToken = default);
    Task SellAsync(SellInputDto input, CancellationToken cancellationToken = default);
    Task<List<ItemForBasketOutputDto>> GetItemsForBasketAsync(Guid productId, IList<Guid> variantIds, CancellationToken cancellationToken = default);
    Task<ProductDetailsOutputDto> GetProductDetailsBySlugAsync(string slugValue, CancellationToken cancellationToken = default);
}