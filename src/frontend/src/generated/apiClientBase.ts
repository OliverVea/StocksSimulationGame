export interface IApiConfiguration {
  baseUrl: string;
  bearerToken: string | undefined;
}

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
