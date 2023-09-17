using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CSD.Story.Users;

public class SetUserSceneStoryContext
{
    [JsonIgnore]
    public long UserId { get; set; }

    [Required]
    public long SceneId { get; set; }
}
