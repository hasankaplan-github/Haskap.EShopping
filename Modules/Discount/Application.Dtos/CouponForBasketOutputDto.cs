using Haskap.EShopping.Application.Dtos.Common;

namespace Modules.Discount.Application.Dtos;

public class CouponForBasketOutputDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public MoneyOutputDto DiscountAmount { get; set; }
    public bool IsApplied { get; set; }
    public bool IsSpecialCoupon { get; set; }
    public string? Code { get; set; } = null;
}
