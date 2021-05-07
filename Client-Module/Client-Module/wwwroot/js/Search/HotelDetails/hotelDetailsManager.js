class HotelDetailsManager {
    constructor(apiConfig) {
        this.apiConfig = apiConfig;
    }

    getHotelDetails(hotelID) {
        var headers = {};
        headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
        return $.ajax({
            method: "GET",
            url: `${this.apiConfig.apiBaseUrl}/hotels/${hotelID}`,
            headers: headers,
        }).then(
            (data, textStatus, jqXHR) => {
                console.log(data);
                return data;
            },
            (jqXHR, textStatus, errorThrown) => {
                var errorDesc = "";
                if ("error" in jqXHR.responseJSON) {
                    errorDesc = ` - ${jqXHR.responseJSON.error}`;
                }
                throw new Error(`Request failed. Reason: ${textStatus}${errorDesc}`);
            }
        );
    }
}