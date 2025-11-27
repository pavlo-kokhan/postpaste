using Shared.Result.Results;

namespace Post.Api.Extensions;

public static class ResultToActionResultWrapperExtensions
{
    public static ActionResultWrapper ToActionResultWrapper(this Result result) 
        => new(result);
}