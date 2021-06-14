function ModalReviewPopup({
    modal
} = {}) {
    this.modal = modal;
    this.isClosed = true;
    this.onCurrentOperationResolve = null;
    this.onCurrentOperationReject = null;

    this.contentValidBox = this.modal.find(".content-valid-box");
    this.ratingValidBox = this.modal.find(".rating-valid-box");

    this.modal.find("button.review-update-btn").on("click", () => {
        if (this.onCurrentOperationResolve === null) {
            return;
        }
        var isValid = true;
        if (this.getReviewContent().trim().length == 0) {
            this.contentValidBox
                .removeClass("d-none")
                .text("Review content must consist of at least 1 non-whitespace character");
            isValid = false;
        } else {
            this.contentValidBox.addClass("d-none");
        }
        if (!this.getRating() || this.getRating() < 1 || this.getRating() > 10) {
            this.ratingValidBox
                .removeClass("d-none")
                .text("Select rating value from 1 to 10");
            isValid = false;
        } else {
            this.ratingValidBox.addClass("d-none");
        }
        if (!isValid) {
            return;
        }
        this.onCurrentOperationResolve(true);
        this.onCurrentOperationResolve = null;
    });

    this.modal.find("button.close").on("click", () => {
        if (this.onCurrentOperationResolve === null) {
            return;
        }
        this.onCurrentOperationResolve(false);
        this.onCurrentOperationResolve = null;
    });

    this.modal.on("show.bs.modal", () => {
        this.isClosed = false;
    });

    this.modal.on("hide.bs.modal", () => {
        this.isClose = true;
    });
}

ModalReviewPopup.prototype.getRating = function () {
    var ratingInput = this.modal.find("select");
    return Number(ratingInput.val());
}

ModalReviewPopup.prototype.setRating = function (rating) {
    var ratingInput = this.modal.find("select");
    if (rating !== null) {
        rating = Math.max(1, Math.min(5, Number(rating)));
    } else {
        rating = "";
    }
    ratingInput.val(rating);
}

ModalReviewPopup.prototype.getReviewContent = function () {
    var contentTextarea = this.modal.find("textarea");
    return contentTextarea.val();
}

ModalReviewPopup.prototype.setReviewContent = function (content) {
    var contentTextarea = this.modal.find("textarea");
    contentTextarea.val(content);
}

ModalReviewPopup.prototype.setAppearance = function (type) {
    var updateBtn = this.modal.find(".review-update-btn");
    switch (type) {
        case "edit": {
            updateBtn
                .removeClass("btn-success")
                .addClass("btn-warning")
                .text("Edit review")
            break;
        }
        case "create": {
            updateBtn
                .removeClass("btn-warning")
                .addClass("btn-success")
                .text("Create review")
            break;
        }
    }
}

ModalReviewPopup.prototype.showModal = function () {
    $(this.modal).modal("show");
}

ModalReviewPopup.prototype.hideModal = function () {
    $(this.modal).modal("hide");
}

ModalReviewPopup.prototype.clearDisplay = function () {
    this.onCurrentOperationReject = null;
    this.onCurrentOperationResolve = null;
    this.setRating(null);
    this.setReviewContent("");
    this.contentValidBox.addClass("d-none");
    this.ratingValidBox.addClass("d-none");
    var reviewStatusBox = this.modal.find(".review-status-box");
    reviewStatusBox.empty();
    var updateBtn = this.modal.find(".review-update-btn");
    updateBtn.prop("disabled", false);
    this.setAppearance("create");
}

ModalReviewPopup.prototype.getModalInput = function () {
    return new Promise((resolve, reject) => {
        this.onCurrentOperationResolve = resolve;
        this.onCurrentOperationReject = reject;
    });
}

ModalReviewPopup.prototype.displayProcessing = function () {
    var reviewStatusBox = this.modal.find(".review-status-box");
    reviewStatusBox.empty();
    reviewStatusBox.append(
        $("<h5>").attr("class", "text-center").text("Processing..."),
        $("<img>")
            .attr("src", `${pathBase}/resources/loading.gif`)
            .attr("style", "width: 50px; height: 50px")
            .attr("class", "d-block ml-auto mr-auto mb-3")
    );
}

ModalReviewPopup.prototype.displayError = function (errorMessage) {
    var reviewStatusBox = this.modal.find(".review-status-box");
    reviewStatusBox.empty();
    reviewStatusBox.append(
        $("<h5>")
            .attr("class", "text-center font-weight-normal text-danger mb-3")
            .text(errorMessage)
    );
}

ModalReviewPopup.prototype.displaySuccess = function () {
    var reviewStatusBox = this.modal.find(".review-status-box");
    reviewStatusBox.empty();
    reviewStatusBox.append(
        $("<h5>")
            .attr("class", "text-center font-weight-normal text-success mb-3")
            .text("Review updated successfully")
    );
}
