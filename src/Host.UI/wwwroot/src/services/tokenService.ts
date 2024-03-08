import AuthService from "./authService";

class TokenService {
    static async getToken() : Promise<string> {
        return await AuthService.getTokenSilently()
    }
}

export default TokenService;