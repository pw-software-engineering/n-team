class HotelOffersManager {
    constructor({
        hotelID = null,
        apiConfig = null
    } = {}) {
        this.hotelID = hotelID;
        this.apiConfig = apiConfig;
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
        var headers = {};
        headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
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
        return $.ajax({
            method: "GET",
            url: `${this.apiConfig.apiBaseUrl}/hotels/${this.hotelID}/offers${queryString}`,
            headers: headers
        }).then(
            (data, textStatus, jqXHR) => {
                console.log(data);
                for (var entry of data) {
                    entry.hotelID = this.hotelID;
                }
                return data;
            },
            (jqXHR, textStatus, errorThrown) => {
                throw new Error(`GetHotels failed - ${textStatus}`);
            }
        );
    }
}