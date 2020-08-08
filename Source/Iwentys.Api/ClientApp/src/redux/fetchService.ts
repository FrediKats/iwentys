export const BASE_URL = 'http://iwentys.azurewebsites.net/api/';

export function get(url: string, queryParams: Record<string,any>) {
    const params = {
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        ...queryParams
    };

    // @ts-ignore
    return fetch(BASE_URL + url, params).then(response => {
        if (response.status >= 200 && response.status < 300) {
            return response.json();
        } else {
            return Promise.reject(response.statusText);
        }
    });
}

export function post(url: string, data: Object) {
    const params = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify(data)
    };

    return fetch(BASE_URL + url, params);
}

export function put(url: string, data: Object) {
    const params = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify(data)
    };

    return fetch(BASE_URL + url, params);
}
