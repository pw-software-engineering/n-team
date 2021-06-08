class HotelOfferReviewEntry {
    constructor({
        reviewData = null
    } = {}) {
        this.reviewData = reviewData;
        this.component = this.createComponent();
    }

    createComponent() {
        var reviewComponent = $("<li> ").attr("class", "offer-review p-3");
        reviewComponent.append(
            this.createReviewHeaderComponent(),
            $("<hr>").attr("class", "my-2").attr("style", "border-top: 1px dashed lightgray"),
            $("<p>").attr("class", "px-3 m-0").attr("style", "white-space: pre-wrap;").text(this.reviewData.content),
            $("<hr>").attr("class", "my-2").attr("style", "border-top: 1px dashed lightgray")
        );
        return reviewComponent;
    }

    createReviewHeaderComponent() {
        var reviewHeaderComponent = $("<div>").attr("class", "d-flex w-100 flex-row-reverse flex-wrap");

        var ratingBoxComponent = $("<div>").attr("class", "align-self-start text-nowrap");
        ratingBoxComponent.append(
            $("<b>").attr("class", "text-secondary").text("Rating: ")
        );
        var i = 2;
        for (; i < this.reviewData.rating; i += 2) {
            ratingBoxComponent.append('<i class="fas fa-star text-warning ml-1">');
        }
        if (i - this.reviewData.rating === 0) {
            ratingBoxComponent.append('<i class="fas fa-star text-warning ml-1">');
        } else {
            ratingBoxComponent.append('<i class="fas fa-star-half-alt text-warning ml-1">');
        }
        i += 2;
        for (; i <= 10; i += 2) {
            ratingBoxComponent.append('<i class="far fa-star text-warning ml-1">');
        }

        var reviewerInfoBoxComponent = $("<div>").attr("class", "mr-auto");
        reviewerInfoBoxComponent.append(
            $('<i class="fas fa-user mr-2">'),
            $("<span>").attr("class", "text-secondary").text(this.reviewData.reviewerUsername),
            $("<br>"),
            $('<i class="far fa-clock mr-2">'),
            $("<span>").attr("class", "text-secondary").text(formatDate(this.reviewData.creationDate))
        );

        reviewHeaderComponent.append(
            ratingBoxComponent,
            reviewerInfoBoxComponent
        );
        return reviewHeaderComponent;
    }
}