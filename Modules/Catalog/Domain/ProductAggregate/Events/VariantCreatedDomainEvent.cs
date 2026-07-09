using Haskap.DddBase.Domain.Events;
using Modules.Catalog.Application.Backoffice.Dtos.Product;

namespace Modules.Catalog.Domain.ProductAggregate.Events;

public record VariantCreatedDomainEvent(
    Guid NewVariantId,
    List<SaveVariantPhotoFileInputDto> SaveVariantPhotoFileInputDtos,
    string ContentRootPath) : DomainEvent;
