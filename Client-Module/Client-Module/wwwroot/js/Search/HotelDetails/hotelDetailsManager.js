class HotelDetailsManager extends ServerApiManager {
    constructor(apiConfig) {
        super(apiConfig);
    }

    getHotelDetails(hotelID) {
        return this.createRequest({
            httpVerb: "GET",
            apiURL: `/hotels/${hotelID}`
        });
    }
}