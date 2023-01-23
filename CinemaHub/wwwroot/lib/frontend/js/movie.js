function ListViewModel() {
    this.movie = ko.observable();

    this.service = new BaseService(window.location.origin + '/api/movies/' + movieId+ "/screenings");
    this.base = new PaginationListBase(this.service);
    this.base.fetchLatest();


    this.fetchMovie = () => {
        let promise = new Promise((resolve, reject) => {
            fetch("/api/movies/"+ movieId,
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
    this.fetchMovie().then(response => {
        this.movie(response);
        $("#loader").css("display", "none");
    });



}

var vm = new ListViewModel();
ko.applyBindings(vm);

function getDate(dateTime) {
    var date = new Date(dateTime+"Z");
    console.log(dateTime);
    return date.getDate() + "." + (date.getMonth()+1) + "." + date.getFullYear();

}
function getHours(dateTime) {
    var date = new Date(dateTime+"Z");
    var minutes = "";
    if (date.getMinutes() < 9) {
        minutes = "0" + date.getMinutes();
    } else {
        minutes = minutes;
    }
    return date.getHours() + ":" + minutes;
}