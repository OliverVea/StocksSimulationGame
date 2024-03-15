//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export class AuthorizedApiBase {
  config: IApiConfiguration;

  protected constructor(config: IApiConfiguration) {
    this.config = config;
  }

  protected getBaseUrl = (): string => {
    return this.config.baseUrl;
  };

  protected transformOptions = (options: RequestInit): Promise<RequestInit> => {
    if (this.config.bearerToken === undefined) {
      return Promise.resolve(options);
    }

    options.headers = {
      ...options.headers,
      Authorization: `Bearer ${this.config.bearerToken}`,
    };

    return Promise.resolve(options);
  };
}

export class ApiClient extends AuthorizedApiBase {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(configuration: IApiConfiguration, baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(configuration);
        this.http = http ? http : window as any;
        this.baseUrl = this.getBaseUrl("", baseUrl);
    }

    /**
     * @param dataPoints (optional) 
     * @param from (optional) 
     * @param to (optional) 
     * @return Success
     */
    apiPriceHistory(stockId: string, dataPoints: number | undefined, from: number | undefined, to: number | undefined): Promise<GetPriceHistoryResponseModel> {
        let url_ = this.baseUrl + "/api/price-history/{stockId}?";
        if (stockId === undefined || stockId === null)
            throw new Error("The parameter 'stockId' must be defined.");
        url_ = url_.replace("{stockId}", encodeURIComponent("" + stockId));
        if (dataPoints === null)
            throw new Error("The parameter 'dataPoints' cannot be null.");
        else if (dataPoints !== undefined)
            url_ += "dataPoints=" + encodeURIComponent("" + dataPoints) + "&";
        if (from === null)
            throw new Error("The parameter 'from' cannot be null.");
        else if (from !== undefined)
            url_ += "from=" + encodeURIComponent("" + from) + "&";
        if (to === null)
            throw new Error("The parameter 'to' cannot be null.");
        else if (to !== undefined)
            url_ += "to=" + encodeURIComponent("" + to) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiPriceHistory(_response);
        });
    }

    protected processApiPriceHistory(response: Response): Promise<GetPriceHistoryResponseModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetPriceHistoryResponseModel;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ErrorModel;
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<GetPriceHistoryResponseModel>(null as any);
    }

    /**
     * @return Success
     */
    apiSimulationInformation(): Promise<GetSimulationInformationResponseModel> {
        let url_ = this.baseUrl + "/api/simulation-information/{stockId}";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiSimulationInformation(_response);
        });
    }

    protected processApiSimulationInformation(response: Response): Promise<GetSimulationInformationResponseModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as GetSimulationInformationResponseModel;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ErrorModel;
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<GetSimulationInformationResponseModel>(null as any);
    }

    /**
     * @return Success
     */
    apiStocksGet(): Promise<ListStocksResponseModel> {
        let url_ = this.baseUrl + "/api/stocks";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiStocksGet(_response);
        });
    }

    protected processApiStocksGet(response: Response): Promise<ListStocksResponseModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ListStocksResponseModel;
            return result200;
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            return throwException("Unauthorized", status, _responseText, _headers);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            return throwException("Forbidden", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ListStocksResponseModel>(null as any);
    }

    /**
     * @return Success
     */
    apiStocksPost(body: AddStocksRequestModel): Promise<AddStocksResponseModel> {
        let url_ = this.baseUrl + "/api/stocks";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiStocksPost(_response);
        });
    }

    protected processApiStocksPost(response: Response): Promise<AddStocksResponseModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as AddStocksResponseModel;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ErrorModel;
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            return throwException("Unauthorized", status, _responseText, _headers);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            return throwException("Forbidden", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<AddStocksResponseModel>(null as any);
    }

    /**
     * @return Success
     */
    apiStocksDelete(body: DeleteStocksRequestModel): Promise<DeleteStocksResponseModel> {
        let url_ = this.baseUrl + "/api/stocks";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiStocksDelete(_response);
        });
    }

    protected processApiStocksDelete(response: Response): Promise<DeleteStocksResponseModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as DeleteStocksResponseModel;
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ErrorModel;
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            return throwException("Unauthorized", status, _responseText, _headers);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            return throwException("Forbidden", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<DeleteStocksResponseModel>(null as any);
    }

    /**
     * @return Success
     */
    apiStocksPatch(body: UpdateStocksRequestModel): Promise<void> {
        let url_ = this.baseUrl + "/api/stocks";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiStocksPatch(_response);
        });
    }

    protected processApiStocksPatch(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            result400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ErrorModel;
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 401) {
            return response.text().then((_responseText) => {
            return throwException("Unauthorized", status, _responseText, _headers);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            return throwException("Forbidden", status, _responseText, _headers);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    apiStocksSummaries(): Promise<SummarizeStocksResponseModel> {
        let url_ = this.baseUrl + "/api/stocks/summaries";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processApiStocksSummaries(_response);
        });
    }

    protected processApiStocksSummaries(response: Response): Promise<SummarizeStocksResponseModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as SummarizeStocksResponseModel;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<SummarizeStocksResponseModel>(null as any);
    }
}

