using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Discount.Domain.CouponAggregate;

namespace Modules.Discount.Domain;

public interface IDiscountDbContext : IUnitOfWork
{
    DbSet<Coupon> Coupon { get; set; }
}
