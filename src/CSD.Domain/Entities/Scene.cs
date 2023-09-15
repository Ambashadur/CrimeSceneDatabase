using System.ComponentModel.DataAnnotations.Schema;

namespace CSD.Domain.Entities;

[Table("scenes")]
public class Scene : EntityBase
{
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("path")]
    public string Path { get; set; } = string.Empty;
}
