using CSD.Common.DataAccess;
using CSD.Common.Helpers;
using CSD.Domain.Dto.Scenes;
using System.Linq;
using System.Threading.Tasks;

namespace CSD.Story.Scenes;

public class GetPageScenesStory : IStory<PageResult<SceneDto>, GetPageContext>
{
    private readonly CsdContext _dbContext;

    public GetPageScenesStory(CsdContext dbContext) {
        _dbContext = dbContext;
    }

    public Task<PageResult<SceneDto>> ExecuteAsync(GetPageContext context) {
        var scenes = _dbContext.Scenes
            .Skip((context.Page - 1) * context.Count)
            .Take(context.Count)
            .Select(scene =>  new SceneDto() {
                Id = scene.Id,
                Name = scene.Name,
                Link = string.Format("api/scenes/{0}/preview?hash={1}", scene.Id, HashHelper.ComputeHash(scene.FileName))
            });

        var pageResult = new PageResult<SceneDto>() {
            Page = context.Page,
            Count = scenes.Count(),
            Data = scenes
        };

        return Task.FromResult(pageResult);
    }
}
