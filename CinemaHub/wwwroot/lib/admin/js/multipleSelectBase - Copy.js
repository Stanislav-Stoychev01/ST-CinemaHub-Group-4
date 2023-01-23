class MultipleSelectBase {
    
    constructor(service, id, data) {
        let secondArray = data;
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

            onOpened: ({ element, results }) => {
                // type - two values 'results' and 'showItems',
                // 'resutls' first rendering of the results
                // 'showItems' only showing the results when clicking on the input field
                // resultList all results rendered containing ul and li
                // input - root input

                // get the data from the input field and divide by the
                // decimal point, then remove the empty last element
                const currentValue = element.value
                    .split(", ")
                    .splice(0, element.value.length - 1);

                // leave in the array only those elements that are in the input field
                secondArray = secondArray.filter((el) => currentValue.includes(el));

                // check if the table 'multipleArr' contains selected elements from
                // the input field, if so we add the 'selected' class to the 'li' element,
                // if not, remove the 'selected' class from the li element
                [].slice.call(results.children).map((item) => {
                    item.classList[secondArray.includes(item.textContent) ? "add" : "remove"](
                        "selected"
                    );
                });
            },

            onSubmit: ({ index, element, object, results }) => {
                if (secondArray.includes(element.value)) {
                    return;
                }

                console.log("index: ", index, "object: ", object, "results: ", results);

                // each click on the li element adds data to the array
                secondArray.push(element.value.trim());

                // check if the table includes selected items from
                // the list, if so, add the 'selected' class
                [].slice.call(results.children).map((item) => {
                    if (secondArray.includes(item.textContent)) {
                        item.classList.add("selected");
                    }
                });

                // add the elements from the array separated by commas
                // to the 'input' field, also add a comma to the last element
                element.value = `${secondArray.join(", ")}${secondArray > 2 ? secondArray.pop()[secondArray.length - 1] : ", "
                    }`;

                // after selecting an item, set the
                // focus to the input field
                element.focus();
            },

            onReset: (element) => {
                // after clicking the 'x' button,
                // clear the table
                secondArray = [];
            },
        });
    }
}