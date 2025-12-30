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
    
    public new static Result<TData> BadRequest(Error error) 
        => new(ResultStatus.BadRequest, error);
    
    public new static Result<TData> BadRequest(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.BadRequest, errors);
    
    public new static Result<TData> Unauthorized(Error error) 
        => new(ResultStatus.Unauthorized, error);
    
    public new static Result<TData> Unauthorized(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.Unauthorized, errors);
    
    public new static Result<TData> Forbidden(Error error) 
        => new(ResultStatus.Forbidden, error);
    
    public new static Result<TData> Forbidden(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.Forbidden, errors);
    
    public new static Result<TData> NotFound(Error error) 
        => new(ResultStatus.NotFound, error);
    
    public new static Result<TData> NotFound(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.NotFound, errors);
    
    public new static Result<TData> Conflict(Error error) 
        => new(ResultStatus.Conflict, error);
    
    public new static Result<TData> Conflict(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.Conflict, errors);
    
    public new static Result<TData> InternalFailure(Error error) 
        => new(ResultStatus.InternalError, error);
    
    public new static Result<TData> InternalFailure(IReadOnlyCollection<Error> errors) 
        => new(ResultStatus.InternalError, errors);

    public static implicit operator Result<TData>(TData data)
        => new(data);
}