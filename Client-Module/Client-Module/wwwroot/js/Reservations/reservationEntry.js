const ReservationState = Object.freeze({
    "Pending": 1,
    "Ongoing": 2,
    "Completed": 3,
    "getLiteral": function (stateNumber) {
        for (const state in this) {
            if (this[state] === stateNumber) {
                return state;
            }
        }
    }
});

function ReservationEntry({
    reservationData,
    reviewManager,
    reservationManager
} = {}) {
    this.reservationData = reservationData;
    this.reviewManager = reviewManager;
    this.reservationManager = reservationManager;

    var from = new Date(reservationData.reservationInfo.from).getTime();
    var to = new Date(reservationData.reservationInfo.to).getTime() + 24 * 60 * 60 * 1000;
    var now = Date.now();
    if (from > now) {
        this.reservationState = ReservationState.Pending;
    } else if (to > now) {
        this.reservationState = ReservationState.Ongoing;
    } else {
        this.reservationState = ReservationState.Completed;
    }

    this.reservationComponent = this.createReservationComponent();
}

ReservationEntry.prototype.getReviewID = function () {
    if (!this.reservationData.reservationInfo.hasOwnProperty("reviewID")) {
        return null;
    }
    return this.reservationData.reservationInfo.reviewID;
}
ReservationEntry.prototype.setReviewID = function (reviewID = null) {
    var oldReviewID = this.getReviewID();
    this.reservationData.reservationInfo.reviewID = reviewID;
    if ((oldReviewID === null && reviewID === null) || (oldReviewID !== null && reviewID !== null)) {
        return;
    }
    if (this.reservationComponent === null) {
        return;
    }
    if (reviewID === null) {
        delete this.reservationData.reservationInfo.reviewID;
    }
    var reviewComponent = $(this.reservationComponent).find(".review-component");
    reviewComponent.replaceWith(this.createReservationReviewComponent().addClass("flex-grow-0 pl-2"));
}

ReservationEntry.prototype.getReservationID = function () {
    return this.reservationData.reservationInfo.reservationID;
}

ReservationEntry.prototype.refreshComponent = function () {
    if (this.reservationComponent === null) {
        throw new Error("This instance does not have a created reservation component");
    }
    var oldComponent = $(this.reservationComponent);
    oldComponent.replaceWith(this.createComponent());
};

ReservationEntry.prototype.createTitleHeaderComponent = function() {
    var reservationData = this.reservationData;
    var titleHeaderComponent = $("<div>").addClass("d-flex align-items-center");

    if (this.reservationState === ReservationState.Pending) {
        titleHeaderComponent.addClass("flex-row-reverse flex-wrap");
        var cancelButton = $("<button>").addClass("btn btn-danger ml-auto btn-reservation-cancel").append(
            $("<span>").attr("style", "font-weight: 500").text("Cancel reservation")
        );
        cancelButton.on("click", () => {
            this.reservationManager.onCancelReservation(this);
        });
        titleHeaderComponent.append(cancelButton);
    }

    titleHeaderComponent.append(
        $("<p>").addClass("m-0 mr-auto").append(
            $("<span>").attr("style", "font-size: 22px").append(
                $("<span>").attr("style", "font-weight: 600").text("Hotel:"),
                $("<i>").addClass("mx-3").text(reservationData.hotelInfoPreview.hotelName)
            ),
            $("<small>").text(`(${reservationData.hotelInfoPreview.country}, ${reservationData.hotelInfoPreview.city})`)
        )
    );
    return titleHeaderComponent;
}

ReservationEntry.prototype.createReservationReviewComponent = function() {
    var reviewComponent = $("<div>").attr("class", "d-flex flex-row flex-wrap align-items-center review-component");
    reviewComponent.append(
        $("<span>")
            .attr("class", "flex-grow-0")
            .attr("style", "font-size: 18px; font-weight: 600")
            .text("Your reservation review:")
    );

    switch (this.reservationState) {
        case ReservationState.Pending:
        case ReservationState.Ongoing: {
            reviewComponent.append(
                $("<span>")
                    .attr("class", "flex-grow-0 ml-4")
                    .attr("style", "font-size: 15px; font-weight: 400; color: gray")
                    .text("Your reservation must be completed in order to create a review")
            );
            break;
        }
        case ReservationState.Completed: {
            if (this.getReviewID() === null) {
                var createReviewButton = $("<button>")
                    .attr("class", "btn btn-sm btn-success ml-3")
                    .text("Create");
                createReviewButton.append($(`<i class="far fa-comment-dots ml-2">`));
                createReviewButton.on("click", () => {
                    this.reviewManager.onCreateReview(this);
                });
                reviewComponent.append(createReviewButton);
            } else {
                var editReviewButton = $("<button>")
                    .attr("class", "btn btn-sm btn-warning ml-3")
                    .text("Edit");
                editReviewButton.append($(`<i class="far fa-edit ml-2">`));
                editReviewButton.on("click", () => {
                    this.reviewManager.onEditReview(this);
                });
                var deleteReviewButton = $("<button>")
                    .attr("class", "btn btn-sm btn-danger ml-3")
                    .text("Delete");
                deleteReviewButton.append($(`<i class="far fa-trash-alt ml-2">`));
                deleteReviewButton.on("click", () => {
                    this.reviewManager.onDeleteReview(this);
                });
                reviewComponent.append(editReviewButton, deleteReviewButton);
            }
            break;
        }
    }
    return reviewComponent;
}

