using AutoFixture;
using Core.Repositories;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Persistence;

public sealed class UserStorageRepositoryIT : BaseIT<IUserStorageRepository>
{
    
    [Test]
    public async Task GetUserAsync_NoUserInDbWithUserId_ReturnsNull()
    {
       // Arrange
       var userId = DataBuilder.UserId().Create();
       
       // Act
       var actual = await Sut.GetUserAsync(userId, CancellationToken);

       // Assert
        Assert.That(actual, Is.Null);
    }
    
    [Test]
    public async Task AddUserAsync_WithUser_AddsUserToDb()
    {
        // Arrange
        var user = DataBuilder.UserInformation().Create();
        
        // Act
        await Sut.AddUserAsync(user, CancellationToken);
        var actual = await Sut.GetUserAsync(user.UserId, CancellationToken);

        // Assert
        Assert.That(actual, Is.EqualTo(user));
    }
    
    [Test]
    public async Task GetUserAsync_WithMultipleUsersInDb_ReturnsCorrectUser()
    {
        // Arrange
        var users = DataBuilder.UserInformation().CreateMany(100).ToArray();
        var user = users[13];
        
        // Act
        foreach (var u in users)
        {
            await Sut.AddUserAsync(u, CancellationToken);
        }
        var actual = await Sut.GetUserAsync(user.UserId, CancellationToken);

        // Assert
        Assert.That(actual, Is.EqualTo(user));
    }
}