namespace Shared.Result.Results;

public class Error
{
    private Error(string code, string message, Exception? exception = null, string? propertyName = null)
    {
        Code = code;
        Message = message;
        Exception = exception;
        PropertyName = propertyName;
    }
    
    public string Code { get; }
    
    public string? Message { get; }
    
    public string? PropertyName { get; }
    
    public Exception? Exception { get; }

    public static Error Create(string code, string? propertyName = null, string message = "Error") 
        => new(code, message, null, propertyName);
    
    public static Error CreatePropertyValidationError(string code, string propertyName, string message = "Validation error.") 
        => new(code, message, null, propertyName);
    
    public static Error CreateNotFoundError(string code, string message = "Not found error.")
        => new(code, message);
    
    public static Error CreateUnknownError(string code, string message = "Unknown error.", Exception? exception = null)
        => new(code, message, exception);
}
