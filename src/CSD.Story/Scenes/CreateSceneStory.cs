using CSD.Common.DataAccess;
using CSD.Common.Files;
using CSD.Domain.Dto.Scenes;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Story.Scenes;

public class CreateSceneStory : IStory<SceneDto, CreateSceneStoryContext>
{
    private readonly CsdContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<CreateSceneStory> _logger;

    public CreateSceneStory(
        CsdContext dbContext,
        IFileStorage fileStorage,
        ILogger<CreateSceneStory> logger) {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<SceneDto> ExecuteAsync(CreateSceneStoryContext context) {
        if (_dbContext.Scenes.Any(scene => scene.Name == context.Name)) {
            throw new ArgumentException("Scene with name {name} already exist!", context.Name);
        }

        await _fileStorage.CreateAsync(context.Content, ContentType.Scene, context.Filename);

        var scene = await _dbContext.Scenes.AddAsync(new() {
            CreateDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Name = context.Name,
            FileName = context.Filename
        });

        _dbContext.SaveChanges();

        return new SceneDto() {
            Id = scene.Entity.Id,
            Name = context.Name
        };
    }
}
