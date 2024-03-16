using Core.Models.Ids;

namespace Core.Services;

public interface IUserIdService
{
    public UserId? UserId { get; }
    public void Initialize(string? userId);
    public void Reset();
}