function EditViewModel(obj, service) {
    this.id = obj?.id;
    this.service = service;
    this.movie = obj?.movie;
    this.theater = obj?.theater;
    this.form = {
            movieId: ko.observable(obj?.movie.id).extend({ required: true }),
            theaterId: ko.observable(obj?.theater.id).extend({ required: true }),
            dateTime: ko.observable(obj?.dateTime+"Z").extend({ required: true }),
            isPremiere: ko.observable(obj?.isPremiere),
            type: ko.observable(obj?.type).extend({ required: true }),
            createdOn: new Date(obj?.createdOn).toLocaleString()
        };
    this.form.errors = ko.validation.group(this.form);
   
    this.load = () => {
        this.movieService = new BaseService(window.location.origin + '/api/admin/movies');
        new SelectBase(this.movieService, "movie-select", this.form.movieId);
        $("#movie-select").val(this.movie?.name);

        this.theaterService = new BaseService(window.location.origin + '/api/admin/movie-theaters');
        new SelectBase(this.theaterService, "movie-theater-select", this.form.theaterId);
        $("#movie-theater-select").val(this.theater?.name);

        var picker = $("#date-select").flatpickr({
            enableTime: true,
            onChange: (selectedDates, dateStr, instance) => {
                if (selectedDates.length > 0) {
                    this.form.dateTime(selectedDates[0].toISOString());
                } else {
                    this.fromDateTime(null);
                    this.toDateTime(null);
                }
            },
        });

        picker.setDate(this.form.dateTime());
    }
    saveBtn = () => {
        
        data = parseForm(this.form);
        if (this.id) {
            editSubmit(data);
        } else {
            createSubmit(data);
        }


    }
    editSubmit = (data) => {
        this.service.edit(this.id, data).then((response) => {
            vm.updatedEntity(response);
            showAlert("Successfully updated movie screening!");
        });

    }
    createSubmit = (data) => {
        this.service.create(data).then((response) => {
            vm.updatedEntity(response);
            showAlert("Successfully added movie screening!");
        });;
    }
}

// array initialization
