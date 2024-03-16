using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// Contains information about an error.
/// </summary>
public sealed class ErrorModel
{
    /// <summary>
    /// The error message.
    /// </summary>
    [Required] public string Message { get; }
    
    internal ErrorModel(string message)
    {
        Message = message;
    }
}