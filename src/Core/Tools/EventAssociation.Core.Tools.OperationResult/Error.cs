namespace EventAssociation.Core.Tools.OperationResult;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public string? Type { get; }

    public Error(string code, string message, string? type = null)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public override string ToString() => Type is null ? $"[{Code}] {Message}" : $"[{Code} - {Type}] {Message}";  
}