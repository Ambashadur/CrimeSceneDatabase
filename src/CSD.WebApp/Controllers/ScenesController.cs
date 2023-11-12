using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CSD.Common.Attributes;
using CSD.Domain.Dto.Scenes;
using CSD.Domain.Enums;
using CSD.Story;
using CSD.Story.Scenes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSD.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScenesController : ControllerBase
{
    private readonly IStory<SceneDto, CreateSceneStoryContext> _createSceneStory;
    private readonly IStory<PageResult<SceneDto>, GetPageContext> _getPageSceneStory;
    private readonly IStory<MediaResult, GetSceneStoryContext> _getSceneStory;

    public ScenesController(
        IStory<SceneDto, CreateSceneStoryContext> createSceneStory,
        IStory<PageResult<SceneDto>, GetPageContext> getPageSceneStory,
        IStory<MediaResult, GetSceneStoryContext> getSceneStory) {
        _createSceneStory = createSceneStory;
        _getPageSceneStory = getPageSceneStory;
        _getSceneStory = getSceneStory;
    }

    [HttpPost]
    [Authorization(Role = UserRole.Admin)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SceneDto))]
    public async Task<IActionResult> CreateScene([FromForm][Required][StringLength(128)] string name, IFormFile formFile) {
        if (formFile.Length == 0) return BadRequest();

        var sceneDto = default(SceneDto);

        using (var stream = formFile.OpenReadStream()) {
            sceneDto = await _createSceneStory.ExecuteAsync(new CreateSceneStoryContext() {
                Name = name,
                Filename = formFile.FileName,
                Content = stream
            });
        }

        return Created("api/scene", sceneDto);
    }

    [HttpGet("page")]
    [Authorization(Role = UserRole.Admin)]
    public Task<PageResult<SceneDto>> GetScenePage([FromQuery] GetPageContext context) {
        return _getPageSceneStory.ExecuteAsync(context);
    }

    [HttpGet("{id:long}")]
    [Authorization]
    public async Task<IActionResult> GetScene([FromRoute] long id) {
        var result = await _getSceneStory.ExecuteAsync(new GetSceneStoryContext() { Id = id });

        return File(result.Content, result.ContentType, result.Name);
    }
}
