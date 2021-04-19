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
