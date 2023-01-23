function UserListViewModel() {
    this.service = new BaseService(window.location.origin + '/api/admin/users');
    this.base = new PaginationListBase(this.service);
    var params = { email: "", searchText: "" };
    var myModal = new coreui.Modal(document.getElementById('userEditModal'))
    this.base.fetchLatest(params);

    this.email = ko.observable();
    this.email.subscribe(() => {
        params.email = this.email();
        this.base.fetchLatest(params);
    })
    this.name = ko.observable();
    this.name.subscribe(() => {
        params.searchText = this.name();
        this.base.fetchLatest(params);
    })

    this.editClick = (item) => {
        this.editModal(new UsersEditViewModel(item));
        myModal.show();
    }
    this.deleteClick = (item) => {
        fetch(window.location.origin + '/api/admin/users/' + item.id, {
            method: 'DELETE', // or 'PUT'
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(() => {
                vm.updatedEntity();
                showAlert("Successfully deleted user!")
            })
            .catch((error) => {
                console.error('Error:', error);
            });
    }

    this.updatedEntity  = () => {
        myModal.hide();
        this.base.fetchLatest(params);
    }

    this.editModal = ko.observable();
}
var vm = new UserListViewModel();
ko.applyBindings(vm);