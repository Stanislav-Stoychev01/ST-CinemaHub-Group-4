function showAlert(message) {
  
}
function handleError(response) {
    let promise = new Promise((resolve, reject) => {
        if (response.status == 404) {
            reject("Resourse not found");
        }
        if (response.status == 401) {
            reject("Access is denied");
        }
        if (response.status == 400) {
            response.json().then((err) => {
                var message = "";
                for (var i in err.errors) {
                    message += err.errors[i].field + ": ";
                    for (var p in err.errors[i].message) {
                        message += err.errors[i].message[p] + ";";
                    }
                }
                reject(message)

            });
        }

    })
    return promise;
}
function getDate(dateTime) {
    var date = new Date(dateTime+"Z");
    console.log(dateTime);
    return date.getDate() + "." + (date.getMonth() + 1) + "." + date.getFullYear();

}
function getHours(dateTime) {
    var date = new Date(dateTime+"Z");
    var minutes = "";
    if (date.getMinutes() < 9) {
        minutes = "0" + date.getMinutes();
    } else {
        minutes = minutes;
    }
    return date.getHours() + ":" + minutes;
}