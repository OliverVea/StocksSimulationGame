import * as ApiClient from "../generated/apiClient";
import TokenService from "./tokenService";

class ApiClientService {
  static async GetApiClient(): Promise<ApiClient.ApiClient> {
    const token = await TokenService.getToken();

    const apiClient = new ApiClient.ApiClient({
      baseUrl: "https://localhost:7066",
      bearerToken: token,
    });

    return apiClient;
  }

  static async getStocks(): Promise<ApiClient.ListStocksResponseModel> {
    const apiClient = await this.GetApiClient();
    return await apiClient.apiStocksGet();
  }

  static async addStocks(
    request: ApiClient.AddStocksRequestModel
  ): Promise<ApiClient.AddStocksResponseModel> {
    const apiClient = await this.GetApiClient();
    return await apiClient.apiStocksPost(request);
  }

  static async deleteStocks(
    request: ApiClient.DeleteStocksRequestModel
  ): Promise<ApiClient.DeleteStocksResponseModel> {
    const apiClient = await this.GetApiClient();
    return await apiClient.apiStocksDelete(request);
  }

  static async updateStocks(
    request: ApiClient.UpdateStocksRequestModel
  ): Promise<void> {
    const apiClient = await this.GetApiClient();
    return await apiClient.apiStocksPatch(request);
  }

  static async getStockSummaries(): Promise<ApiClient.SummarizeStocksResponseModel> {
    const apiClient = await this.GetApiClient();
    return await apiClient.apiStocksSummaries();
  }
}

export default ApiClientService;