/** Creates a single stock with the given parameters. */
export interface AddStockRequestModel {
    /** The ticker of the stock. Must be unique. */
    ticker: string;
    /** The starting price of the stock. */
    startingPrice: number;
    /** The volatility of the stock. */
    volatility: number;
    /** The drift of the stock. */
    drift: number;
    /** The color of the stock. */
    color: string;
}

/** Contains information about a stock that was added. */
export interface AddStockResponseModel {
    /** The unique identifier of the stock. */
    readonly stockId: string;
    /** The ticker symbol of the stock. */
    readonly ticker: string;
}

/** Creates new stocks with the given parameters. */
export interface AddStocksRequestModel {
    /** The stocks to create. */
    stocks: AddStockRequestModel[];
    /** If true, an error will be thrown if a stock with the same ticker already exists.
Otherwise, the duplicate stock will be ignored. */
    errorIfDuplicate: boolean;
}

/** Contains information about stocks that were added. */
export interface AddStocksResponseModel {
    /** The stocks that were added. */
    readonly stocks: AddStockResponseModel[];
}

/** Represents a request to delete stocks. */
export interface DeleteStocksRequestModel {
    /** The unique identifiers of the stocks to delete. */
    stockIds: string[];
    /** Whether to return an error if any of the stocks are missing. */
    errorIfMissing: boolean;
}

/** Contains information about the deleted stocks. */
export interface DeleteStocksResponseModel {
    /** The unique identifiers of the deleted stocks. */
    readonly ids: string[];
}

/** Contains information about an error. */
export interface ErrorModel {
    /** The error message. */
    readonly message: string;
}

/** Represents the response to a request to get the price history of a stock. */
export interface GetPriceHistoryResponseModel {
    /** The unique identifier of the stock. */
    readonly stockId: string;
    /** The first simulation step of the data.
Can be higher than the "From" parameter of the request. */
    readonly from: number;
    /** The last simulation step of the data.
Can be lower than the "To" parameter of the request. */
    readonly to: number;
    /** The price history of the stock. */
    readonly prices: PriceHistoryEntryModel[];
}

/** Represents the response model for the simulation information endpoint. */
export interface GetSimulationInformationResponseModel {
    /** The current step of the simulation. */
    readonly currentStep?: number;
    /** The number of seconds per step in the simulation. */
    readonly secondsPerStep?: number;
    /** The start time of the simulation. */
    readonly startTime?: string;
}

/** Contains information about a stock. */
export interface ListStockResponseModel {
    /** The unique identifier of the stock. */
    readonly stockId: string;
    /** The ticker symbol of the stock. */
    readonly ticker: string;
    /** The volatility of the stock. */
    readonly volatility: number;
    /** The drift of the stock. */
    readonly drift: number;
    /** The price of the stock. */
    readonly price: number;
    /** The color of the stock. */
    readonly color: string;
}

/** Contains a list of stocks. */
export interface ListStocksResponseModel {
    /** Contains information about a stock. */
    readonly stocks: ListStockResponseModel[];
}

/** Represents an entry in the price history of a stock. */
export interface PriceHistoryEntryModel {
    /** The simulation step of the price. */
    readonly step: number;
    /** The price of the stock at the simulation step. */
    readonly price: number;
}

/** Contains summaries of a stock. */
export interface SummarizeStockResponseModel {
    /** The unique identifier of the stock. */
    readonly stockId: string;
    /** The ticker symbol of the stock. */
    readonly ticker: string;
    /** The price of the stock. */
    readonly price: number;
    /** The color of the stock. */
    readonly color: string;
}

/** Contains a list of stocks. */
export interface SummarizeStocksResponseModel {
    /** Contains summaries of a stock. */
    readonly stocks: SummarizeStockResponseModel[];
}

/** The stock id to update */
export interface UpdateStockRequestModel {
    /** The stock id to update */
    stockId: string;
    /** The updated ticker. If null, the ticker will not be updated. */
    ticker?: string | undefined;
    /** The updated volatility. If null, the volatility will not be updated. */
    volatility?: number | undefined;
    /** The updated drift. If null, the drift will not be updated. */
    drift?: number | undefined;
}

/** Updates stocks. */
export interface UpdateStocksRequestModel {
    /** The stocks to update */
    stocks: UpdateStockRequestModel[];
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}

export interface IApiConfiguration {
  baseUrl: string;
  bearerToken: string | undefined;
}