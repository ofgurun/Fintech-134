namespace InteraktifKredi.Web.Models.Api
{
    /// <summary>
    /// Configuration settings for the external Azure Function APIs
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Base URL of the Customers API
        /// Example: https://customers-api.azurewebsites.net
        /// </summary>
        public string CustomersApi { get; set; } = string.Empty;

        /// <summary>
        /// Base URL of the IDC API (with /api/ path)
        /// Example: https://api-idc.azurewebsites.net/api/
        /// </summary>
        public string IdcApi { get; set; } = string.Empty;

        /// <summary>
        /// Azure Function authentication key for Customers API
        /// This will be appended as ?code={FunctionKey} to API requests
        /// </summary>
        public string FunctionKey { get; set; } = string.Empty;

        /// <summary>
        /// Bearer token for API authorization
        /// This will be added as Authorization: Bearer {BearerToken} header
        /// </summary>
        public string BearerToken { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for KVKK Text endpoint
        /// Endpoint: get-kvkk-text
        /// </summary>
        public string KvkkTextKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for KVKK Save endpoint
        /// Endpoint: save-kvkk-approval
        /// </summary>
        public string KvkkSaveKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for OTP Generate endpoint
        /// Endpoint: generate-otp
        /// </summary>
        public string OtpGenerateKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for OTP Send SMS endpoint
        /// Endpoint: send-otp-sms
        /// </summary>
        public string OtpSendKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for OTP Verify endpoint
        /// Endpoint: verify-otp
        /// </summary>
        public string OtpVerifyKey { get; set; } = string.Empty;

        // ========================================================================
        // Dashboard API Keys
        // ========================================================================

        /// <summary>
        /// Function Key for Dummy Report List endpoint
        /// Endpoint: api/dummy/report-list
        /// </summary>
        public string DummyReportKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Report Detail endpoint
        /// Endpoint: api/GetReportDetail
        /// </summary>
        public string ReportDetailKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Customer Address (GET) endpoint
        /// Endpoint: api/customer/address/{customerId}
        /// </summary>
        public string CustomerAddressKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Job Information (GET) endpoint
        /// Endpoint: api/customer/job-info/{customerId}
        /// </summary>
        public string JobInformationKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Wife Information (GET) endpoint
        /// Endpoint: api/customer/wife-info/{customerId}
        /// </summary>
        public string WifeInformationKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Customer Finance (GET) endpoint
        /// Endpoint: api/customer/finance-assets/{customerId}
        /// </summary>
        public string CustomerFinanceKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Save Customer Address (POST) endpoint
        /// Endpoint: api/customer/address
        /// </summary>
        public string SaveCustomerAddressKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Save Job Information (POST) endpoint
        /// Endpoint: api/customer/job-info
        /// </summary>
        public string SaveJobInformationKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Save Wife Information (POST) endpoint
        /// Endpoint: api/customer/wife-info/{customerId}
        /// </summary>
        public string SaveWifeInformationKey { get; set; } = string.Empty;

        /// <summary>
        /// Function Key for Save Customer Finance (POST) endpoint
        /// Endpoint: api/customer/finance-assets
        /// </summary>
        public string SaveCustomerFinanceKey { get; set; } = string.Empty;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int Timeout { get; set; } = 30;
    }
}

