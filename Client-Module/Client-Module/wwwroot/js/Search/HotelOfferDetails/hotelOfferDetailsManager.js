class HotelOfferDetailsManager {
    constructor({
        hotelID = null,
        offerID = null,
        apiConfig = null
    } = {}) {
        this.hotelID = hotelID;
        this.offerID = offerID;
        this.apiConfig = apiConfig;
    }

    getHotelOfferDetails() {
        var headers = {};
        headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
        return $.ajax({
            method: "GET",
            url: `${this.apiConfig.apiBaseUrl}/hotels/${this.hotelID}/offers/${this.offerID}`,
            headers: headers
        }).then(
            (data, textStatus, jqXHR) => {
                console.log(data);
                return data;
            },
            (jqXHR, textStatus, errorThrown) => {
                var errorInfo = `Error has occured - ${textStatus} (status code: ${jqXHR.status})`;
                if (jqXHR.responseJSON && "error" in jqXHR.responseJSON) {
                    errorInfo += `: ${jqXHR.responseJSON.error}`;
                }
                throw new Error(errorInfo);
            }
        );
    }
}