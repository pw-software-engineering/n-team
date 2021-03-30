using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.ViewsTagID.Client
{
    public static class AccountTagID
    {
        public static string InputClientNameID { get; set; } = "client-name";
        public static string InputClientSurnameID { get; set; } = "client-surname";
        public static string InputClientUsernameID { get; set; } = "client-username";
        public static string ClientUsernameValidationBoxID { get; set; } = "client-username-validation";
        public static string InputClientEmailID { get; set; } = "client-email";
        public static string ClientEmailValidationBoxID { get; set; } = "client-email-validation";
        public static string ApplyChangesBtnID { get; set; } = "apply-changes-btn";
        public static string ServerErrorBoxID { get; set; } = "server-error";
    }
}
