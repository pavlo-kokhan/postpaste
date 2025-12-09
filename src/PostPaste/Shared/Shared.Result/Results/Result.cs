using Shared.Result.Results.Abstract;

namespace Shared.Result.Results;

public class Result : IResult
{
    protected Result()
    {
        IsSuccess = true;
        Status = ResultStatus.Ok;
        Errors = [];
    }

    protected Result(ResultStatus status, IReadOnlyCollection<Error> errors)
    {
        IsSuccess = false;
        Status = status;
        Errors = errors;
    }

    protected Result(ResultStatus status, Error error) 
        : this(status, [error])
    { }
    
    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public ResultStatus Status { get; }
    
    public IReadOnlyCollection<Error> Errors { get; }
    
    public static Result Success() 
        => new();
    
    public static Result Failure(ResultStatus status, Error error) 
        => new(status, error);
    
    public static Result Failure(ResultStatus status, IReadOnlyCollection<Error> errors) 
        => new(status, errors);
    
    public static Result ValidationFailure(Error error) 
        => new(ResultStatus.ValidationError, error);
    
    public static Result ValidationFailure(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.ValidationError, errors);
    
    public static Result NotFound(Error error) 
        => new(ResultStatus.NotFound, error);
    
    public static Result NotFound(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.NotFound, errors);

    public static Result InternalFailure(Error error) 
        => new(ResultStatus.InternalError, error);
    
    public static Result InternalFailure(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.InternalError, errors);
}