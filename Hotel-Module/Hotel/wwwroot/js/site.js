// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// preview selected images in gallery
function previewGallery(inputElement, gallery) {
    var count = inputElement.files.length;

    gallery.innerHTML = '';
    for (i = 0; i < count; i++) {
        var urls = URL.createObjectURL(inputElement.files[i]);
        gallery.innerHTML +=
            `<img src="` + urls + `" class="gallery-img" />`;
    }
}

// checkbox controlling element's enabled/disabled attribute
// and gallery content
function switchEnabled(checkbox, inputElement, gallery, defaultGalleryHtml) {
    if (checkbox.checked == true) {
        inputElement.value = '';
        inputElement.disabled = false;
        gallery.innerHTML = '';
    } else {
        inputElement.value = '';
        inputElement.disabled = true;
        gallery.innerHTML = defaultGalleryHtml;
    }
}

// function used when sending a picture to controller
async function get64BaseString(file) {
    var str;
    await readUploadedImage(file)
        .then(value => {
            str = value;
        })
        .catch(function () {
            console.log("oops");
        });
    return str;
}
function readUploadedImage(file) {
    var reader = new FileReader();
    return new Promise((resolve, reject) => {
        reader.onerror = () => {
            reader.abort();
            reject();
        };
        reader.onload = () => {
            resolve(reader.result);
        };
        reader.readAsDataURL(file);
    });
}

//const loader = document.querySelector("#loader");
// showing loading
function displayLoading(loader) {
    loader.classList.add("display");
}

// hiding loading 
function hideLoading(loader) {
    loader.classList.remove("display");
}

async function fetchSend(confirmationMessage, url, httpMethod, formData, succesMessage, redirectUrl, loaderElem) {
    if (confirmationMessage && !confirm(confirmationMessage))
        return;

    if (loaderElem) {
        displayLoading(loaderElem);
    }

    let obj = formData ? {
        method: httpMethod,
        body: formData
    } : {
        method: httpMethod
    };
    await fetch(url,
        obj
    ).then((response) => {
        if (loaderElem) {
            hideLoading(loaderElem)
        }
        if (response.ok) {
            if (succesMessage) {
                alert(succesMessage);
            }
            window.location.replace(redirectUrl);
        } else {
            alert("error: " + response.status);
        }
    }).catch((error) => {
        if (loaderElem) {
            hideLoading(loaderElem)
        }
        console.error('fetch error: ', error);
    });
}
