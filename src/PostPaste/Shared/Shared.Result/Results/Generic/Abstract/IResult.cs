using Shared.Result.Results.Abstract;

namespace Shared.Result.Results.Generic.Abstract;

public interface IResult<out TData> : IResult
{
    TData Data { get; }
}