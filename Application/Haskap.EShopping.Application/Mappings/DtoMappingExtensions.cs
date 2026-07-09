using Haskap.EShopping.Application.Dtos.Common;
using Haskap.EShopping.Domain.Common;

namespace Haskap.EShopping.Application.Mappings;

public static class DtoMappingExtensions
{
    extension(Money money)
    {
        public MoneyOutputDto ToMoneyOutputDto()
        {
            return new()
            {
                Value = money.Value,
                Currency = money.Currency,
                StringRepresentation = money.ToString()
            };
        }
    }
}
