using System;
using System.Threading.Tasks;
using CSD.Common;
using CSD.Common.DataAccess;
using CSD.Common.Exceptions;
using CSD.Common.Files;
using CSD.Common.VoiceRecognition;
using CSD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSD.Story.Comments;

public class CreateCommentStory : IStory<CreateCommentStoryContext>
{
    private readonly CsdContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IVoiceRecognitionService _voiceRecognitionService;
    private readonly IUserContext _userContext;

    public CreateCommentStory(
        CsdContext dbContext,
        IFileStorage fileStorage,
        IVoiceRecognitionService voiceRecognitionService,
        IUserContext userContext) {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _voiceRecognitionService = voiceRecognitionService;
        _userContext = userContext;
    }

    public async Task ExecuteAsync(CreateCommentStoryContext context) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == _userContext.CurrentUser.Id)
            ?? throw new NotFoundException($"User with Id: {_userContext.CurrentUser.Id} was not found!");

        var scene = await _dbContext.Scenes.FirstOrDefaultAsync(scene => scene.Id == context.SceneId)
            ?? throw new NotFoundException($"Scene with Id: {context.SceneId} was not found!");

        await _fileStorage.CreateAsync(context.PhotoContentStream, ContentType.Photo, context.PhotoFileName);
        await _fileStorage.CreateAsync(context.AudioContentStream, ContentType.Audio, context.AudioFileName);
        var text = await _voiceRecognitionService.GetTextFromAudioAsync(_fileStorage.GetPath(ContentType.Audio, context.AudioFileName));
        var textFileName = $"comment_audio_recognition_{user.FirstName}_{user.LastName}_{scene.Name}_{DateTime.UtcNow.ToString("dd_MM_yyyy_hh_mm_ss")}.txt";
        await _fileStorage.CreateTextFileAsync(text, textFileName);

        await _dbContext.Comments.AddAsync(new Comment() {
            CreateDate = DateTimeOffset.UtcNow,
            UpdateDate= DateTimeOffset.UtcNow,
            SceneId = scene.Id,
            UserId = user.Id,
            AudioFileName = context.AudioFileName,
            PhotoFileName = context.PhotoFileName,
            TextFileName = textFileName
        });

        _dbContext.SaveChanges();
    }
}
