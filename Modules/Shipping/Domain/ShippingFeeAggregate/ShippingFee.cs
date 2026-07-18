using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Shipping.Domain.ShippingFeeAggregate;

public class ShippingFee : AggregateRoot
{
    public Guid CityId { get; private set; }
    public Guid DistrictId { get; private set; }
    public Guid NeighborhoodId { get; private set; }
    public Money Price { get; private set; }

    private ShippingFee()
    {}

    public ShippingFee(Guid id, Guid cityId, Guid districtId, Guid neighborhoodId, Money price)
        : base(id)
    {
        Guard.Against.Null(price, nameof(price));

        CityId = cityId;
        DistrictId = districtId;
        NeighborhoodId = neighborhoodId;
        Price = price;
    }
}
