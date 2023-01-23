function ListViewModel() {
    this.service = new BaseService(window.location.origin + '/api/admin/movie-screenings');
    this.base = new PaginationListBase(this.service);
    var params = { searchText: "", sortFields:["+dateTime"] };
    var myModal = new coreui.Modal(document.getElementById('editModal'))
    this.base.fetchLatest(params);

    this.movieId = ko.observable();
    this.movieId.subscribe(() => {
        params.movieId = this.movieId();
        this.base.fetchLatest(params);
    })
    this.fromDateTime = ko.observable();
    this.fromDateTime.subscribe(() => {
        params.fromDateTime = this.fromDateTime();
        this.base.fetchLatest(params);
    })
    this.toDateTime = ko.observable();
    this.toDateTime.subscribe(() => {
        params.toDateTime = this.toDateTime();
        this.base.fetchLatest(params);
    })
    this.theaterId = ko.observable();
    this.theaterId.subscribe(() => {
        params.theaterId = this.theaterId();
        this.base.fetchLatest(params);
    })
    this.isPremiere = ko.observable(false);
    this.isPremiere.subscribe(() => {
        params.isPremiere = this.isPremiere();
        this.base.fetchLatest(params);
    })

    this.movieService = new BaseService(window.location.origin + '/api/admin/movies');
    new SelectBase(this.movieService, "movie-search-text", this.movieId);

    this.theaterService = new BaseService(window.location.origin + '/api/admin/movie-theaters');
    new SelectBase(this.theaterService, "movie-theater-search-text", this.theaterId);

    $("#date-range-select").flatpickr({
        mode: "range",
        wrap: true,
        onChange: (selectedDates, dateStr, instance)  => {
           if (selectedDates.length > 1) {
               this.fromDateTime(selectedDates[0].toISOString());
               this.toDateTime(selectedDates[1].toISOString());
           } else {
               this.fromDateTime(null);
               this.toDateTime(null);
           }
        },
    })

    this.editClick = (item) => {
        var modal = new EditViewModel(item, this.service);
        this.editModal(modal);
        myModal.show();
        modal.load();
    }

    this.newItemClick = () => {
        var modal = new EditViewModel(null, this.service);
        this.editModal(modal);
        myModal.show();
        modal.load();
    }
    this.deleteClick = async (item) => {
        this.service.delete(item.id).then((res) => {
            vm.updatedEntity();
            showAlert("Successfully deleted movie screening!")
        });
    }

    this.updatedEntity  = () => {
        myModal.hide();
        this.base.fetchLatest(params);
    }

    this.editModal = ko.observable();
}
var vm = new ListViewModel();
ko.applyBindings(vm);