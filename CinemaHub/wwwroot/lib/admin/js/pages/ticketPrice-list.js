function ListViewModel() {
    this.service = new BaseService(window.location.origin + '/api/admin/ticket-prices');
    this.base = new PaginationListBase(this.service);
    var params = { searchText: "" };
    var myModal = new coreui.Modal(document.getElementById('editModal'))
    this.base.fetchLatest(params);

    this.editClick = (item) => {
        this.editModal(new EditViewModel(item, this.service));
        myModal.show();
    }

    this.updatedEntity = () => {
        myModal.hide();
        this.base.fetchLatest(params);
    }

    this.editModal = ko.observable();
}
var vm = new ListViewModel();
ko.applyBindings(vm);