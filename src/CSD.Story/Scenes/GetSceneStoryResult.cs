using System.IO;

namespace CSD.Story.Scenes;

public class GetSceneStoryResult
{
    public FileStream Content { get; set; }

    public string ContentType { get; set; }

    public string Name { get; set; }
}
