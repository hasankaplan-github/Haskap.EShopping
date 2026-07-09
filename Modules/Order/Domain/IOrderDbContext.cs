using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;

namespace Modules.Order.Domain;

public interface IOrderDbContext: IUnitOfWork
{
    DbSet<OrderAggregate.Order> Order { get; set; }
}
