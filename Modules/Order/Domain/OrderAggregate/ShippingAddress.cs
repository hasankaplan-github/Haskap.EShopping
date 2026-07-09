using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Order.Domain.OrderAggregate;

public class ShippingAddress : ValueObject
{
    public Guid OwnerShippingAddressId { get; private set; }
    public string CityName { get; private set; }
    public string DistrictName { get; private set; }
    public string NeighborhoodName { get; private set; }
    public string Street { get; private set; }
    public string? Postcode { get; private set; }
    public string BuildingNo { get; private set; }
    public int? Floor { get; private set; }
    public int? ApartmentNo { get; private set; }
    public string? AddressLine { get; private set; }

    private ShippingAddress()
    {}

    public ShippingAddress(
        Guid ownerShippingAddressId, 
        string cityName,
        string districtName,
        string neighborhoodName,
        string street,
        string? postcode,
        string buildingNo,
        int? floor,
        int? apartmentNo,
        string? addressLine)
    {
        Guard.Against.NullOrWhiteSpace(cityName);
        Guard.Against.NullOrWhiteSpace(districtName);
        Guard.Against.NullOrWhiteSpace(neighborhoodName);
        Guard.Against.NullOrWhiteSpace(street);
        Guard.Against.NullOrWhiteSpace(buildingNo);

        OwnerShippingAddressId = ownerShippingAddressId;
        CityName = cityName;
        DistrictName = districtName;
        NeighborhoodName = neighborhoodName;
        Street = street;
        Postcode = postcode;
        BuildingNo = buildingNo;
        Floor = floor;
        ApartmentNo = apartmentNo;
        AddressLine = addressLine;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CityName;
        yield return DistrictName;
        yield return NeighborhoodName;
        yield return Street;
        yield return Postcode ?? string.Empty;
        yield return BuildingNo;
        yield return Floor ?? 0;
        yield return ApartmentNo ?? 0;
        yield return AddressLine ?? string.Empty;
    }
}
