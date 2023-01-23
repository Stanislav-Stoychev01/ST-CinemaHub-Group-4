function ListViewModel() {
    this.service = new BaseService(window.location.origin + '/api/admin/genres');
    this.base = new PaginationListBase(this.service);
    var params = { searchText: "" };
    var myModal = new coreui.Modal(document.getElementById('editModal'))
    this.base.fetchLatest(params);

    this.name = ko.observable();
    this.name.subscribe(() => {
        params.searchText = this.name();
        this.base.fetchLatest(params);
    })

    this.editClick = (item) => {
        this.editModal(new EditViewModel(item, this.service));
        myModal.show();
    }

    this.newItemClick = () => {
        this.editModal(new EditViewModel(null, this.service));
        myModal.show();
    }
    this.deleteClick = async (item) => {
        this.service.delete(item.id).then((res) => {
            vm.updatedEntity();
            showAlert("Successfully deleted genre!")
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