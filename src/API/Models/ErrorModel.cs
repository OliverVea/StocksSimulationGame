namespace API.Models;

/// <summary>
/// Contains information about an error.
/// </summary>
public class ErrorModel
{
    /// <summary>
    /// The error message.
    /// </summary>
    public string Message { get; }
    
    internal ErrorModel(string message)
    {
        Message = message;
    }
}