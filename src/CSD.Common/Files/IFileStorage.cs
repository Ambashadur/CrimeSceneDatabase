using System.IO;
using System.Threading.Tasks;

namespace CSD.Common.Files;

public interface IFileStorage
{
    Task CreateAsync(Stream content, ContentType contentType, string name);

    Task<FileStream> GetContentAsync(ContentType contentType, string name);
}
