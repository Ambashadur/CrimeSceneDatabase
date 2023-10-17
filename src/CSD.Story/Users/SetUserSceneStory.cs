using System.Threading.Tasks;
using CSD.Common.DataAccess;
using CSD.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CSD.Story.Users;

public class SetUserSceneStory : IStory<SetUserSceneStoryContext>
{
    private readonly CsdContext _dbContext;

    public SetUserSceneStory(CsdContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(SetUserSceneStoryContext context) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == context.UserId)
            ?? throw new NotFoundException($"User with Id: {context.UserId} was not found!");

        if (context.SceneId.HasValue) {
            var scene = await _dbContext.Scenes.FirstOrDefaultAsync(scene => scene.Id == context.SceneId)
                ?? throw new NotFoundException($"Scene with Id: {context.SceneId} was not found!");

            user.SceneId = scene.Id;
        } else {
            user.SceneId = null;
        }

        _dbContext.SaveChanges();
    }
}
