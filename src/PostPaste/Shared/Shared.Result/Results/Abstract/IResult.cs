namespace Shared.Result.Results.Abstract;

public interface IResult
{
    public bool IsSuccess { get; }
    
    public bool IsFailure { get; }
    
    public ResultStatus Status { get; }
    
    public IDictionary<string, Error> Errors { get; }
}