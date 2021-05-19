class HotelOfferReviewsManager extends ServerApiManager {
    constructor({
        hotelID = null,
        offerID = null,
        apiConfig = null
    } = {}) {
        super(apiConfig);
        this.hotelID = hotelID;
        this.offerID = offerID;
    }

    getHotelOfferReviews({
        pageNumber = null,
        pageSize = null
    } = {}) {
        var queryParameters = [];
        queryParameters.push(
            pageNumber !== null ? `pageNumber=${Number(pageNumber)}` : null,
            pageSize !== null ? `pageSize=${Number(pageSize)}` : null
        );
        var queryString = queryParameters.filter(Boolean).join('&');
        if (queryString) {
            queryString = '?' + queryString;
        }
        return this.createRequest({
            httpVerb: "GET",
            apiURL: `/hotels/${this.hotelID}/offers/${this.offerID}/reviews${queryString}`
        });
    }
}