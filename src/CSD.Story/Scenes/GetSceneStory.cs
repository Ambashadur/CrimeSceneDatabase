using System.IO;
using System.Threading.Tasks;
using CSD.Common.DataAccess;
using CSD.Common.Exceptions;
using CSD.Common.Files;
using CSD.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CSD.Story.Scenes;

public class GetSceneStory : IStory<MediaResult, GetSceneStoryContext>
{
    private readonly CsdContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public GetSceneStory(CsdContext dbContext, IFileStorage fileStorage) {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<MediaResult> ExecuteAsync(GetSceneStoryContext context) {
        var scene = await _dbContext.Scenes.FirstOrDefaultAsync(scene => scene.Id == context.Id)
            ?? throw new NotFoundException($"Scene with Id: {context.Id} not found!");

        return new MediaResult {
            Content = await _fileStorage.GetContentAsync(ContentType.Scene, scene.FileName),
            ContentType = "image/" + Path.GetExtension(scene.FileName),
            Name = scene.FileName
        };
    }
}
