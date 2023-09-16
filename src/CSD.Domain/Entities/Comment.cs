using System.ComponentModel.DataAnnotations.Schema;

namespace CSD.Domain.Entities;

[Table("comments")]
public class Comment : EntityBase
{
    [Column("scene_id")]
    public long SceneId { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("audio_filename")]
    public string AudioFileName { get; set; } = string.Empty;

    [Column("photo_filename")]
    public string PhotoFileName { get; set;} = string.Empty;

    [Column("text_filename")]
    public string TextFileName { get; set;} = string.Empty;
}
