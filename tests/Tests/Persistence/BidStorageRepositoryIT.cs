using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Persistence;

public sealed class BidStorageRepositoryIT : BaseIT<IBidStorageRepository>
{
    private UserId _userId;
    
    [SetUp]
    public void SetupUser()
    {
        _userId = WithUserId();
    }
    
    [Test]
    public async Task GetBidsAsync_NoBidsInDatabase_EmptyResponseIsReturned()
    {
        // Arrange
        var request = DataBuilder.GetBidsRequest(_userId, Array.Empty<BidId>()).Create();

        // Act
        var actual = await Sut.GetBidsAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Bids, Is.Empty);
    }
    
    [Test]
    public async Task GetBidsAsync_WithBidInDatabase_BidIsReturned()
    {
        // Arrange
        var bid = DataBuilder.Bid().With(x => x.UserId, _userId).Create();
        await Sut.CreateBidAsync(bid, CancellationToken);
        
        var request = DataBuilder.GetBidsRequest(_userId, [bid.BidId]).Create();

        // Act
        var actual = await Sut.GetBidsAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Bids, Has.Length.EqualTo(1));
        var actualBid = actual.Bids.Single();
        Assert.That(actualBid, Is.EqualTo(bid));
    }

    [Test]
    public async Task GetBidsAsync_BidIdsNotSpecified_AllBidsForUserAreReturned()
    {
        // Arrange
        const int bidCount = 42;
        var bids = DataBuilder.Bid().With(x => x.UserId, _userId).CreateMany(bidCount).ToArray();
        foreach (var bid in bids) await Sut.CreateBidAsync(bid, CancellationToken);

        var request = DataBuilder.GetBidsRequest(_userId).Create();

        // Act
        var actual = await Sut.GetBidsAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Bids, Is.EquivalentTo(bids));
        Assert.That(actual.Bids, Has.Length.EqualTo(bidCount));
    }
    
    [Test]
    public async Task GetBidsAsync_WithStockIdSpecified_OnlyBidsForStockAreReturned()
    {
        // Arrange
        var bids = DataBuilder.Bid().With(x => x.UserId, _userId).CreateMany(2).ToArray();
        var bid = bids.First();
        foreach (var a in bids) await Sut.CreateBidAsync(a, CancellationToken);

        var request = DataBuilder.GetBidsRequest(_userId, stockId: bid.StockId).Create();

        // Act
        var actual = await Sut.GetBidsAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Bids, Has.Length.EqualTo(1));
        Assert.That(actual.Bids.Single(), Is.EqualTo(bid));
    }
    
    [Test]
    public async Task GetBidsAsync_WithMinPriceSpecified_OnlyBidsWithPriceAboveMinAreReturned()
    {
        // Arrange
        const int count = 100;
        const int top = 5;
        
        var bids = DataBuilder.Bid().With(x => x.UserId, _userId)
            .CreateMany(count)
            .OrderByDescending(x => x.PricePerUnit.Value)
            .ToArray();
        
        foreach (var bid in bids) await Sut.CreateBidAsync(bid, CancellationToken);
        
        var topBids = bids[..top];
        var minPrice = topBids.Last().PricePerUnit;

        var request = DataBuilder.GetBidsRequest(_userId, minPrice: minPrice).Create();

        // Act
        var actual = await Sut.GetBidsAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Bids, Is.EquivalentTo(topBids));
    }
    
    [Test]
    public async Task GetBidsAsync_WithBidIdsSpecified_OnlySpecifiedBidsAreReturned()
    {
        // Arrange
        var bids = DataBuilder.Bid().With(x => x.UserId, _userId).CreateMany(10).ToArray();
        foreach (var bid in bids) await Sut.CreateBidAsync(bid, CancellationToken);
        
        var request = DataBuilder.GetBidsRequest(_userId, bids[..5].Select(x => x.BidId).ToArray()).Create();

        // Act
        var actual = await Sut.GetBidsAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Bids, Is.EquivalentTo(bids[..5]));
    }
    
    [Test]
    public async Task DeleteBidsAsync_WithBidInDatabase_BidIsDeleted()
    {
        // Arrange
        var bid = DataBuilder.Bid().With(x => x.UserId, _userId).Create();
        await Sut.CreateBidAsync(bid, CancellationToken);
        
        var request = DataBuilder.DeleteBidsRequest([bid.BidId]).Create();

        // Act
        await Sut.DeleteBidsAsync(request, CancellationToken);

        // Assert
        var getBidsRequest = DataBuilder.GetBidsRequest(_userId, [bid.BidId]).Create();
        var actual = await Sut.GetBidsAsync(getBidsRequest, CancellationToken);
        Assert.That(actual.Bids, Is.Empty);
    }
}