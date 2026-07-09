using Ardalis.GuardClauses;
using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Application.Dtos.Common;
using Haskap.EShopping.Application.Mappings;
using Haskap.EShopping.Domain.Common;
using Haskap.EShopping.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Modules.Basket.Application.Dtos;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;
using Modules.Shipping.Application.Mappings;
using Modules.Shipping.Domain;
using Modules.Shipping.Domain.ShippingAddressAggregate;

namespace Modules.Shipping.Application;

public class ShippingService : UseCaseService, IShippingService
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IShippingDbContext _shippingDbContext;
    private readonly IAnonymousAccountProvider _anonymousAccountProvider;

    public ShippingService(
        ICurrentUserIdProvider currentUserIdProvider,
        IShippingDbContext shippingDbContext,
        IAnonymousAccountProvider anonymousAccountProvider)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _shippingDbContext = shippingDbContext;
        _anonymousAccountProvider = anonymousAccountProvider;
    }

    public async Task ApplyShippingFeeAsync(BasketOutputDto basketOutput, ShippingAddressInputDto? shippingAddress, CancellationToken cancellationToken = default)
    {
        basketOutput.ShippingFee = await GetShippingFeeAsync(shippingAddress, cancellationToken);
    }

    public async Task<MoneyOutputDto> GetShippingFeeAsync(ShippingAddressInputDto? shippingAddressInput, CancellationToken cancellationToken = default)
    {
        if (_currentUserIdProvider.CurrentUserId is null &&
            (shippingAddressInput is null ||
            shippingAddressInput.CityId is null || 
            shippingAddressInput.DistrictId is null || 
            shippingAddressInput.NeighborhoodId is null))
        {
            return Money.Zero.ToMoneyOutputDto();
        }

        var shippingAddress = new ShippingAddressForShippingFeeInputDto()
        {
            CityId = shippingAddressInput!.CityId!.Value,
            DistrictId = shippingAddressInput!.DistrictId!.Value,
            NeighborhoodId = shippingAddressInput!.NeighborhoodId!.Value
        };

        // Calculate shipping fee based on the shipping address
        return await CalculateShippingFeeAsync(shippingAddress, cancellationToken);
    }

    public async Task<ShippingAddressOutputDto?> GetAccountShippingAddressAsync(CancellationToken cancellationToken)
    {
        if (_currentUserIdProvider.CurrentUserId is null)
        {
            return null;
        }
        
        var accountShippingAddress = await _shippingDbContext.ShippingAddress
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.Neighborhood)
            .Where(a => a.OwnerAccountId == _currentUserIdProvider.CurrentUserId.Value)
            .FirstAsync(cancellationToken);

        return accountShippingAddress.ToShippingAddressOutputDto();
    }

    public async Task<ShippingAddressOutputDto?> GetAnonymousShippingAddressAsync(CancellationToken cancellationToken)
    {
        var anonymousShippingAddress = await _shippingDbContext.ShippingAddress
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.Neighborhood)
            .Where(a => a.OwnerAccountId == _anonymousAccountProvider.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        return anonymousShippingAddress?.ToShippingAddressOutputDto();
    }

    private async Task<MoneyOutputDto> CalculateShippingFeeAsync(ShippingAddressForShippingFeeInputDto input, CancellationToken cancellationToken)
    {
        var shippingFee = await _shippingDbContext.ShippingFee
            .Where(x => x.CityId == input.CityId && x.DistrictId == input.DistrictId && x.NeighborhoodId == input.NeighborhoodId)
            .FirstOrDefaultAsync(cancellationToken);

        return (shippingFee?.Price ?? new Money(49.99m)).ToMoneyOutputDto();
    }

    public async Task<IReadOnlyList<CityOutputDto>> GetAllCitiesAsync(CancellationToken cancellationToken = default)
    {
        var cities = await _shippingDbContext.City
            .Select(c => c.ToCityOutputDto())
            .ToListAsync(cancellationToken);

        return cities.AsReadOnly();
    }

    public async Task<IReadOnlyList<DistrictOutputDto>> GetDistrictsByCityIdAsync(Guid? cityId, CancellationToken cancellationToken = default)
    {
        if (cityId is null)
        {
            return [];
        }

        var districts = await _shippingDbContext.District
            .Where(d => d.CityId == cityId)
            .Select(d => d.ToDistrictOutputDto())
            .ToListAsync(cancellationToken);

        return districts.AsReadOnly();
    }

    public async Task<IReadOnlyList<NeighborhoodOutputDto>> GetNeighborhoodsByDistrictIdAsync(Guid? districtId, CancellationToken cancellationToken = default)
    {
        if (districtId is null)
        {
            return [];
        }

        var neighborhoods = await _shippingDbContext.Neighborhood
            .Where(n => n.DistrictId == districtId)
            .Select(n => n.ToNeighborhoodOutputDto())
            .ToListAsync(cancellationToken);

        return neighborhoods.AsReadOnly();
    }

    public async Task SaveOrUpdateAnonymousShippingAddressAsync(ShippingAddressInputDto shippingAddressInput, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(shippingAddressInput.CityId, message: "İl boş olamaz!");
        Guard.Against.Null(shippingAddressInput.DistrictId, message: "İlçe boş olamaz!");
        Guard.Against.Null(shippingAddressInput.NeighborhoodId, message: "Mahalle boş olamaz!");
        Guard.Against.Null(shippingAddressInput.Street, message: "Sokak/Cadde boş olamaz!");
        Guard.Against.Null(shippingAddressInput.BuildingNo, message: "Bina no boş olamaz!");

        var anonymousShippingAddress = await _shippingDbContext.ShippingAddress
            .Where(a => a.OwnerAccountId == _anonymousAccountProvider.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (anonymousShippingAddress is null)
        {
            anonymousShippingAddress = new ShippingAddress(
                GuidGenerator.CreateSimpleGuid(),
                _anonymousAccountProvider.AccountId,
                shippingAddressInput.CityId!.Value,
                shippingAddressInput.DistrictId!.Value,
                shippingAddressInput.NeighborhoodId!.Value,
                shippingAddressInput.Street!,
                shippingAddressInput.Postcode,
                shippingAddressInput.BuildingNo!,
                shippingAddressInput.Floor,
                shippingAddressInput.ApartmentNo,
                shippingAddressInput.AddressLine);

            await _shippingDbContext.ShippingAddress.AddAsync(anonymousShippingAddress, cancellationToken);
        }
        else
        {
            anonymousShippingAddress.CityId = shippingAddressInput.CityId!.Value;
            anonymousShippingAddress.DistrictId = shippingAddressInput.DistrictId!.Value;
            anonymousShippingAddress.NeighborhoodId = shippingAddressInput.NeighborhoodId!.Value;
            anonymousShippingAddress.Street = shippingAddressInput.Street!;
            anonymousShippingAddress.Postcode = shippingAddressInput.Postcode;
            anonymousShippingAddress.BuildingNo = shippingAddressInput.BuildingNo!;
            anonymousShippingAddress.Floor = shippingAddressInput.Floor;
            anonymousShippingAddress.ApartmentNo = shippingAddressInput.ApartmentNo;
            anonymousShippingAddress.AddressLine = shippingAddressInput.AddressLine;
        }

        await _shippingDbContext.SaveChangesAsync(cancellationToken);
    }
}
