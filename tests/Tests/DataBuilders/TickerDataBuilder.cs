using AutoFixture.Dsl;
using Core.Models.Tickers;

namespace Tests.DataBuilders;

public partial class DataBuilder
{
    public IPostprocessComposer<AddTickersRequest> AddTickersRequest()
    {
        return Fixture.Build<AddTickersRequest>();
    }
    
    public IPostprocessComposer<AddTickersResponse> AddTickersResponse()
    {
        return Fixture.Build<AddTickersResponse>();
    }

    public IPostprocessComposer<ListTickersRequest> ListTickersRequest()
    {
        return Fixture.Build<ListTickersRequest>();
    }
    
    public IPostprocessComposer<ListTickersResponse> ListTickersResponse()
    {
        return Fixture.Build<ListTickersResponse>();
    }

    public IPostprocessComposer<DeleteTickersRequest> DeleteTickersRequest()
    {
        return Fixture.Build<DeleteTickersRequest>();
    }
    
    public IPostprocessComposer<DeleteTickersResponse> DeleteTickersResponse()
    {
        return Fixture.Build<DeleteTickersResponse>();
    }
}