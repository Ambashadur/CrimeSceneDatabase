using System.Text.Json.Serialization;

namespace CSD.Story.Users;

public class SetUserSceneStoryContext
{
    [JsonIgnore]
    public long UserId { get; set; }

    public long? SceneId { get; set; }
}
