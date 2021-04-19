function InputValidator({
    inputHandle = null,
    errorBoxHandle = null,
    inputValidator = null,
    validationError = null
} = {}) {
    this.inputHandle = inputHandle;
    this.errorBoxHandle = errorBoxHandle;
    this.inputValidator = inputValidator;
    this.validationError = validationError;
}

InputValidator.prototype.isInputValid = function () {
    return this.inputValidator(this.inputHandle.val());
}

InputValidator.prototype.hideValidationError = function () {
    this.errorBoxHandle.addClass("d-none");
    this.inputHandle.removeClass("input-validation-error");
}

InputValidator.prototype.showValidationError = function () {
    this.errorBoxHandle.removeClass("d-none");
    this.errorBoxHandle.text(this.validationError);
    this.inputHandle.addClass("input-validation-error");
}

InputValidator.prototype.validate = function () {
    this.hideValidationError();
    var isValid;
    if (!(isValid = this.isInputValid())) {
        this.showValidationError();
    }
    return isValid;
}

InputValidator.prototype.val = function () {
    return this.inputHandle.val();
}