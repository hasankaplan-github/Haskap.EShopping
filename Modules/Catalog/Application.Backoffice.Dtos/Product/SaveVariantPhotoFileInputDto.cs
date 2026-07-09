namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class SaveVariantPhotoFileInputDto
{
    public string OriginalName { get; set; }
    public string NewName { get; set; }
    public string? Extension { get; set; }
    public byte[] Content { get; set; }
}
