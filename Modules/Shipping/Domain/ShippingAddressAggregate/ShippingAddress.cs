using Haskap.EShopping.Domain;
using Modules.Shipping.Domain.CityAggregate;
using Modules.Shipping.Domain.DistrictAggregate;
using Modules.Shipping.Domain.NeighborhoodAggregate;

namespace Modules.Shipping.Domain.ShippingAddressAggregate;

public class ShippingAddress : AggregateRoot
{
    public Guid OwnerAccountId { get; set; }
    public Guid CityId { get; set; }
    public City City { get; set; }
    public Guid DistrictId { get; set; }
    public District District { get; set; }
    public Guid NeighborhoodId { get; set; }
    public Neighborhood Neighborhood { get; set; }
    public string Street { get; set; }
    public string? Postcode { get; set; }
    public string BuildingNo { get; set; }
    public int? Floor { get; set; }
    public int? ApartmentNo { get; set; }
    public string? AddressLine { get; set; }

    private ShippingAddress()
    {}

    public ShippingAddress(
        Guid id,
        Guid ownerAccountId,
        Guid cityId,
        Guid districtId,
        Guid neighborhoodId,
        string street,
        string? postcode,
        string buildingNo,
        int? floor,
        int? apartmentNo,
        string? addressLine)
        : base(id)
    {
        OwnerAccountId = ownerAccountId;
        CityId = cityId;
        DistrictId = districtId;
        NeighborhoodId = neighborhoodId;
        Street = street;
        Postcode = postcode;
        BuildingNo = buildingNo;
        Floor = floor;
        ApartmentNo = apartmentNo;
        AddressLine = addressLine;
    }
}
