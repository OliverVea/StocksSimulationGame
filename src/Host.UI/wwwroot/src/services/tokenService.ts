import AuthService from "./authService";

class TokenService {
  static token: string | undefined;

  static async getToken(): Promise<string | undefined> {
    const isAuthenticated = await AuthService.isAuthenticated();
    if (!isAuthenticated) return undefined;

    const hasValidToken = !(await this.isTokenExpiredAsync());
    if (hasValidToken) return this.token;

    const tokenFromStorage = await this.getTokenFromStorageAsync();
    if (tokenFromStorage) {
      this.token = tokenFromStorage;
      return tokenFromStorage;
    }

    const token = await AuthService.getTokenSilently();
    this.setToken(token);
    return token;
  }

  static async isTokenExpiredAsync(): Promise<boolean> {
    if (!this.token) return true;

    const token = this.token.split(".")[1];
    const decodedToken = JSON.parse(atob(token));
    const expiration = decodedToken.exp;

    return Date.now() >= expiration * 1000;
  }

  static setToken(token: string) {
    localStorage.setItem("token", token);
    this.token = token;
  }

  static async getTokenFromStorageAsync(): Promise<string | undefined> {
    const token = localStorage.getItem("token");
    if (token) return token;
    return undefined;
  }

  static clearToken() {
    localStorage.removeItem("token");
    this.token = undefined;
  }
}

export default TokenService;
