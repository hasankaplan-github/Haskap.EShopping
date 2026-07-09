using Haskap.DddBase.Application;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Contracts;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Application;

internal class StockCheckerService : UseCaseService, IStockCheckerService
{
    private readonly ICatalogDbContext _catalogDbContext;

    public StockCheckerService(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<bool> IsStockAvailableAsync(Guid productId, Guid productVariantId, int requestedQuantity, CancellationToken cancellationToken = default)
    {
        var requestedVariant = await _catalogDbContext.Product
            .AsNoTracking()
            .Include(x => x.Variants.Where(y => y.Id == productVariantId))
            .Where(p => p.Id == productId)
            .Select(x => x.Variants.Where(y => y.Id == productVariantId).First())
            .FirstOrDefaultAsync(cancellationToken);

        return requestedVariant?.StockQuantity.IsAvailable(requestedQuantity) ?? false;
    }
}
