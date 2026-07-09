using Haskap.DddBase.Application.Dtos.Common;

namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class ProductPictureDto
{
    public FileOutputDto PhotoFile { get; set; }
    public bool IsPrimary { get; set; }
}
