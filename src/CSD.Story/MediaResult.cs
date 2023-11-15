using System.IO;

namespace CSD.Contracts;

public class MediaResult
{
    public FileStream? Content { get; set; }

    public string? ContentType { get; set; }

    public string? Name { get; set; }
}
