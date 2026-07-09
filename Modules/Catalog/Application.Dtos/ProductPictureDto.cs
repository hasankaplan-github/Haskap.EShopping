using Haskap.DddBase.Application.Dtos.Common;

namespace Modules.Catalog.Application.Dtos;

public class ProductPictureDto
{
    public FileOutputDto PhotoFile { get; set; }
    public bool IsPrimary { get; set; }
}
