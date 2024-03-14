import AuthService from './services/authService';
import TokenService from './services/tokenService';

document.addEventListener('DOMContentLoaded', async () => {
    const login = document.getElementById('loginLink');
    const logout = document.getElementById('logoutLink');

    const showAuthenticated = document.getElementsByClassName('showAuthenticated');
    const showUnauthenticated = document.getElementsByClassName('showUnauthenticated');

    const updateUI = async () => {
        const isAuthenticated = await AuthService.isAuthenticated();
        console.log('isAuthenticated', isAuthenticated);

        if (isAuthenticated) {
            for (let i = 0; i < showAuthenticated.length; i++) {
                showAuthenticated[i].classList.remove('hidden');
            }
            for (let i = 0; i < showUnauthenticated.length; i++) {
                showUnauthenticated[i].classList.add('hidden');
            }
        } else {
            for (let i = 0; i < showAuthenticated.length; i++) {
                showAuthenticated[i].classList.add('hidden');
            }
            for (let i = 0; i < showUnauthenticated.length; i++) {
                showUnauthenticated[i].classList.remove('hidden');
            }
        }
    };

    login?.addEventListener('click', async () => {
        await AuthService.login();
        await updateUI();
        await TokenService.getToken();
    });

    logout?.addEventListener('click', async () => {
        await AuthService.logout();
        await updateUI();
        await TokenService.clearToken();
    });

    await updateUI();
});

