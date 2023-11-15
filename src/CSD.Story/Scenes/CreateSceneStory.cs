using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Common.Files;
using CSD.Common.Helpers;
using CSD.Contracts.Scenes;
using Microsoft.Extensions.Logging;

namespace CSD.Story.Scenes;

public class CreateSceneStory : IStory<SceneDto, CreateSceneStoryContext>
{
    private readonly CsdContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IPhotoComressionService _photoComressionService;
    private readonly ILogger<CreateSceneStory> _logger;

    public CreateSceneStory(
        CsdContext dbContext,
        IFileStorage fileStorage,
        ILogger<CreateSceneStory> logger,
        IPhotoComressionService photoComressionService) {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _logger = logger;
        _photoComressionService = photoComressionService;
    }

    public async Task<SceneDto> ExecuteAsync(CreateSceneStoryContext context) {
        if (_dbContext.Scenes.Any(scene => scene.Name == context.Name)) {
            throw new ArgumentException("Scene with name {name} already exist!", context.Name);
        }

        await _fileStorage.CreateAsync(context.Content, ContentType.Scene, context.Filename);
        context.Content.Seek(0, SeekOrigin.Begin);
        await _fileStorage.CreateAsync(
            await _photoComressionService.CompressAsync(context.Content, 4),
            ContentType.CompressedPhoto,
            context.Filename);

        var scene = await _dbContext.Scenes.AddAsync(new() {
            CreateDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Name = context.Name,
            FileName = context.Filename
        });

        _dbContext.SaveChanges();

        return new SceneDto() {
            Id = scene.Entity.Id,
            Name = context.Name,
            Link = string.Format("api/scenes/{0}/preview?hash={1}", scene.Entity.Id, HashHelper.ComputeHash(context.Name))
        };
    }
}
