using AutoFixture.Dsl;
using Core.Models.Ids;
using Core.Models.Portfolio;
using Tests.Extensions;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<GetUserPortfolioRequest> GetUserPortfolioRequest()
    {
        return Fixture.Build<GetUserPortfolioRequest>()
            .WithEmpty(x => x.StockIds);
    }
    
    public static IPostprocessComposer<GetUserPortfolioRequest> GetUserPortfolioRequest(UserId userId, IReadOnlyCollection<StockId>? stockIds = null)
    {
        var request = GetUserPortfolioRequest();
            
        request = request .With(x => x.UserId, userId);
        if (stockIds is not null) request = request.With(x => x.StockIds, stockIds);

        return request;
    }

    public static IPostprocessComposer<SetPortfolioRequest> SetPortfolioRequest()
    {
        return Fixture.Build<SetPortfolioRequest>()
            .WithEmpty(x => x.Stocks);
    }
    
    public static IPostprocessComposer<SetPortfolioRequest> SetPortfolioRequest(UserId userId, IReadOnlyCollection<SetPortfolioStock>? stocks = null)
    {
        var request = SetPortfolioRequest();
            
        request = request .With(x => x.UserId, userId);
        if (stocks is not null) request = request.With(x => x.Stocks, stocks);

        return request;
    }

    public static IPostprocessComposer<SetPortfolioStock> SetPortfolioStock()
    {
        return Fixture.Build<SetPortfolioStock>()
            .With(x => x.Quantity, () => GetRandomInt(1, int.MaxValue));
    }

    public static IPostprocessComposer<SetPortfolioStock> SetPortfolioStock(StockId stockId, int? quantity = null)
    {
        var stock = SetPortfolioStock();
            
        stock = stock .With(x => x.StockId, stockId);
        if (quantity is not null) stock = stock.With(x => x.Quantity, quantity.Value);

        return stock;
    }
}