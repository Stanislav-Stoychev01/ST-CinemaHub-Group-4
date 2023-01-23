function EditViewModel(obj, service) {
        this.id = obj?.id;
        this.service = service;

        this.form = {
            name: ko.observable(obj?.name).extend({ required: true }),
            createdOn: new Date(obj?.createdOn).toLocaleString()
        };
    this.form.errors = ko.validation.group(this.form);


    saveBtn = () => {
        if (this.form.errors().length != 0) {
            this.form.errors.showAllMessages();
            return;
        }

        data = parseForm(this.form);
        if (this.id) {
            editSubmit();
        } else {
            createSubmit();
        }
    }

    editSubmit = () => {
        data = parseForm(this.form);
        this.service.edit(this.id, data).then((response) => {
            vm.updatedEntity(response);
            showAlert("Successfully updated genre!")
        });
       
    }

    createSubmit = () => {
        data = parseForm(this.form);
        this.service.create(data).then((response) => {
            vm.updatedEntity(response);
        showAlert("Successfully added genre!")
        });;
    }
}