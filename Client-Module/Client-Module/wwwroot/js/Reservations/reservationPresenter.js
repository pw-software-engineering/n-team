function ReservationPresenter({
    reservationListDisplay
} = {}) {
    this.reservationListDisplay = reservationListDisplay;
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
            $("<li>").append(
                $(`<h2 class="text-center text-secondary">`)
                    .attr("class", "text-center text-secondary font-weight-normal")
                    .text("There are no reservations on this page")
            )
        )
    }
}