function HotelManager(apiConfig) {
    this.apiConfig = apiConfig;
}

HotelManager.prototype.getHotels = function ({
    hotelName = null,
    country = null,
    city = null,
    pageNumber = null,
    pageSize = null
} = {}) {
    var headers = {};
    headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
    var queryParameters = [];
    queryParameters.push(
        hotelName && hotelName.trim() ? `hotelName=${hotelName.trim()}` : null,
        country && country.trim() ? `country=${country.trim()}` : null,
        city && city.trim() ? `city=${city.trim()}` : null,
        pageNumber && Number(pageNumber) ? `pageNumber=${Number(pageNumber)}` : null,
        pageSize && Number(pageSize) ? `pageSize=${pageSize}` : null
    );
    var queryString = queryParameters.filter(Boolean).join('&');
    if (queryString) {
        queryString = '?' + queryString;
    }
    return $.ajax({
        method: "GET",
        url: `${this.apiConfig.apiBaseUrl}/hotels${queryString}`,
        headers: headers
    }).then(
        (data, textStatus, jqXHR) => {
            console.log(data);
            return data;
        },
        (jqXHR, textStatus, errorThrown) => {
            throw new Error(`GetHotels failed - ${textStatus}`);
        }
    );
}