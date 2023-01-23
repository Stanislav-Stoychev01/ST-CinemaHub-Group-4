function ListViewModel() {
    this.premiereMovie = ko.observable();
    this.latestMovies = ko.observableArray();

    this.fetchLatest = () => {
        let promise = new Promise((resolve, reject) => {
            fetch("/api/movies/latest",
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

    this.fetchPremiere = () => {
        let promise = new Promise((resolve, reject) => {
            fetch("/api/movies/premiere",
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

    this.fetchPremiere().then(response => {
        this.premiereMovie(response);
        $("#loader").css("display", "none")
    });
    this.fetchLatest().then(response => {
        this.latestMovies(response);
        $("#loader").css("display", "none")
    });

   

}

var vm = new ListViewModel();
ko.applyBindings(vm);