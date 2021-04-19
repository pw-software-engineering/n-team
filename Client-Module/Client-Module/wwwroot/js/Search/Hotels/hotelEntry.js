function HotelEntry({
    hotelData = null,
    hotelEntryEvents = {
        onDetails: () => { },
        onOffers: () => { }
    }
} = {}) {
    this.hotelData = hotelData;
    this.hotelEntryEvents = hotelEntryEvents;
    this.component = this.createComponent();
}

HotelEntry.prototype.createComponent = function () {
    var hotelComponent = $("<li>").attr("class", "list-group-item rounded-0 m-1 mb-3 bg-light reservation-list-entry");
    hotelComponent.append(this.createHeaderComponent());
    hotelComponent.append($("<hr>").attr("class", "my-1 border-secondary"));
    hotelComponent.append(this.createHotelInfoComponent());
    return hotelComponent;
}

HotelEntry.prototype.createHeaderComponent = function () {
    var headerComponent = $("<div>");
    headerComponent.append(
        $("<p>").attr("class", "m-0").append(
            $("<span>").attr("style", "font-size: 22px").append(
                $(`<i class="fas fa-hotel mr-2" style="color: gray">`),
                $("<b>").text("Hotel:"),
                $("<i>").attr("class", "mx-3").text(this.hotelData.hotelName)
            )
        )
    );
    return headerComponent;
}

HotelEntry.prototype.createHotelInfoComponent = function () {
    var hotelInfoComponent = $("<div>")
        .attr("class", "d-flex flex-row")
        .attr("style", "flex-wrap: wrap; justify-content: center");
    var hotelPreviewPictureComponent = $("<div>").attr("class", "flex-grow-0 p-2").append(
        $("<img>")
            .attr("class", "d-block rounded mx-auto")
            .attr("style", "width: 260px; height: 200px;")
            .attr("src", this.hotelData.previewPicture)
    );
    hotelInfoComponent.append(hotelPreviewPictureComponent);

    var hotelInfoPanelComponent = $("<div>").attr("class", "flex-grow-1 p-2").append(
        $("<b>").attr("class", "m-1").attr("style", "font-size: 18px").text("Location:"),
        $("<ul>").attr("class", "pl-4").attr("style", "list-style-type: none").append(
            $("<li>").append(
                $(`<i class="mr-2 fas fa-globe-americas" style="color: gray">`),
                $("<span>").text("Country: "),
                $("<span>").attr("style", "font-weight: 500").text(this.hotelData.country)
            ),
            $("<li>").append(
                $(`<i class="mr-2 fas fa-city" style="color: gray">`),
                $("<span>").text("City: "),
                $("<span>").attr("style", "font-weight: 500").text(this.hotelData.city)
            )
        )
    );

    var detailsBtn = $("<a>")
        .attr("class", "w-100 my-2 btn btn-secondary mt-4")
        .text("Find out more")
        .append($(`<i class="ml-2 fas fa-chevron-right">`))
        .attr("href", `/hotels/${this.hotelData.hotelID}`);
    detailsBtn.on("click", () => {
        this.hotelEntryEvents.onDetails(this);
    });
    var offersBtn = $("<a>")
        .attr("class", "w-100 my-2 btn btn-info")
        .text("Show offers")
        .append($(`<i class="ml-2 fas fa-chevron-right">`))
        .attr("href", `/hotels/${this.hotelData.hotelID}/offers`);
    offersBtn.on("click", () => {
        this.hotelEntryEvents.onOffers(this);
    });
    hotelInfoPanelComponent.append(detailsBtn, offersBtn);

    hotelInfoComponent.append(hotelInfoPanelComponent);
    return hotelInfoComponent;
}