function EditViewModel(obj, service) {
    var uploadPending = false;
    this.id = obj?.id;
    this.service = service;
    this.genres = obj?.genres ?? [];
    this.genreService = new BaseService("/api/admin/genres")
        this.form = {
            name: ko.observable(obj?.name).extend({ required: true }),
            actors: ko.observable(obj?.actors).extend({ required: true }),
            duration: ko.observable(obj?.duration).extend({ required: true }),
            trailerId: ko.observable(obj?.trailerId).extend({ required: true }),
            genresString: ko.observable(obj?.genres.join(', ')),
            isActive: ko.observable(obj?.isActive),
            description: ko.observable(obj?.description),
            imageUrl: ko.observable(obj?.imageUrl),
            createdOn: new Date(obj?.createdOn).toLocaleString()
        };
    this.form.errors = ko.validation.group(this.form);
   
    this.load = () => {
        $("#multiple-choices").val(this.form.genresString())
        this.autoselect = new MultipleSelectBase(this.genreService, "multiple-choices", this.genres);
       
    }
    saveBtn = () => {
        var genres = ($("#multiple-choices").val());
        var genresArray = genres.split(",");
        if (this.form.errors().length != 0) {
            this.form.errors.showAllMessages();
            return;
        }

        data = parseForm(this.form);
        data.genres = genresArray;
        if (this.id) {
            editSubmit(data);
        } else {
            createSubmit(data);
        }


    }
    appendImage = (file) => {
        if (file) {
            var reader = new FileReader();

            reader.onload =  (e) => {
                this.form.imageUrl(e.target.result);
            }

            reader.readAsDataURL(file);
            uploadPending = true;
        }
    }
    editSubmit = (data) => {
        this.service.edit(this.id, data).then((response) => {
            vm.updatedEntity(response);
            showAlert("Successfully updated movie theater!");
            this.uploadImage(response.id);
        });

    }

    createSubmit = (data) => {
        this.service.create(data).then((response) => {
            vm.updatedEntity(response);
            showAlert("Successfully added movie theater!")
            this.uploadImage(response.id);
        });;
    }
}

// array initialization
