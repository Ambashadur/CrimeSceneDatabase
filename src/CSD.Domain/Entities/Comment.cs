using System.ComponentModel.DataAnnotations.Schema;

namespace CSD.Domain.Entities;

[Table("comment")]
public class Comment : EntityBase
{
    [Column("scene_id")]
    public long SceneId { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("path_to_audio")]
    public string PathToAudio { get; set; } = string.Empty;

    [Column("path_to_photo")]
    public string PathToPhoto { get; set;} = string.Empty;

    [Column("path_to_text")]
    public string PathToText { get; set;} = string.Empty;
}
