using CSD.Common.Attributes;
using CSD.Story;
using CSD.Story.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSD.WebApp.Controllers;

[ApiController]
[Authorization]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IStory<CreateCommentStoryContext> _createCommentStory;

    public CommentsController(IStory<CreateCommentStoryContext> createCommentStory) {
        _createCommentStory = createCommentStory;
    }

    [HttpPost]
    public Task CreateComment(
        [FromForm] long sceneId,
        IFormFile audioFile,
        IFormFile photoFile) {
        return _createCommentStory.ExecuteAsync(new CreateCommentStoryContext {
            SceneId = sceneId,
            AudioFileName = audioFile.FileName,
            AudioContentStream = audioFile.OpenReadStream(),
            PhotoFileName = photoFile.FileName,
            PhotoContentStream = photoFile.OpenReadStream()
        });
    }
}
