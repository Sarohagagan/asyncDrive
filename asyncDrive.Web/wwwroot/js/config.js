$(async function () {
    debugger
    await GetToken();
    await GetConfig();
    async function GetConfig() {
        try {
            const response = await fetch('https://localhost:7122/api/config');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            // Use the configuration values as needed
            const config = await response.json();
            window.getConfig = function () {
                return config;
            };
            window.getApiBaseUrl = function () {
                return config.apiBaseUrl;
            };
        } catch (error) {
            console.error('Failed to fetch configuration:', error);
        }
    }
    async function GetToken() {
        try {
            const jwtToken = localStorage.getItem('jwtToken');

            if (!jwtToken || jwtToken.trim() === '') {
                const response = await fetch('https://localhost:7122/api/Session/data', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    throw new Error(`Network response was not ok: ${response.statusText}`);
                }
                const data = await response.json();
                localStorage.setItem('jwtToken', data.sessionData);
            }
        }
        catch (error) {
            console.error('Fetch error:', error);
        }
    }

})