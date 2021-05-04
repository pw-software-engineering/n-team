// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// preview selected images in gallery
function previewGallery(inputFileElement, galleryId) {
    var count = inputFileElement.files.length;
    var gallery = document.getElementById(galleryId);

    gallery.innerHTML = "";
    for (i = 0; i < count; i++) {
        var urls = URL.createObjectURL(inputFileElement.files[i]);
        gallery.innerHTML +=
            `<img src="` + urls + `" class="gallery-img" />`;
    }
}

// checkbox controlling element's enabled/disabled attribute
function switchEnabled(checkbox, elementId) {
    var element = document.getElementById(elementId);

    if (checkbox.checked == false) {
        element.disabled = true;
    } else {
        element.disabled = false;
    }
}

// function used when sending a picture to controller
async function get64BaseString(file) {
    var str;
    await readUploadedImage(file)
        .then(value => {
            str = value.split(',')[1];
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