class ServerApiManager {
    constructor(apiConfig) {
        this.apiConfig = apiConfig;
    }

    createRequest({
        httpVerb = null,
        apiURL = null,
        additionalAjaxConfig = null
    } = {}) {
        var ajaxConfig = {};
        var headers = {};
        headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
        ajaxConfig.headers = headers;
        ajaxConfig.method = httpVerb;
        if (apiURL.length > 0 && apiURL[0] !== '/') {
            apiURL = '/' + apiURL;
        }
        ajaxConfig.url = `${this.apiConfig.apiBaseUrl}${apiURL}`;

        if (additionalAjaxConfig !== null) {
            for (var key of Object.keys(additionalAjaxConfig)) {
                ajaxConfig[key] = additionalAjaxConfig[key];
            }
        }
        return $.ajax(ajaxConfig).then(
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