class HotelOfferReservationModal {
    constructor({
        modalHandle = null,
        inputHandles = {
            fromTimeInput: null,
            toTimeInput: null,
            numberOfAdultsInput: null,
            numberOfChildrenInput: null
        },
        guestInputsValidationBox = null,
        createReservationButtonHandle = null,
        serverErrorBox = null,
        offerAvailabilityListHandle = null,
        hotelOfferData = null
    } = {}) {
        this.modalHandle = modalHandle;
        this.inputHandles = inputHandles;
        this.createReservationButtonHandle = createReservationButtonHandle;
        this.hotelOfferData = hotelOfferData;
        this.guestInputsValidationBox = guestInputsValidationBox;
        this.serverErrorBox = serverErrorBox;
        this.offerAvailabilityListHandle = offerAvailabilityListHandle;

        this.currentUserInputPromiseResolve = null;
        this.currentUserInputPromiseReject = null;

        this.offerAvailabilityListHandle.empty();
        // cast undefined result from null coalescing to 0
        if (this.hotelOfferData.availabilityTimeIntervals?.length == 0) {
            this.offerAvailabilityListHandle.append(
                $("<li>")
                    .addClass("offer-availability-list-item p-2")
                    .text("This offer is not available during next 6 months")
            );
        } else {
            for (var i = 0; i < this.hotelOfferData.availabilityTimeIntervals.length; i++) {
                this.offerAvailabilityListHandle.append(
                    $("<li>").addClass("offer-availability-list-item p-2").append(
                        $("<span>").text(formatDate(this.hotelOfferData.availabilityTimeIntervals[i].startDate)),
                        $("<span>").addClass("text-secondary mx-3").html('&mdash;'),
                        $("<span>").text(formatDate(this.hotelOfferData.availabilityTimeIntervals[i].endDate))
                    )
                );
            }
        }

        var now = new Date(new Date(new Date().toISOString().split('T')[0]).getTime() + 24 * 60 * 60 * 1000);
        var fromTimeInput = this.inputHandles.fromTimeInput;
        fromTimeInput.focusout(function () {
            var input = $(this);
            var inputData = input.val();
            if (!inputData || new Date(inputData) < now) {
                input.val(now.toISOString().split('T')[0]);
                inputData = input.val();
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
            if (!inputData || new Date(inputData) < now) {
                input.val(now.toISOString().split('T')[0]);
                inputData = input.val();
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

        this.createReservationButtonHandle.on('click', () => {
            this.guestInputsValidationBox.addClass("d-none");
            var numberOfAdults = Number(this.inputHandles.numberOfAdultsInput.val());
            var numberOfChildren = Number(this.inputHandles.numberOfChildrenInput.val());
            if (numberOfAdults < 0 || numberOfChildren < 0) {
                this.guestInputsValidationBox
                    .removeClass("d-none")
                    .text("Both fields 'Number of adults' and 'Number of children' must contain a non-negative integer");
                return;
            }
            if (numberOfAdults + numberOfChildren > this.hotelOfferData.maxGuests) {
                this.guestInputsValidationBox
                    .removeClass("d-none")
                    .text(`Number of children and adults combined must be less or equal than maximum number of guests: ${this.hotelOfferData.maxGuests}`);
                return;
            }
            if (numberOfAdults + numberOfChildren <= 0) {
                this.guestInputsValidationBox
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
            this.currentUserInputPromiseResolve = null;
            this.currentUserInputPromiseReject = null;
        });

        this.modalHandle.on('hide.bs.modal', (e) => {
            //e.preventDefault();
            if (!this.isModalActive) {
                return;
            }
            this.isModalActive = false;
            this.currentUserInputPromiseReject?.(new Error("Modal has been forcibly closed by the user"));
            this.currentUserInputPromiseResolve = null;
            this.currentUserInputPromiseReject = null;
        });

        this.currentSessionID = 0;
        this.isModalActive = false;

        this.onDisplayStateChanged = () => { };
    }

    createSession() {
        this.onDisplayStateChanged?.();
        this.guestInputsValidationBox.addClass("d-none");
        this.isModalActive = true;
        this.modalHandle.modal('show');
        return ++this.currentSessionID;
    }

    getUserInput(sessionID) {
        if (sessionID !== this.currentSessionID || !this.isModalActive) {
            return false;
        }
        return new Promise((resolve, reject) => {
            this.currentUserInputPromiseResolve = resolve;
            this.currentUserInputPromiseReject = reject;
        });
    }

    displayLoading(sessionID) {
        if (sessionID !== this.currentSessionID || !this.isModalActive) {
            return false;
        }
        this.onDisplayStateChanged?.();
        this.createReservationButtonHandle.attr("disabled", true);
        this.serverErrorBox.empty();
        this.serverErrorBox.append(
            $("<p>").attr("class", "text-center h3").append(
                $("<img>")
                    .attr("src", `${pathBase}/resources/loading.gif`)
                    .attr("style", "width: 50px; height: 50px")
                    .attr("class", "d-inline-block mr-3"),
                $("<span>").attr("class", "text-secondary").css("vertical-align", "middle").text("Processing...")
            )
        );
        this.onDisplayStateChanged = () => {
            this.createReservationButtonHandle.attr("disabled", false);
            this.serverErrorBox.empty();
            this.onDisplayStateChanged = null;
        }
        return true;
    }

    displayServerError(sessionID, errorText) {
        if (sessionID !== this.currentSessionID || !this.isModalActive) {
            return false;
        }
        this.onDisplayStateChanged?.();
        this.serverErrorBox.append(
            $("<p>")
                .attr("class", "text-center text-danger h3")
                .text("Could not create the reservation"),
            $("<p>")
                .attr("class", "text-center text-danger h5 px-3")
                .text(errorText)
        )
        this.onDisplayStateChanged = () => {
            this.serverErrorBox.empty();
            this.onDisplayStateChanged = null;
        };
        return true;
    }

    displaySuccess(sessionID) {
        if (sessionID !== this.currentSessionID || !this.isModalActive) {
            return false;
        }
        this.onDisplayStateChanged?.();
        this.serverErrorBox.append(
            $("<p>")
                .attr("class", "text-center text-success h5 px-3")
                .text("Operation completed successfully")
        )
        this.onDisplayStateChanged = () => {
            this.serverErrorBox.empty();
            this.onDisplayStateChanged = null;
        };
        return true;
    }

    closeSession(sessionID) {
        if (sessionID !== this.currentSessionID || !this.isModalActive) {
            return false;
        }
        this.isModalActive = false;
        this.modalHandle.modal('hide');
        return true;
    }
}