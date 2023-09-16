using CSD.Common.DataAccess;
using CSD.Domain.Dto.Scenes;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Story.Scenes;

public class CreateSceneStory : IStory<SceneDto, CreateSceneDto>
{
    private readonly CsdContext _context;
    private readonly ILogger<CreateSceneStory> _logger;

    public CreateSceneStory(CsdContext context, ILogger<CreateSceneStory> logger) {
        _context = context;
        _logger = logger;
    }

    public Task<SceneDto> ExecuteAsync(CreateSceneDto context) {
        if (_context.Scenes.Any(scene => scene.Name == context.Name)) {
            throw new ArgumentException("Scene with name {name} already exist!", context.Name);
        }


    }
}
