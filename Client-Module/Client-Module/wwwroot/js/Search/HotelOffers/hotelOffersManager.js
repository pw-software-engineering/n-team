class HotelOffersManager extends ServerApiManager {
    constructor({
        hotelID = null,
        apiConfig = null
    } = {}) {
        super(apiConfig);
        this.hotelID = hotelID;
    }

    getHotelOffers({
        from = null,
        to = null,
        minCost = null,
        maxCost = null,
        minGuests = null,
        pageNumber = null,
        pageSize = null
    } = {}) {
        var queryParameters = [];
        queryParameters.push(
            `fromTime=${from}`,
            `toTime=${to}`
        );
        queryParameters.push(
            minCost !== null ? `minCost=${Number(minCost)}` : null,
            maxCost !== null ? `maxCost=${Number(maxCost)}` : null,
            minGuests !== null ? `minGuests=${Number(minGuests)}` : null,
            pageNumber !== null ? `pageNumber=${Number(pageNumber)}` : null,
            pageSize !== null ? `pageSize=${Number(pageSize)}` : null
        );
        var queryString = queryParameters.filter(Boolean).join('&');
        if (queryString) {
            queryString = '?' + queryString;
        }
        return this.createRequest({
            httpVerb: "GET",
            apiURL: `/hotels/${this.hotelID}/offers${queryString}`
        });
    }
}