using System.IO;

namespace CSD.Story.Scenes;

public class CreateSceneStoryContext {
    public string Name { get; set; } = string.Empty;

    public string Filename { get; set; } = string.Empty;

    public Stream? Content { get; set; }
}
