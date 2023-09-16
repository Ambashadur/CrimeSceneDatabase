using System.Threading.Tasks;

namespace CSD.Story;

public interface IStory<TResult, TContext>
{
    Task<TResult> ExecuteAsync(TContext context);
}

public interface IStory<TContext>
{
    Task ExecuteAsync(TContext context);
}

public interface IStory
{
    Task ExecuteAsync();
}