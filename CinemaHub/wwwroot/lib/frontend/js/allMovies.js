function ListViewModel() {
    this.movies = ko.observableArray();

    this.fetchLatest = () => {
        let promise = new Promise((resolve, reject) => {
            fetch("/api/movies",
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

    this.fetchLatest().then(response => {
        this.movies(response);
        $("#loader").css("display", "none")
    });
}

var vm = new ListViewModel();
ko.applyBindings(vm);