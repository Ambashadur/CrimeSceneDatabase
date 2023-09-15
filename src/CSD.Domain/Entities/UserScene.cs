using System.ComponentModel.DataAnnotations.Schema;

namespace CSD.Domain.Entities;

[Table("user_scenes")]
public class UserScene : EntityBase
{
    [Column("user_id")]
    public long UserId { get; set; }

    [Column("scene_id")]
    public long SceneId { get; set; }
}
