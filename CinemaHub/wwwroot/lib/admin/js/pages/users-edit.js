function UsersEditViewModel(obj) {
    this.id = obj?.id,
        this.form = {
            firstName: ko.observable(obj?.firstName).extend({ required: true }),
            lastName: ko.observable(obj?.lastName).extend({ required: true }),
            phoneNumber: ko.observable(obj?.phoneNumber).extend({ required: true, maxLength: 15 }),
            email: obj?.email,
            role: ko.observable(obj?.role),
            status: obj?.status,
            createdOn: new Date(obj?.createdOn).toLocaleString()
        };
    this.form.errors = ko.validation.group(this.form);


    saveBtn = () => {
        if (this.form.errors().length != 0) {
            this.form.errors.showAllMessages();
            return;
        }

        data = parseForm(this.form);
        fetch(window.location.origin + '/api/admin/users/' + this.id, {
            method: 'PUT', // or 'PUT'
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        })
            .then((response) => response.json())
            .then((data) => {
                vm.updatedEntity(data);
                showAlert("Successfully updated user!")
            })
            .catch((error) => {
                console.error('Error:', error);
            });

    }
}