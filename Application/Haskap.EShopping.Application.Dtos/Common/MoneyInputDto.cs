using Haskap.EShopping.Domain.Shared.Enums;

namespace Haskap.EShopping.Application.Dtos.Common;
public class MoneyInputDto
{
    public decimal Value { get; set; }
    public Currency Currency { get; set; }
}
