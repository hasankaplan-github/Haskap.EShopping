using Haskap.DddBase.Application.Dtos;
using Haskap.DddBase.Domain.Common;
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
    public MoneyOutputDto ShippingFee { get; set; } = new() { Currency = Money.Zero.Currency, Value = Money.Zero.Value, StringRepresentation = Money.Zero.ToString() };
    public MoneyOutputDto Total
    {
        get
        {
            var total = new Money(Items.Sum(i => i.Total.Value));
            return new()
            {
                Currency = total.Currency,
                Value = total.Value,
                StringRepresentation = total.ToString()
            };
        }
    }
    public MoneyOutputDto TotalWithDiscount
    {
        get
        {
            var totalWithDiscount = new Money(Items.Sum(x => x.TotalWithDiscount.Value));
            return new()
            {
                Currency = totalWithDiscount.Currency,
                Value = totalWithDiscount.Value,
                StringRepresentation = totalWithDiscount.ToString()
            };
        }
    }
}
