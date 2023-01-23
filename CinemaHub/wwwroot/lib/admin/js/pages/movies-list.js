function ListViewModel() {
    this.service = new BaseService(window.location.origin + '/api/admin/movies');
    this.base = new PaginationListBase(this.service);
    var params = { searchText: "" };
    var myModal = new coreui.Modal(document.getElementById('editModal'))
    this.base.fetchLatest(params);

    this.name = ko.observable();
    this.name.subscribe(() => {
        params.searchText = this.name();
        this.base.fetchLatest(params);
    })
    this.genre = ko.observable();
    this.genre.subscribe(() => {
        params.genre = this.genre();
        this.base.fetchLatest(params);
    })
    this.includeInactive = ko.observable();
    this.includeInactive.subscribe(() => {
        params.includeInactive = this.includeInactive();
        this.base.fetchLatest(params);
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
            showAlert("Successfully deleted movie!")
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