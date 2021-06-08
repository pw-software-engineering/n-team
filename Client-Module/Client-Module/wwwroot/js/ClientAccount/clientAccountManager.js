class ClientAccountManager extends ServerApiManager {
    constructor(apiConfig) {
        super(apiConfig);
    }

    getClientInfo() {
        return this.createRequest({
            httpVerb: "GET",
            apiURL: "/client"
        });
    }

    updateClientInfo(updateInfo) {
        return this.createRequest({
            httpVerb: "PATCH",
            apiURL: "/client",
            additionalAjaxConfig: {
                contentType: "application/json",
                data: JSON.stringify(updateInfo)
            }
        });
    }
}