using System;
using System.Threading.Tasks;
using CSD.Common.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CSD.Story.Comments;

public class DeleteCommentStory : IStory<DeleteCommentStoryContext>
{
    private readonly CsdContext _context;

    public DeleteCommentStory(CsdContext context) {
        _context = context;
    }

    public async Task ExecuteAsync(DeleteCommentStoryContext context) {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == context.Id)
            ?? throw new ArgumentException($"Comment with id {context.Id} doesn't exist!");

        _context.Comments.Remove(comment);
        _context.SaveChanges();
    }
}
