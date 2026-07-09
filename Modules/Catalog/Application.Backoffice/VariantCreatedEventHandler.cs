using Haskap.DddBase.Domain.Events;
using Microsoft.Extensions.Options;
using Modules.Catalog.Domain.ProductAggregate.Events;
using Modules.Catalog.Domain.Shared.Consts;

namespace Modules.Catering.Application.Caterings;
public class VariantCreatedEventHandler : IEventHandler<VariantCreatedDomainEvent>
{
    private readonly VariantPhotoSettings _variantPhotoSettings;

    public VariantCreatedEventHandler(IOptions<VariantPhotoSettings> variantPhotoSettingsOptions)
    {
        _variantPhotoSettings = variantPhotoSettingsOptions.Value;
    }

    public async Task HandleAsync(VariantCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        await CreatePhotosAsync(@event, cancellationToken);
    }

    private async Task CreatePhotosAsync(VariantCreatedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        if (!notification.SaveVariantPhotoFileInputDtos.Any())
        {
            return;
        }

        string[] paths = [notification.ContentRootPath, .. _variantPhotoSettings.FolderName.Split('\\'), notification.NewVariantId.ToString()];
        var fullFolderPath = Path.Combine(paths);
        Directory.CreateDirectory(fullFolderPath);

        var tasks = notification.SaveVariantPhotoFileInputDtos
            .AsParallel()
            .Select(async photo =>
            {
                var fullFileName = Path.Combine(fullFolderPath, $"{photo.NewName}{photo.Extension}");
                using (var fileStream = File.Create(fullFileName))
                {
                    await fileStream.WriteAsync(photo.Content, cancellationToken);
                }
            });

        await Task.WhenAll(tasks);
    }
}
