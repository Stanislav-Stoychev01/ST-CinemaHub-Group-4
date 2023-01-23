function EditViewModel(obj, service) {
        this.id = obj?.id;
        this.service = service;

        this.form = {
            daysOfTheWeek: ko.observable(obj?.daysOfTheWeek).extend({ required: true }),
            screeningType: ko.observable(obj?.screeningType).extend({ required: true }),
            price: ko.observable(obj?.price).extend({ required: true }),
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
            showAlert("Successfully updated ticket price!")
        });
       
    }

    createSubmit = () => {
        data = parseForm(this.form);
        this.service.create(data).then((response) => {
            vm.updatedEntity(response);
            showAlert("Successfully ticket price!")
        });;
    }
}