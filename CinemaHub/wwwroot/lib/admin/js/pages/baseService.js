class BaseService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl
    }

    fetchLatest = (params) => {
        let promise = new Promise((resolve, reject) => {
            fetch(this.baseUrl + "?" + new URLSearchParams(params),
                    {
                        method: 'GET', // or 'PUT'
                    })
                .then((response) => {
                    if (response.ok) {
                        return response.json();
                    }
                    return handleError(response);
                })
                .then((data) => {
                    resolve(data);
                })
                .catch((error) => {
                    showAlert(error);
                    reject(error);
                });
        })
        return promise;
    }

    edit = (id, data) => {
        let promise = new Promise((resolve, reject) => {
            fetch(this.baseUrl + '/' + id,
                    {
                        method: 'PUT', // or 'PUT'
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(data),
                    })
                .then((response) => {
                    if (response.ok) {
                        return response.text();
                    }
                    return handleError(response);
                })
                .then((data) => {
                    if (data) {
                        resolve(JSON.parse(data));
                    } else {
                        resolve();
                    }
                })
                .catch((error) => {
                    showAlert(error);
                    reject(error);
                });
        });
        return promise;
    }

    create = (data) => {
        let promise = new Promise((resolve, reject) => {
            fetch(this.baseUrl,
                    {
                        method: 'POST', // or 'PUT'
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(data),
                    })
                .then((response) => {
                    if (response.ok) {
                        return response.text();
                    }
                    return handleError(response);
                })
                .then((data) => {
                    if (data) {
                        resolve(JSON.parse(data));
                    } else {
                        resolve();
                    }
                })
                .catch((error) => {
                    showAlert(error);
                    reject(error);
                });
        });
        return promise;
    }

    delete = (id) => {
        let promise = new Promise((resolve, reject) => {
            fetch(this.baseUrl + '/' + id,
                    {
                        method: 'DELETE', // or 'PUT'
                        headers: {
                            'Content-Type': 'application/json',
                        },
                    })
                .then((response) => {
                    if (response.ok) {
                        return response.text();
                    }
                    return handleError(response);
                })
                .then((data) => {
                    if (data) {
                        resolve(JSON.parse(data));
                    } else {
                        resolve();
                    }
                })
                .catch((error) => {
                    showAlert(error);
                    reject(error);
                });
        });
        return promise
    }
}