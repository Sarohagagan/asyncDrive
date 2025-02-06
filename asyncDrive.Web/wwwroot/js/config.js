﻿//$(async function () {
// await GetToken();
// await GetConfig();

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
        //const jwtToken = localStorage.getItem('accessToken');

        //if (!jwtToken || jwtToken.trim() === '' || jwtToken == undefined) {
            const response = await fetch('https://localhost:7122/api/Session/data', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        if (!response.ok) {
                //throw new Error(`Network response was not ok: ${response.statusText}`);
            } else {
                const data = await response.json();
                localStorage.setItem('accessToken', data.accessToken);
                localStorage.setItem('refreshToken', data.refreshToken);
            }
        //}
    }
    catch (error) {
        console.error('Fetch error:', error);
    }
}

//async function refreshAccessToken() {
//    const response = await fetch(`${apiUrl}/refresh`, {
//        method: "POST",
//        headers: {
//            "Content-Type": "application/json"
//        },
//        body: JSON.stringify({ accessToken, refreshToken })
//    });
//    const data = await response.json();
//    accessToken = data.accessToken;
//    refreshToken = data.refreshToken;
//    console.log("Token refreshed successfully");
//}
async function refreshToken() {
    try {
        let accessToken = localStorage.getItem('accessToken');
        let refreshToken = localStorage.getItem('refreshToken');
        const response = await fetch(window.getApiBaseUrl() + 'Auth/Refresh', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ "AccessToken": accessToken, "RefreshToken": refreshToken })
        });

        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }

        const data = await response.json();
        localStorage.setItem('accessToken', data.accessToken); // Assuming the new token is in data.token
        localStorage.setItem('refreshToken', data.refreshToken);
        loadDataTable();
        console.log('Token refreshed successfully');
    } catch (error) {
        console.error('Error refreshing token:', error);
        // Optionally handle token refresh failure, e.g., redirect to login
    }
}
//})