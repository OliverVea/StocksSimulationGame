import * as ApiClient from "../generated/api-client";
import TokenService from "./tokenService";

class StocksService {
    private static apiClient: ApiClient.ApiClient = new ApiClient.ApiClient("https://localhost:7066");

    static async getStocks(): Promise<ApiClient.ListStocksResponseModel> {
        const token = TokenService.getToken();
        return await this.apiClient.apiStocksGet(token);
    }

    static async addStocks(request: ApiClient.AddStocksRequestModel): Promise<ApiClient.AddStocksResponseModel> {
        return await this.apiClient.apiStocksPost(request);
    }

    static async deleteStocks(request: ApiClient.DeleteStocksRequestModel): Promise<ApiClient.DeleteStocksResponseModel> {
        return await this.apiClient.apiStocksDelete(request);
    }

    static async updateStocks(request: ApiClient.UpdateStocksRequestModel): Promise<void> {
        return await this.apiClient.apiStocksPatch(request);
    }

    static async getStockSummaries(): Promise<ApiClient.SummarizeStocksResponseModel> {
        return await this.apiClient.apiStocksSummaries();
    }  
}

export default StocksService;