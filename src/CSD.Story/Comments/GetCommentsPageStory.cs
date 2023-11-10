using System;
using System.Linq;
using System.Threading.Tasks;
using CSD.Common.DataAccess;
using CSD.Common.Files;
using CSD.Common.Helpers;
using CSD.Domain.Dto.Comments;

namespace CSD.Story.Comments;

public class GetCommentsPageStory : IStory<PageResult<CommentDto>, GetCommentsPageContext>
{
    private readonly CsdContext _context;
    private readonly IFileStorage _fileStorage;

    public GetCommentsPageStory(CsdContext context, IFileStorage fileStorage) {
        _context = context;
        _fileStorage = fileStorage;
    }

    public Task<PageResult<CommentDto>> ExecuteAsync(GetCommentsPageContext context) {
        if (context.Page < 1) throw new ArgumentException("Page must be greater or equal than 1");

        var comments = (from comment in _context.Comments
                        join scene in _context.Scenes
                            on comment.SceneId equals scene.Id into scenes
                        from scene in scenes.DefaultIfEmpty()
                        where comment.UserId == context.UserId
                        select new CommentDto {
                            Id = comment.Id,
                            Text = _fileStorage.GetTextFromFile(comment.TextFileName),
                            Scene = scene.Name,
                            AudioLink = string.Format("api/comments/{0}/audio?hash={1}", comment.Id, HashHelper.ComputeHash(comment.AudioFileName)),
                            PhotoLink = string.Format("api/comments/{0}/photo?hash={1}", comment.Id, HashHelper.ComputeHash(comment.PhotoFileName))
                        }).Skip((context.Page - 1) * context.Count).Take(context.Count);

        return Task.FromResult(new PageResult<CommentDto> {
            Page = context.Page,
            Count = comments.Count(),
            TotalCount = _context.Comments.Where(comment => comment.UserId == context.UserId).Count(),
            Data = comments
        });
    }
}
