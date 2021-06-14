function ReservationPresenter({
    reservationListDisplay
} = {}) {
    this.reservationListDisplay = reservationListDisplay;
}

ReservationPresenter.prototype.displayLoading = function () {
    this.reservationListDisplay.empty();
    this.reservationListDisplay.append(
        $("<li>")
            .attr("class", "list-group-item rounded-0 m-1 mb-3 bg-light reservation-list-entry")
            .append(
                $("<img>")
                    .attr("class", "d-block mx-auto mt-2")
                    .attr("style", "width: 75px").attr("src", `${pathBase}/resources/loading.gif`),
                $("<p>")
                    .attr("class", "text-center h5 mt-2")
                    .text("Loading hotel offer page...")
            )
    );
}

ReservationPresenter.prototype.displayError = function (errorString) {
    this.reservationListDisplay.empty();
    this.reservationListDisplay.append(
        $("<li>")
            .attr("class", "list-group-item rounded-0 m-1 mb-3 bg-light reservation-list-entry")
            .append(
                $("<p>")
                    .attr("class", "text-center text-danger h3")
                    .text("Could not fetch data from the server"),
                $("<p>")
                    .attr("class", "text-center text-danger h5")
                    .text(errorString)
            )
    );
}

ReservationPresenter.prototype.displayReservations = async function (reservationEntries) {
    this.reservationListDisplay.empty();
    var isEmpty = true;
    for (reservationID in reservationEntries) {
        isEmpty = false;
        this.reservationListDisplay.append(reservationEntries[reservationID].reservationComponent);
    }
    if (isEmpty) {
        this.reservationListDisplay.append(
            $("<li>")
                .attr("class", "list-group-item rounded-0 m-1 mb-3 bg-light reservation-list-entry")
                .append(
                    $("<p>")
                        .attr("class", "h4 my-4 text-secondary text-center")
                        .text("There are no reservations on this page")
                )
        );
    }
}