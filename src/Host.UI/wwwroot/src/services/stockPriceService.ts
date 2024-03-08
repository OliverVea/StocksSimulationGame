import ApiClientService from "./apiClientService";

class StockPriceService {
    static async getStockPriceHistory(): Promise<IStockPricesHistory> {
        const apiClient = await ApiClientService.GetApiClient();

        const stocks = await apiClient.apiStocksSummaries();

        const stockPrices = stocks.stocks.map(async (stock) => {
            const prices = await apiClient.apiPriceHistory(stock.stockId, undefined, undefined);

            return {
                stockId: stock.stockId,
                ticker: stock.ticker,
                from: prices.from,
                to: prices.to,
                prices: prices.prices.map(entry => {
                    return {
                        price: entry.price,
                        timestep: entry.step
                    };
                })
            };
        });

        return {
            stockPrices: await Promise.all(stockPrices)
        };
    }
}

export default StockPriceService;
