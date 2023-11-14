using System.IO;
using System.Threading.Tasks;

namespace CSD.Common;

public interface IPhotoComressionService
{
    Task<MemoryStream> CompressAsync(Stream stream, int aspectRatio);
}
