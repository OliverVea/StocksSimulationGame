import * as ApiClient from "../generated/apiClient";
import ApiClientService from "./apiClientService";

class StocksService {
  static async getStocks(): Promise<ApiClient.ListStocksResponseModel> {
    const apiClient = await ApiClientService.GetApiClient();
    return await apiClient.apiStocksGet();
  }

  static async addStocks(
    request: ApiClient.AddStocksRequestModel
  ): Promise<ApiClient.AddStocksResponseModel> {
    const apiClient = await ApiClientService.GetApiClient();
    return await apiClient.apiStocksPost(request);
  }

  static async deleteStocks(
    request: ApiClient.DeleteStocksRequestModel
  ): Promise<ApiClient.DeleteStocksResponseModel> {
    const apiClient = await ApiClientService.GetApiClient();
    return await apiClient.apiStocksDelete(request);
  }

  static async updateStocks(
    request: ApiClient.UpdateStocksRequestModel
  ): Promise<void> {
    const apiClient = await ApiClientService.GetApiClient();
    return await apiClient.apiStocksPatch(request);
  }

  static async getStockSummaries(): Promise<ApiClient.SummarizeStocksResponseModel> {
    const apiClient = await ApiClientService.GetApiClient();
    return await apiClient.apiStocksSummaries();
  }
}

export default StocksService;
