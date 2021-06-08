using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.ViewsTagID.Client
{
    public class AccountTagID
    {
        public string InputClientNameID { get; set; } = "client-name";
        public string InputClientSurnameID { get; set; } = "client-surname";
        public string InputClientUsernameID { get; set; } = "client-username";
        public string ClientUsernameValidationBoxID { get; set; } = "client-username-validation";
        public string InputClientEmailID { get; set; } = "client-email";
        public string ClientEmailValidationBoxID { get; set; } = "client-email-validation";
        public string ApplyChangesBtnID { get; set; } = "apply-changes-btn";
        public string ServerErrorBoxID { get; set; } = "server-error";
        public string ServerSuccessBoxID { get; set; } = "server-success";
        public string LoadingDataDivID { get; } = "loading-data-div";
        public string MainContentDivID { get; } = "main-content-div";
        public string UpdatingClientInfoDivID { get; } = "updating-info-div";
    }
}