ReservationEntry.prototype.createReservationInfoComponent = function() {
    var reservationData = this.reservationData;
    var reservationInfoComponent = $(`<div>`).attr("class", "d-flex flex-column");

    reservationInfoComponent.append(
        $("<div>").attr("class", "flex-grow-0").append(
            $("<p>").attr("class", "m-0 p-1 text-center")
                .attr("style", "font-size: 18px")
                .text(reservationData.offerInfoPreview.offerTitle)
        )
    );
    reservationInfoComponent.append($("<hr>").attr("class", "my-1 w-100"));

    var reservationStatusComponent = $("<div>").attr("class", "flex-grow-1");
    var textClassStateMapper = {};
    textClassStateMapper[ReservationState.Pending] = "text-warning";
    textClassStateMapper[ReservationState.Ongoing] = "text-danger";
    textClassStateMapper[ReservationState.Completed] = "text-success";
    reservationStatusComponent.append(
        $("<div>").attr("class", "d-flex flex-row pl-2 mb-2 align-items-center").append(
            $("<span>").attr("class", "mr-3")
                .attr("style", "font-size: 18px; font-weight: 600")
                .text("Reservation status:"),
            $("<span>")
                .attr("class", textClassStateMapper[this.reservationState])
                .attr("style", "font-size: 18px; font-weight: 600")
                .text(ReservationState.getLiteral(this.reservationState))
        )
    );

    reservationStatusComponent.append(
        $("<div>").attr("class", "d-flex flex-row pl-3 align-items-center").append(
            $("<i>").attr("class", "flex-grow-0 far fa-calendar-minus mx-2"),
            $("<p>").attr("class", "flex-grow-1 m-0").append(
                $("<span>").attr("style", "color: gray").text("Reservation period:"),
                $("<span>").attr("class", "mx-3").text(`${new Date(reservationData.reservationInfo.from).toDateString()} — ${new Date(reservationData.reservationInfo.to).toDateString()}`)
            )
        )
    );

    reservationStatusComponent.append(
        $("<div>").attr("class", "d-flex flex-row pl-3 align-items-center").append(
            $("<i>").attr("class", "flex-grow-0 fas fa-user-tie mx-2"),
            $("<p>").attr("class", "flex-grow-1 m-0").append(
                $("<span>").attr("style", "color: gray").text("Number of adults:"),
                $("<span>").attr("class", "mx-3").text(`${reservationData.reservationInfo.numberOfAdults}`)
            )
        )
    );

    reservationStatusComponent.append(
        $("<div>").attr("class", "d-flex flex-row pl-3 mb-0 align-items-center").append(
            $("<i>").attr("class", "flex-grow-0 fas fa-child mx-2"),
            $("<p>").attr("class", "flex-grow-1 m-0").append(
                $("<span>").attr("style", "color: gray").text("Number of children:"),
                $("<span>").attr("class", "mx-3").text(`${reservationData.reservationInfo.numberOfChildren}`)
            )
        )
    );

    reservationInfoComponent.append(reservationStatusComponent);
    reservationInfoComponent.append($("<hr>").attr("class", "my-2 w-100"));
    reservationInfoComponent.append(this.createReservationReviewComponent().addClass("flex-grow-0 pl-2"));

    return reservationInfoComponent;
}

ReservationEntry.prototype.createReservationComponent = function() {
    var reservationComponent = $("<li>").attr("class", "list-group-item rounded-0 m-1 mb-3 bg-light reservation-list-entry");

    reservationComponent.append(this.createTitleHeaderComponent());
    reservationComponent.append($("<hr>").attr("class", "my-2 border-secondary"));

    reservationComponent.append(
        $("<div>").attr("class", "d-flex flex-row flex-wrap justify-content-center").append(
            $("<div>").attr("class", "flex-grow-0 my-2").append(
                $("<img>")
                    .attr("class", "d-block rounded mx-auto")
                    .attr("style", "width: 230px; height: 180px; background-color: red")
                    .attr("src", this.reservationData.offerInfoPreview.offerPreviewPicture)
            ),
            this.createReservationInfoComponent().addClass("flex-grow-1 ml-2")
        )
    );

    return reservationComponent;
}