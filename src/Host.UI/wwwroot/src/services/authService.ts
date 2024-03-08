import { createAuth0Client } from '@auth0/auth0-spa-js';
import { Auth0Client } from '@auth0/auth0-spa-js/dist/typings/Auth0Client';

class AuthService {
    private static instance: Auth0Client;

    private static async getInstance(): Promise<Auth0Client> {
        if (!this.instance) this.instance = await this.initializeInstance();
        return this.instance;
    }

    static async initializeInstance() : Promise<Auth0Client> {
        const instance = await createAuth0Client({
            domain: 'dev-upqv5rwnfhyvo18d.us.auth0.com',
            clientId: 'wlZcPA8IQN7dBssjev4oK4nvAtSYdtt9',
            authorizationParams: {
                redirect_uri: window.location.origin,
            }
        });

        return instance;
    }

    static async login(): Promise<void> {
        const client = await this.getInstance();
        await client.loginWithPopup();
    }

    static async logout(): Promise<void> {
        const client = await this.getInstance();
        await client.logout();
    }

    static async isAuthenticated(): Promise<boolean> {
        const client = await this.getInstance();
        return await client.isAuthenticated();
    }

    static async getTokenSilently(): Promise<string> {
        const client = await this.getInstance();
        return await client.getTokenSilently();
    }
}

export default AuthService;
