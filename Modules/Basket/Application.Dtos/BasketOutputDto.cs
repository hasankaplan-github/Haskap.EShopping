using Haskap.EShopping.Application.Dtos.Common;
using Haskap.EShopping.Application.Mappings;
using Haskap.EShopping.Domain.Common;
using Modules.Catalog.Application.Dtos;
using Modules.Discount.Application.Dtos;

namespace Modules.Basket.Application.Dtos;

public class BasketOutputDto
{
    public Guid Id { get; set; }
    public List<ItemForBasketOutputDto> Items { get; set; } = [];
    public List<Guid> AppliedSpecialCouponIds { get; set; }
    public List<Guid> RemovedRegularCouponIds { get; set; }
    public List<CouponForBasketOutputDto> AllPossibleCoupons { get; set; } = [];
    public bool HasFreeShippingCoupon { get; set; }
    public MoneyOutputDto ShippingFee { get; set; } = Money.Zero.ToMoneyOutputDto();
    public MoneyOutputDto Total
    {
        get
        {
            return new Money(Items.Sum(i => i.Total.Value)).ToMoneyOutputDto();
        }
    }
    public MoneyOutputDto TotalWithDiscount
    {
        get
        {
            return new Money(Items.Sum(x => x.TotalWithDiscount.Value)).ToMoneyOutputDto();
        }
    }
}
