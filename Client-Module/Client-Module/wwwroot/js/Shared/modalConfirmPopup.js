function ModalConfirmPopup({
    modal
} = {}) {
    this.modal = modal;
    this.isClosed = true;
    this.onCurrentOperationResolve = null;
    this.onCurrentOperationReject = null;
    this.confirmBtn = $(".popup-confirm");
    this.cancelBtn = $(".popup-cancel");
    this.popupInput = $(".popup-input");
    this.popupOutput = $(".popup-output");

    this.confirmBtn.on("click", () => {
        if (this.onCurrentOperationResolve !== null) {
            this.onCurrentOperationResolve(true);
        }
    });

    this.cancelBtn.on("click", () => {
        if (this.onCurrentOperationResolve !== null) {
            this.onCurrentOperationResolve(false);
        }
    });

    this.modal.find("button.close").on("click", () => {
        if (this.onCurrentOperationResolve !== null) {
            this.onCurrentOperationResolve(false);
        }
    });

    this.modal.on("show.bs.modal", () => {
        this.isClosed = false;
    });

    this.modal.on("hide.bs.modal", () => {
        this.isClosed = true;
    });

}

ModalConfirmPopup.prototype.getModalInput = function () {
    return new Promise((resolve, reject) => {
        this.onCurrentOperationResolve = resolve;
        this.onCurrentOperationReject = reject;
    });
}

ModalConfirmPopup.prototype.hideModal = function () {
    $(this.modal).modal("hide");
}

ModalConfirmPopup.prototype.showModal = function () {
    $(this.modal).modal("show");
}

ModalConfirmPopup.prototype.displayQuestion = function (question) {
    this.popupInput.addClass("d-flex");
    this.popupInput.removeClass("d-none");
    this.popupOutput.empty();
    this.popupOutput.append(
        $("<h5>").attr("class", "text-secondary text-center mb-3").text(question)
    );
}

ModalConfirmPopup.prototype.displayProcessing = function () {
    this.popupInput.removeClass("d-flex");
    this.popupInput.addClass("d-none");
    this.popupOutput.empty();
    this.popupOutput.append(
        $("<h5>").attr("class", "text-center").text("Processing..."),
        $("<img>")
            .attr("src", "/resources/loading.gif")
            .attr("style", "width: 50px; height: 50px")
            .attr("class", "d-block ml-auto mr-auto")
    );
}

ModalConfirmPopup.prototype.displaySuccess = function (successMessage) {
    this.popupInput.removeClass("d-flex");
    this.popupInput.addClass("d-none");
    this.popupOutput.empty();
    this.popupOutput.append(
        $("<h5>")
            .attr("class", "text-center text-success")
            .text(successMessage)
    );
}

ModalConfirmPopup.prototype.displayError = function (errorMessage) {
    this.popupInput.removeClass("d-flex");
    this.popupInput.addClass("d-none");
    this.popupOutput.empty();
    this.popupOutput.append(
        $("<h5>")
            .attr("class", "text-center text-danger")
            .text(errorMessage)
    );
}