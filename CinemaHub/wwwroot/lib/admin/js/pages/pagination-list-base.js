class PaginationListBase {
    constructor(service, count = 10) {
        this.service = service;
        this.count = count;
    }

    startAt = 0;
    count = 10;
    totalCount = ko.observable();
    items = ko.observableArray();
    pages = ko.observableArray();
    fetchLatest = async (params) => {
        if (!params) params = {};
        params.startAt = this.startAt;
        params.count = this.count;
        this.service.fetchLatest(params)
            .then((data) => {
                console.log(data);
                this.items(data.data);
                this.totalCount(data.totalCount);
                this.calcPages();
            });

    }

    calcPages() {
        this.pages.removeAll();
        var index = 1
        for (var i = 0; i < this.totalCount(); i += this.count) {
            this.pages.push({ startAt: i, number: index, isActive: this.startAt == i })
            index++;
        }

    }

    nextPage = () => {
        if (this.totalCount() > this.startAt + this.count) {
            this.startAt += this.count;
            this.fetchLatest();
        }
    }
    prevPage = () => {
        if (this.startAt - this.count >= 0) {
            this.startAt -= this.count;
            this.fetchLatest();
        }
    }
    setPage = (page) => {
        this.startAt = page.startAt;
        this.fetchLatest();
    }

}