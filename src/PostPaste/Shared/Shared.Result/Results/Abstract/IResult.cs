namespace Shared.Result.Results.Abstract;

public interface IResult
{
    public bool IsSuccess { get; }
    
    public bool IsFailure { get; }
    
    public ResultStatus Status { get; }
    
    public IReadOnlyCollection<Error> Errors { get; }
}