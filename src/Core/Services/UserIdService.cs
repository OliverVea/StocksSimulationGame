using Core.Models.Ids;

namespace Core.Services;

public sealed class UserIdService : IUserIdService
{
    private bool _initialized;
    private UserId? _userId;

    public UserId? UserId => GetUserId();
    
    public void Initialize(string? userId)
    {
        if (_initialized) throw new InvalidOperationException("UserIdService has already been initialized.");
        _initialized = true;
        
        if (userId != null) _userId = new UserId(userId);
    }
    
    private UserId? GetUserId()
    {
        if (!_initialized) throw new InvalidOperationException("UserIdService has not been initialized.");
        return _userId;
    }

    public void Reset()
    {
        _userId = null;
        _initialized = false;
    }
}