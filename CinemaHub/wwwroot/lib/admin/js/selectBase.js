class SelectBase {
    
    constructor(service, id, observable) {
        new Autocomplete(id, {
            onSearch: async ({ element }) => {
                // first get all the items and split with a comma
                const lastElement = element.value.split(",").pop().trim();
                // if the last item is 0 then we don't do a search
                if (lastElement.length === 0) return;

                var params = { searchText: lastElement };
                return await service.fetchLatest(params).then((res) => {
                    console.log(res);
                    var result = res.data;
                    return result;
                })
            },

            onResults: ({ matches }) =>
                matches.map((el) => `<li class='loupe'>${el.name}</li>`).join(""),

            onSubmit: ({ index, element, object, results }) => {
               
                console.log("index: ", index, "object: ", object, "results: ", results);
                
                observable(object.id);
                element.value = element.value.trim();
                
                element.focus();
            },

            onReset: (element) => {
                observable(null);
            },
        });
    }
}