class HotelOfferEntry {
    constructor({
        hotelOfferData = null,
        hotelOfferEvents = {
            onOfferDetails: () => { }
        }
    } = {}) {
        this.hotelOfferData = hotelOfferData;
        this.hotelOfferEvents = hotelOfferEvents;
        this.component = this.createComponent();
    }

    createComponent() {
        var offerInfoComponent = $("<li>").attr("class", "list-group-item rounded-0 m-1 mb-3 bg-light reservation-list-entry");
        offerInfoComponent.append(
            this.createHeaderComponent(),
            $("<hr>").attr("class", "my-1 border-secondary"),
            this.createOfferInfoComponent()
        )
    }

    createHeaderComponent() {
        var headerComponent = $("<div>");
        return headerComponent.append(
            $("<p>").attr("class", "m-0 text-align-center").text(this.hotelOfferData.offerTitle).append(
                $('<i style="font-size: 22px">')
            )
        );
    }

    createOfferInfoComponent() {
        //justify-content-center to horizontally center flex items after they're wrapped
        var offerInfoComponent = $("<div>").attr("class", "d-flex flex-row flex-wrap justify-content-center");
        var offerPreviewPictureComponent = $("<div>").attr("class", "flex-grow-0 p-2");
        offerPreviewPictureComponent.append(
            $("<img>")
                .attr("class", "d-block rounded mx-auto")
                .attr("style", "width: 260px; height: 200px;")
                .attr("src", this.hotelOfferData.offerPreviewPicture)
        );
        offerInfoComponent.append(offerPreviewPictureComponent);
        var offerInfoPanelComponent = $("<div>").attr("flex-grow-1 p-2 d-flex flex-column");
        offerInfoPanelComponent.append(
            $("<p>").attr("class", "mb-2 mx-1").attr("style", "font-size: 19px").append(
                $("<span>").attr("style", "font-weight: 500").text("Max. number of guests: "),
                $("<span>").attr("class", "mx-2").text(data.maxGuests)
            ),
            $("<p>").attr("class", "my-0").attr("style", "font-size: 19px; font-weight: 500").text("Cost per person:"),
            $("<ul>").attr("class", "pl-4 mb-2 list-unstyled").append(
                $("<li>").append(
                    $('<i class="fas fa-user-tie mx-2">'),
                    $("<span>").attr("style", "color: gray").text("Adults: "),
                    $("<span>").attr("class", "mx-3").text(`${this.hotelOfferData.costPerAdult} zł`)
                ),
                $("<li>").append(
                    $('<i class="fas fa-user-tie mx-2">'),
                    $("<span>").attr("style", "color: gray").text("Children: "),
                    $("<span>").attr("class", "mx-3").text(`${this.hotelOfferData.costPerChild} zł`)
                )
            )
        );
        var offerDetailsBtn = $("<a>")
            .attr("class", "w-100 btn btn-secondary mt-auto")
            .attr("href", `/hotels/${this.hotelOfferData.hotelID}/offers/${this.hotelOfferData.offerID}`)
            .text("Find out more")
            .append($('<i class="ml-2 fas fa-chevron-right">'));
        offerDetailsBtn.on('click', this.hotelOfferEvents.onOfferDetails.bind(null, this));
        offerInfoPanelComponent.append(offerDetailsBtn);
        offerInfoComponent.append(offerInfoPanelComponent);
        return offerInfoComponent;
    }
}