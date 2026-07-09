using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Basket.Domain.BasketAggregate;

namespace Modules.Basket.Domain;

public interface IBasketDbContext: IUnitOfWork
{
    DbSet<BasketAggregate.Basket> Basket { get; set; }
    DbSet<BasketItem> BasketItem { get; set; }
}
