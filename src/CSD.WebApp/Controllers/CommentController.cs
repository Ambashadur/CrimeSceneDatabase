﻿using System.Threading.Tasks;
using CSD.Common.Attributes;
using CSD.Story;
using CSD.Story.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSD.WebApp.Controllers;

[ApiController]
[Authorization]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IStory<CreateCommentStoryContext> _createCommentStory;

    public CommentController(IStory<CreateCommentStoryContext> createCommentStory) {
        _createCommentStory = createCommentStory;
    }

    [HttpPost]
    public Task CreateComment(
        [FromForm] long userId,
        [FromForm] long sceneId,
        IFormFile audioFile,
        IFormFile photoFile) {
        return _createCommentStory.ExecuteAsync(new CreateCommentStoryContext() {
            UserId = userId,
            SceneId = sceneId,
            AudioFileName = audioFile.FileName,
            AudioContentStream = audioFile.OpenReadStream(),
            PhotoFileName = photoFile.FileName,
            PhotoContentStream = photoFile.OpenReadStream()
        });
    }
}