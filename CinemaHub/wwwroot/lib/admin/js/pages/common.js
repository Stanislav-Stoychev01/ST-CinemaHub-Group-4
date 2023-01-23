const toastLiveExample = document.getElementById('liveToast')
const toast = new coreui.Toast(toastLiveExample)
function showAlert(message) {
    $("#liveToast .toast-body").html(message);
    toast.show()
}



function parseForm(model) {
    var form = {};
    var keys = Reflect.ownKeys(model);
    for (var i in keys) {
        if (typeof (model[keys[i]]) == "function" && keys[i] != "errors") {
            form[keys[i]] = model[keys[i]]();
        }
    }
    return form;
}

function handleError(response) {
    let promise = new Promise((resolve, reject) => {
        if (response.status == 404) {
            reject("Resourse not found");
        }
        if (response.status == 401) {
            reject("Access is denied");
            location.reload();
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

function formatDate(date){
    var dateTime = new Date(date+"Z");
    return dateTime.toLocaleString();
}