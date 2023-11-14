using System;
using System.IO;
using System.Threading.Tasks;
using CSD.Common.DataAccess;
using CSD.Common.Exceptions;
using CSD.Common.Files;
using CSD.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CSD.Story.Scenes;

public class GetScenePreviewStory : IStory<MediaResult, GetScenePreviewStoryContext>
{
    private readonly CsdContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public GetScenePreviewStory(CsdContext dbContext, IFileStorage fileStorage) {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<MediaResult> ExecuteAsync(GetScenePreviewStoryContext context) {
        var scene = await _dbContext.Scenes.FirstOrDefaultAsync(scene => scene.Id == context.Id)
            ?? throw new NotFoundException($"Scene with Id: {context.Id} not found!");

        var hash = HashHelper.ComputeHash(scene.FileName);
        if (!string.Equals(hash, context.Hash)) throw new ArgumentException("Incorrect hash!");

        return new MediaResult {
            Content = await _fileStorage.GetContentAsync(ContentType.CompressedPhoto, scene.FileName),
            ContentType = "image/" + Path.GetExtension(scene.FileName),
            Name = scene.FileName
        };
    }
}
