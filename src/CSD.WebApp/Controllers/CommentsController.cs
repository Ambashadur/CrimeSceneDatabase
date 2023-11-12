using CSD.Common.Attributes;
using CSD.Domain.Dto.Comments;
using CSD.Domain.Enums;
using CSD.Story;
using CSD.Story.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IStory<CreateCommentStoryContext> _createCommentStory;
    private readonly IStory<MediaResult, GetAudioFromCommentStoryContext> _getAudioStory;
    private readonly IStory<MediaResult, GetPhotoFromCommentStoryContext> _getPhotoStory;
    private readonly IStory<PageResult<CommentDto>, GetCommentsPageContext> _getCommentsPageStory;

    public CommentsController(
        IStory<CreateCommentStoryContext> createCommentStory,
        IStory<MediaResult, GetAudioFromCommentStoryContext> getAudioStory,
        IStory<PageResult<CommentDto>, GetCommentsPageContext> getCommentsPageStory,
        IStory<MediaResult, GetPhotoFromCommentStoryContext> getPhotoStory) {
        _createCommentStory = createCommentStory;
        _getAudioStory = getAudioStory;
        _getCommentsPageStory = getCommentsPageStory;
        _getPhotoStory = getPhotoStory;
    }

    [HttpPost]
    [Authorization]
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

    [HttpGet("page")]
    [Authorization(Role = UserRole.Admin)]
    public Task<PageResult<CommentDto>> GetCommentPage([FromQuery] GetCommentsPageContext context) {
        return _getCommentsPageStory.ExecuteAsync(context);
    }

    [HttpGet("{id}/audio")]
    public async Task<IActionResult> GetAudio([FromRoute] long id, [FromQuery] string hash) {
        var result = await _getAudioStory.ExecuteAsync(new() { Id = id, Hash = hash });

        return File(result.Content, result.ContentType, result.Name);
    }

    [HttpGet("{id}/photo")]
    public async Task<IActionResult> GetPhoto([FromRoute] long id, [FromQuery] string hash) {
        var result = await _getPhotoStory.ExecuteAsync(new() { Id = id, Hash = hash });
        return File(result.Content, result.ContentType, result.Name);
    }
}
