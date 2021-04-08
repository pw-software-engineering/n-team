// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// preview of the selected preview image
function previewImg() {
    var preview = document.getElementById('previewImage');
    var file = document.getElementById('previewFile').files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        preview.src = reader.result;
    }

    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.src = "";
    }
}

// preview of the selected additional images
function previewMultiple(event) {
    var input = document.getElementById("additionalFiles");
    var count = input.files.length;

    document.getElementById("gallery").innerHTML = "";
    for (i = 0; i < count; i++) {
        var urls = URL.createObjectURL(event.target.files[i]);
        document.getElementById("gallery").innerHTML +=
            `<img src="` + urls + `" class="gallery-img" />`;
    }
}

// checkbox controlling element's enabled/disabled attribute
function switchDisability(checkboxId, elementId) {
    var checkbox = document.getElementById(checkboxId);
    var input = document.getElementById(elementId);

    if (checkbox.checked == false) {
        input.disabled = true;
    } else {
        input.disabled = false;
    }
}
