using Haskap.DddBase.Application.Contracts;

namespace Modules.Catalog.Application.Contracts;

public interface IStockCheckerService : IUseCaseService
{
    Task<bool> IsStockAvailableAsync(Guid productId,Guid productVariantId, int requestedQuantity, CancellationToken cancellationToken = default);
}
