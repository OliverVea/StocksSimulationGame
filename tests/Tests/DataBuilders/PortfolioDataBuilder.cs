using AutoFixture.Dsl;
using Core.Models.Ids;
using Core.Models.Portfolio;

namespace Tests.DataBuilders;

public sealed partial class DataBuilder
{
    public IPostprocessComposer<GetUserPortfolioRequest> GetUserPortfolioRequest()
    {
        return Fixture.Build<GetUserPortfolioRequest>()
            .With(x => x.StockIds, Array.Empty<StockId>());
    }
    
    public IPostprocessComposer<GetUserPortfolioRequest> GetUserPortfolioRequest(UserId userId, IReadOnlyCollection<StockId>? stockIds = null)
    {
        var request = GetUserPortfolioRequest();
            
        request = request .With(x => x.UserId, userId);
        if (stockIds is not null) request = request.With(x => x.StockIds, stockIds);

        return request;
    }

    public IPostprocessComposer<SetPortfolioRequest> SetPortfolioRequest()
    {
        return Fixture.Build<SetPortfolioRequest>()
            .With(x => x.Stocks, Array.Empty<SetPortfolioStock>());
    }
    
    public IPostprocessComposer<SetPortfolioRequest> SetPortfolioRequest(UserId userId, IReadOnlyCollection<SetPortfolioStock>? stocks = null)
    {
        var request = SetPortfolioRequest();
            
        request = request .With(x => x.UserId, userId);
        if (stocks is not null) request = request.With(x => x.Stocks, stocks);

        return request;
    }

    public IPostprocessComposer<SetPortfolioStock> SetPortfolioStock()
    {
        return Fixture.Build<SetPortfolioStock>()
            .With(x => x.Quantity, () => GetRandomInt(1, int.MaxValue));
    }

    public IPostprocessComposer<SetPortfolioStock> SetPortfolioStock(StockId stockId, int? quantity = null)
    {
        var stock = SetPortfolioStock();
            
        stock = stock .With(x => x.StockId, stockId);
        if (quantity is not null) stock = stock.With(x => x.Quantity, quantity.Value);

        return stock;
    }
}