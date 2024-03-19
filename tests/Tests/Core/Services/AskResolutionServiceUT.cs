using AutoFixture;
using Core.Models.Asks;
using Core.Models.Portfolio;
using Core.Models.User;
using Core.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using OneOf.Types;
using Tests.DataBuilders;

namespace Tests.Core.Services;

public class AskResolutionServiceUT : BaseUT<IAskResolutionService, AskResolutionService>
{
    private IUserPortfolioService _userPortfolioService = null!;
    private IAskService _askService = null!;
    private IUserService _userService = null!;
    
    [SetUp]
    public void SetupSubstitutes()
    {
        SutBuilder.AddSubstitute<ILogger<AskResolutionService>>();
        _userPortfolioService = SutBuilder.AddSubstitute<IUserPortfolioService>();
        _askService = SutBuilder.AddSubstitute<IAskService>();
        _userService = SutBuilder.AddSubstitute<IUserService>();
    }
    
    [Test]
    public async Task ResolveAsksForStockAsync_WithAsks_DeletesAsksAfter()
    {
        // Arrange
        var stockPrice = DataBuilder.GetStockPriceResponse().Create();

        SetupMocks();
        
        // Act
        await Sut.ResolveAsksForStockAsync(stockPrice, CancellationToken);

        // Assert
        await _userPortfolioService.Received(1).RemoveFromPortfolioAsync(Arg.Any<RemoveFromPortfolioRequest>(), CancellationToken);
        await _userService.Received(1).ModifyUserBalanceAsync(Arg.Any<ModifyUserBalanceRequest>(), CancellationToken);
        await _askService.Received(1).DeleteAsksAsync(Arg.Any<DeleteAsksRequest>(), CancellationToken);
    }
    
    [Test]
    public async Task ResolveAsksForStockAsync_WithNoAsks_DoesNotDeleteAsks()
    {
        // Arrange
        var stockPrice = DataBuilder.GetStockPriceResponse().Create();

        SetupMocks(askCount: 0);
        
        // Act
        await Sut.ResolveAsksForStockAsync(stockPrice, CancellationToken);

        // Assert
        await _userPortfolioService.DidNotReceive().RemoveFromPortfolioAsync(Arg.Any<RemoveFromPortfolioRequest>(), CancellationToken);
        await _userService.DidNotReceive().ModifyUserBalanceAsync(Arg.Any<ModifyUserBalanceRequest>(), CancellationToken);
        await _askService.DidNotReceive().DeleteAsksAsync(Arg.Any<DeleteAsksRequest>(), CancellationToken);
    }
    
    [Test]
    public async Task ResolveAsksForStockAsync_WithRemoveFromPortfolioError_DoesNotModifyUserBalance()
    {
        // Arrange
        var stockPrice = DataBuilder.GetStockPriceResponse().Create();

        SetupMocks(removeFromPortfolioSuccess: false);
        
        // Act
        await Sut.ResolveAsksForStockAsync(stockPrice, CancellationToken);

        // Assert
        await _userPortfolioService.Received(1).RemoveFromPortfolioAsync(Arg.Any<RemoveFromPortfolioRequest>(), CancellationToken);
        await _userService.DidNotReceive().ModifyUserBalanceAsync(Arg.Any<ModifyUserBalanceRequest>(), CancellationToken);
        await _askService.DidNotReceive().DeleteAsksAsync(Arg.Any<DeleteAsksRequest>(), CancellationToken);
    }
    
    [Test]
    public async Task ResolveAsksForStockAsync_WithModifyUserBalanceError_DoesNotDeleteAsks()
    {
        // Arrange
        var stockPrice = DataBuilder.GetStockPriceResponse().Create();

        SetupMocks(modifyUserBalanceSuccess: false);
        
        // Act
        Assert.ThrowsAsync<Exception>(async () => 
            await Sut.ResolveAsksForStockAsync(stockPrice, CancellationToken));

        // Assert
        await _userPortfolioService.Received(1).RemoveFromPortfolioAsync(Arg.Any<RemoveFromPortfolioRequest>(), CancellationToken);
        await _userService.Received(1).ModifyUserBalanceAsync(Arg.Any<ModifyUserBalanceRequest>(), CancellationToken);
        await _askService.DidNotReceive().DeleteAsksAsync(Arg.Any<DeleteAsksRequest>(), CancellationToken);
    }
    
    private void SetupMocks(int askCount = 1, bool removeFromPortfolioSuccess = true, bool modifyUserBalanceSuccess = true)
    {
        MockGetAsksResponse(askCount);
        MockRemoveFromPortfolioResponse(removeFromPortfolioSuccess);
        MockModifyUserBalanceResponse(modifyUserBalanceSuccess);
    }
    
    private void MockRemoveFromPortfolioResponse(bool success)
    {
        _userPortfolioService.RemoveFromPortfolioAsync(Arg.Any<RemoveFromPortfolioRequest>(), CancellationToken)
            .Returns(success ? new Success() : new Error());
    }
    
    private void MockModifyUserBalanceResponse(bool success)
    {
        _userService.ModifyUserBalanceAsync(Arg.Any<ModifyUserBalanceRequest>(), CancellationToken)
            .Returns(success ? new Success() : new Error());
    }
    
    private void MockGetAsksResponse(int askCount)
    {
        var asks = DataBuilder.GetAsksResponse(askCount: askCount).Create();

        _askService.GetAsksAsync(Arg.Any<GetAsksRequest>(), CancellationToken)
            .Returns(asks);
    }
}