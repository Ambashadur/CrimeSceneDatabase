using System.ComponentModel.DataAnnotations;
using System.IO;

namespace CSD.Domain.Dto.Scenes;

public class CreateSceneDto
{
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    public Stream? Content { get; set; }
}
