class HotelOfferReservationModal {
    constructor({
        modalHandle = null,
        inputHandles = {
            fromTimeInput: null,
            toTimeInput: null,
            numberOfAdultsInput: null,
            numberOfChildrenInput: null
        },
        createReservationButtonHandle = null,
        hotelOfferData = null
    } = {}) {
        this.modalHandle = modalHandle;
        this.inputHandles = inputHandles;
        this.createReservationButtonHandle = createReservationButtonHandle;
        this.hotelOfferData = hotelOfferData;

        this.modalHandle.on('hide.bs.modal', (e) => {
            //e.preventDefault();
        });

        this.modalHandle.on('show.bs.modal', (e) => {

        });

        this.currentUserInputPromiseResolve = null;
        this.currentUserInputPromiseReject = null;

        var fromTimeInput = this.inputHandles.fromTimeInput;
        fromTimeInput.focusout(function () {
            var input = $(this);
            var inputData = input.val();
            var now = new Date().toISOString().split('T')[0];
            if (!inputData || new Date(inputData) < new Date(now)) {
                input.val(now);
            }
            if (new Date(inputData) > new Date(toTimeInput.val())) {
                toTimeInput.val(inputData);
            }
            console.log(inputData);
        });

        var toTimeInput = this.inputHandles.toTimeInput;
        toTimeInput.focusout(function () {
            var input = $(this);
            var inputData = input.val();
            var now = new Date().toISOString().split('T')[0];
            if (!inputData || new Date(inputData) < new Date(now)) {
                input.val(now);
            }
            if (new Date(inputData) < new Date(fromTimeInput.val())) {
                fromTimeInput.val(inputData);
            }
            console.log(inputData);
        });

        this.inputHandles.numberOfAdultsInput.on('change', function () {
            var input = $(this);
            if (!input.val()) {
                return;
            }
            input.val(Math.max(0, Math.floor(Number(input.val()))));
        });

        this.inputHandles.numberOfChildrenInput.on('change', function () {
            var input = $(this);
            if (!input.val()) {
                return;
            }
            input.val(Math.max(0, Math.floor(Number(input.val()))));
        });

        var guestInputsValidationBox = $("#guests-validation-error");
        this.createReservationButtonHandle.on('click', () => {
            guestInputsValidationBox.addClass("d-none");
            var numberOfAdults = Number(this.inputHandles.numberOfAdultsInput.val());
            var numberOfChildren = Number(this.inputHandles.numberOfChildrenInput.val());
            console.log(numberOfAdults);
            console.log(numberOfChildren);
            console.log(this.hotelOfferData.maxGuests);
            if (!numberOfAdults || !numberOfChildren) {
                guestInputsValidationBox
                    .removeClass("d-none")
                    .text("Both fields 'Number of adults' and 'Number of children' must contain a non-negative integer");
                return;
            }
            if (numberOfAdults + numberOfChildren > this.hotelOfferData.maxGuests) {
                guestInputsValidationBox
                    .removeClass("d-none")
                    .text(`Number of children and adults combined must be less or equal than maximum number of guests: ${this.hotelOfferData.maxGuests}`);
                return;
            }
            if (numberOfAdults + numberOfChildren <= 0) {
                guestInputsValidationBox
                    .removeClass("d-none")
                    .text(`Number of children and adults combined must be greater or equal to 0`);
                return;
            }
            var resolveResult = {
                from: this.inputHandles.fromTimeInput.val(),
                to: this.inputHandles.toTimeInput.val(),
                numberOfChildren: numberOfChildren,
                numberOfAdults: numberOfAdults
            };
            this.currentUserInputPromiseResolve?.(resolveResult);
            console.log(resolveResult);
            this.currentUserInputPromiseResolve = null;
            this.currentUserInputPromiseReject = null;
        });

        this.currentSessionID = 0;
        this.isModalActive = false;
    }

    createSession() {
        this.isModalActive = true;
        this.modalHandle.modal('show');
        $("#guests-validation-error").addClass("d-none");
        return ++this.currentSessionID;
    }

    getUserInput(sessionID) {
        if (sessionID !== this.currentSessionID) {
            throw new Error("Invalid session ID");
        }
        return new Promise((resolve, reject) => {
            this.currentUserInputPromiseResolve = resolve;
            this.currentUserInputPromiseReject = reject;
        });
    }

    closeSession(sessionID) {
        if (sessionID !== this.currentSessionID) {
            throw new Error("Invalid session ID");
        }
        this.modalHandle.modal('hide');
    }
}