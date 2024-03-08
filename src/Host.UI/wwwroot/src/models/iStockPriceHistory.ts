interface IStockPricesHistory {
    stockPrices: IStockPriceHistory[];
}

interface IStockPriceHistory {
    stockId: string;
    ticker: string;
    from: number;
    to: number;
    prices: IStockPriceHistoryEntry[];
}

interface IStockPriceHistoryEntry {
    price: number;
    timestep: number;
}