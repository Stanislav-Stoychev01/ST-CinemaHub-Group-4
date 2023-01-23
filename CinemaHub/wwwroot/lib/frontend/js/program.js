function ListViewModel() {
    this.service = new BaseService(window.location.origin + '/api/movie-screenings');
    this.base = new PaginationListBase(this.service);
    this.base.fetchLatest();

    this.base.items.subscribe(change => {
        $("#loader").css("display", "none")
    })
}
var vm = new ListViewModel();
ko.applyBindings(vm);
