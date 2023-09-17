using System.IO;

namespace CSD.Story.Comments;

public class CreateCommentStoryContext
{
    public long UserId { get; set; }

    public long SceneId { get; set; }

    public string AudioFileName { get; set; }

    public Stream AudioContentStream { get; set; }

    public string PhotoFileName { get; set; }

    public Stream PhotoContentStream { get; set; }
}
