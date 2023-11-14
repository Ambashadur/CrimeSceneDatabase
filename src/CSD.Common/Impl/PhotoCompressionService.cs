using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CSD.Common.Impl;

public class PhotoCompressionService : IPhotoComressionService
{
    public async Task<MemoryStream> CompressAsync(Stream stream, int aspectRatio) {
        using var image = await Image.LoadAsync(stream);
        var copyStream = new MemoryStream();

        image.Mutate(x => x.Resize(image.Width / aspectRatio, image.Height / aspectRatio));
        await image.SaveAsync(copyStream, image.Metadata.DecodedImageFormat);

        copyStream.Seek(0, SeekOrigin.Begin);

        return copyStream;

    }
}
