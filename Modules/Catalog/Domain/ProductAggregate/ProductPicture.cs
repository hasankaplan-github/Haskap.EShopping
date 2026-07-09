using Haskap.DddBase.Domain;

namespace Modules.Catalog.Domain.ProductAggregate;

public class ProductPicture : ValueObject
{
    public Guid Id { get; init; }
    public Haskap.DddBase.Domain.Common.File PhotoFile { get; private set; }
    public bool IsPrimary { get; private set; }

    private ProductPicture()
    {}

    public ProductPicture(Haskap.DddBase.Domain.Common.File photoFile, bool isPrimary)
    {
        PhotoFile = photoFile;
        IsPrimary = isPrimary;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PhotoFile;
        yield return IsPrimary;
    }
}
