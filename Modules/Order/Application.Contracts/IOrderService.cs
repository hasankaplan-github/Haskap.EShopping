using Haskap.DddBase.Application.Contracts;
using Modules.Order.Application.Dtos;

namespace Modules.Order.Application.Contracts;

public interface IOrderService : IUseCaseService
{
    Task<string> CreateOrderAsync(CreateInputDto input, CancellationToken cancellationToken = default);
}
