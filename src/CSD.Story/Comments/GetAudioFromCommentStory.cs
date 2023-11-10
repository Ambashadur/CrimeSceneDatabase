using System;
using System.IO;
using System.Threading.Tasks;
using CSD.Common.DataAccess;
using CSD.Common.Exceptions;
using CSD.Common.Files;
using CSD.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CSD.Story.Comments;

public class GetAudioFromCommentStory : IStory<MediaResult, GetAudioFromCommentStoryContext>
{
    private readonly CsdContext _context;
    private readonly IFileStorage _fileStorage;

    public GetAudioFromCommentStory(CsdContext context, IFileStorage fileStorage) {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<MediaResult> ExecuteAsync(GetAudioFromCommentStoryContext context) {
        var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == context.Id)
            ?? throw new NotFoundException($"Comment with Id: {context.Id} not found!");

        var hash = HashHelper.ComputeHash(comment.AudioFileName);
        if (!string.Equals(hash, context.Hash)) throw new ArgumentException("Incorrect hash!");

        return new MediaResult {
            Content = await _fileStorage.GetContentAsync(ContentType.Audio, comment.AudioFileName),
            ContentType = "audio/" + Path.GetExtension(comment.AudioFileName),
            Name = comment.AudioFileName
        };
    }
}
