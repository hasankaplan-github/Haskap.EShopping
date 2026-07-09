using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Discount.Domain.RegularCouponAggregate;
using Modules.Discount.Domain.SpecialCouponAggregate;

namespace Modules.Discount.Domain;

public interface IDiscountDbContext : IUnitOfWork
{
    DbSet<RegularCoupon> RegularCoupon { get; set; }
    DbSet<SpecialCoupon> SpecialCoupon { get; set; }
}
