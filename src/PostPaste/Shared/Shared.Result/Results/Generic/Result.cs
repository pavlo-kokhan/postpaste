using Shared.Result.Results.Generic.Abstract;

namespace Shared.Result.Results.Generic;

public class Result<TData> : Result, IResult<TData>
{
    private Result(TData data) => Data = data;
    
    private Result(ResultStatus status, IReadOnlyCollection<Error> errors) : base(status, errors) { }
    
    private Result(ResultStatus status, Error error) : base(status, error) { }

    public TData Data { get; } = default!;

    public static Result<TData> Success(TData data) 
        => new(data);

    public new static Result<TData> Failure(ResultStatus status, Error error) 
        => new(status, error);

    public new static Result<TData> Failure(ResultStatus status, IReadOnlyCollection<Error> errors) 
        => new(status, errors);
    
    public new static Result<TData> ValidationFailure(Error error) 
        => new(ResultStatus.ValidationError, error);
    
    public new static Result<TData> ValidationFailure(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.ValidationError, errors);
    
    public new static Result<TData> NotFound(Error error) 
        => new(ResultStatus.NotFound, error);
    
    public new static Result<TData> NotFound(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.NotFound, errors);
    
    public new static Result<TData> InternalFailure(Error error) 
        => new(ResultStatus.InternalError, error);
    
    public new static Result<TData> InternalFailure(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.InternalError, errors);

    public static implicit operator Result<TData>(TData data)
        => new(data);
}