using InteraktifKredi.Web.Models.Api;
using InteraktifKredi.Web.Models.Api.Auth;
using InteraktifKredi.Web.Models.Api.Dashboard;
using InteraktifKredi.Web.Models.Api.Reports;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace InteraktifKredi.Web.Services
{
    /// <summary>
    /// Implementation of API service for consuming external Azure Function REST API
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _requestJsonOptions;
        private readonly JsonSerializerOptions _responseJsonOptions;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="httpClient">HTTP client for making API requests</param>
        /// <param name="logger">Logger for tracking operations</param>
        /// <param name="apiSettings">API configuration settings (CustomersApi, IdcApi, DefaultToken)</param>
        public ApiService(HttpClient httpClient, ILogger<ApiService> logger, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiSettings = apiSettings.Value;
            
            // Configure JSON serialization options for REQUEST
            // API expects PascalCase for request body (TCKN, GSM)
            _requestJsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // Keep PascalCase for request
                PropertyNameCaseInsensitive = true, // KRİTİK: Büyük/küçük harf farkını yok sayar
                WriteIndented = false
            };
            
            // Configure JSON serialization options for RESPONSE
            // API'ler farklı formatlar dönüyor: Customers API (PascalCase), IDC API (camelCase)
            // PropertyNameCaseInsensitive ile her ikisini de deserialize edebiliriz
            _responseJsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Default camelCase
                PropertyNameCaseInsensitive = true, // KRİTİK: Büyük/küçük harf farkını yok sayar
                WriteIndented = false
            };
        }

        /// <summary>
        /// Verifies user credentials (TCKN and GSM) with the external Azure Function API
        /// </summary>
        public async Task<ServiceResponse<VerifyUserResponse>> VerifyUserAsync(VerifyUserRequest request)
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║              API REQUEST DEBUG - DETAILED LOGGING                 ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════════╝");
                Console.WriteLine("");
                
                _logger.LogInformation("Verifying user with TCKN: {TCKN}", request.TCKN);

                // Build URL with code parameter (Azure Function Key)
                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var endpoint = "api/customer/tckn-gsm";
                var requestUrl = $"{baseUrl}/{endpoint}?code={_apiSettings.FunctionKey}";

                // Detailed console logging
                Console.WriteLine($"→ Base URL         : {baseUrl}");
                Console.WriteLine($"→ Endpoint         : {endpoint}");
                Console.WriteLine($"→ Full URL         : {requestUrl}");
                Console.WriteLine($"→ Function Key (first 20) : {_apiSettings.FunctionKey?.Substring(0, 20)}...");
                Console.WriteLine($"→ Bearer Token (first 20) : {_apiSettings.BearerToken?.Substring(0, 20)}...");
                Console.WriteLine($"");
                Console.WriteLine($"→ Request Object:");
                Console.WriteLine($"  - TCKN: {request.TCKN}");
                Console.WriteLine($"  - GSM : {request.GSM}");
                
                // Serialize and show exact JSON being sent
                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                Console.WriteLine($"");
                Console.WriteLine($"→ JSON Payload (EXACT):");
                Console.WriteLine($"  {requestJson}");
                Console.WriteLine($"");
                Console.WriteLine($"→ JSON Options:");
                Console.WriteLine($"  - PropertyNamingPolicy: {(_requestJsonOptions.PropertyNamingPolicy == null ? "null (PascalCase)" : _requestJsonOptions.PropertyNamingPolicy.GetType().Name)}");
                Console.WriteLine($"");
                Console.WriteLine("───────────────────────────────────────────────────────────────────");
                Console.WriteLine("Sending request...");
                Console.WriteLine("");

                // Log request details
                _logger.LogInformation("API Request URL: {Url}", requestUrl);
                _logger.LogInformation("Request JSON: {Json}", requestJson);

                // IMPORTANT: Create HttpRequestMessage manually with StringContent
                // This ensures proper JSON encoding
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                
                // Add Bearer token to Authorization header (CRITICAL for CORS/Auth)
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");
                
                Console.WriteLine($"→ Authorization Header:");
                Console.WriteLine($"  Bearer {_apiSettings.BearerToken?.Substring(0, 20)}...");
                Console.WriteLine($"");

                // Send request
                var response = await _httpClient.SendAsync(httpRequest);

                Console.WriteLine($"← Response received!");
                Console.WriteLine($"← HTTP Status Code : {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"← Status Category  : {(response.IsSuccessStatusCode ? "SUCCESS" : "FAILURE")}");
                
                // Read raw response content
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"");
                Console.WriteLine($"← Response Headers:");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"  - {header.Key}: {string.Join(", ", header.Value)}");
                }
                Console.WriteLine($"");
                Console.WriteLine($"← Response Body (RAW):");
                Console.WriteLine($"  {responseContent}");
                Console.WriteLine($"");

                _logger.LogInformation("API Response Status: {StatusCode}", response.StatusCode);
                _logger.LogInformation("API Response Body: {Body}", responseContent);

                // Check HTTP response status
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Processing successful response...");
                    
                    // Customers API returns PascalCase JSON, so use _requestJsonOptions for deserialization
                    ApiResponse<VerifyUserResponse>? apiResponse = null;
                    try
                    {
                        // Use PascalCase deserializer (PropertyNamingPolicy = null)
                        apiResponse = JsonSerializer.Deserialize<ApiResponse<VerifyUserResponse>>(responseContent, _requestJsonOptions);
                        Console.WriteLine($"✅ Deserialization successful!");
                        Console.WriteLine($"  - Success     : {apiResponse?.Success}");
                        Console.WriteLine($"  - StatusCode  : {apiResponse?.StatusCode}");
                        Console.WriteLine($"  - Message     : {apiResponse?.Message}");
                        Console.WriteLine($"  - Value is null: {apiResponse?.Value == null}");
                        
                        if (apiResponse?.Value != null)
                        {
                            Console.WriteLine($"  - CustomerId  : {apiResponse.Value.CustomerId}");
                            Console.WriteLine($"  - TCKN        : {apiResponse.Value.TCKN}");
                            Console.WriteLine($"  - GSM         : {apiResponse.Value.GSM}");
                            Console.WriteLine($"  - IsNewUser   : {apiResponse.Value.IsNewUser}");
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"❌ JSON Deserialization Error: {jsonEx.Message}");
                        _logger.LogError(jsonEx, "JSON deserialization failed");
                        throw;
                    }

                    if (apiResponse != null)
                    {
                        _logger.LogInformation("API Success: {Success}, Message: {Message}", apiResponse.Success, apiResponse.Message);

                        if (apiResponse.Success && apiResponse.Value != null)
                        {
                            Console.WriteLine($"✅ USER VERIFIED SUCCESSFULLY!");
                            Console.WriteLine($"  - CustomerId: {apiResponse.Value.CustomerId}");
                            Console.WriteLine($"  - TCKN: {apiResponse.Value.TCKN}");
                            Console.WriteLine($"  - GSM: {apiResponse.Value.GSM}");
                            Console.WriteLine($"  - IsNewUser: {apiResponse.Value.IsNewUser}");
                            Console.WriteLine("═══════════════════════════════════════════════════════════════════");
                            Console.WriteLine("");
                            
                            _logger.LogInformation("User verified successfully. CustomerId: {CustomerId}, TCKN: {TCKN}, GSM: {GSM}", 
                                apiResponse.Value.CustomerId, apiResponse.Value.TCKN, apiResponse.Value.GSM);
                            
                            return ServiceResponse<VerifyUserResponse>.SuccessResponse(
                                apiResponse.Value,
                                apiResponse.Message,
                                apiResponse.StatusCode
                            );
                        }
                        else
                        {
                            var errorMsg = apiResponse.Message ?? "Kullanıcı doğrulanamadı.";
                            Console.WriteLine($"⚠️  API returned unsuccessful: {errorMsg}");
                            Console.WriteLine("═══════════════════════════════════════════════════════════════════");
                            Console.WriteLine("");
                            
                            _logger.LogWarning("API returned unsuccessful: {Message}", errorMsg);
                            
                            return ServiceResponse<VerifyUserResponse>.FailureResponse(
                                errorMsg,
                                apiResponse.StatusCode
                            );
                        }
                    }
                    else
                    {
                        Console.WriteLine($"❌ Failed to deserialize - apiResponse is null");
                        Console.WriteLine("═══════════════════════════════════════════════════════════════════");
                        Console.WriteLine("");
                        
                        _logger.LogError("Failed to deserialize API response");
                        return ServiceResponse<VerifyUserResponse>.FailureResponse(
                            "API yanıtı işlenirken bir hata oluştu.",
                            (int)response.StatusCode
                        );
                    }
                }
                else
                {
                    Console.WriteLine($"❌ HTTP ERROR: {response.StatusCode}");
                    Console.WriteLine($"");
                    Console.WriteLine($"Error Details:");
                    Console.WriteLine($"  Status Code: {(int)response.StatusCode}");
                    Console.WriteLine($"  Reason Phrase: {response.ReasonPhrase}");
                    Console.WriteLine($"  Response Body: {responseContent}");
                    Console.WriteLine("═══════════════════════════════════════════════════════════════════");
                    Console.WriteLine("");
                    
                    _logger.LogError("API request failed. Status: {StatusCode}, Error: {Error}", 
                        response.StatusCode, responseContent);

                    var errorMessage = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.Unauthorized => "Yetkilendirme hatası. Lütfen sistem yöneticinizle iletişime geçin.",
                        System.Net.HttpStatusCode.NotFound => "Kullanıcı bulunamadı. Lütfen TCKN ve GSM bilgilerinizi kontrol edin.",
                        System.Net.HttpStatusCode.BadRequest => "Geçersiz istek. Lütfen girdiğiniz bilgileri kontrol edin.",
                        System.Net.HttpStatusCode.InternalServerError => $"Sunucu hatası (500). Detay: {responseContent}",
                        _ => "Kullanıcı doğrulama başarısız. Lütfen daha sonra tekrar deneyin."
                    };

                    return ServiceResponse<VerifyUserResponse>.FailureResponse(
                        errorMessage,
                        (int)response.StatusCode
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"");
                Console.WriteLine($"❌ HTTP REQUEST EXCEPTION: {ex.Message}");
                Console.WriteLine($"═══════════════════════════════════════════════════════════════════");
                Console.WriteLine("");
                
                _logger.LogError(ex, "HTTP request error during user verification");
                return ServiceResponse<VerifyUserResponse>.FailureResponse(
                    "API ile bağlantı kurulamadı. Lütfen internet bağlantınızı kontrol edin.",
                    500
                );
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"");
                Console.WriteLine($"❌ JSON EXCEPTION: {ex.Message}");
                Console.WriteLine($"═══════════════════════════════════════════════════════════════════");
                Console.WriteLine("");
                
                _logger.LogError(ex, "JSON deserialization error");
                return ServiceResponse<VerifyUserResponse>.FailureResponse(
                    "API yanıtı işlenirken bir hata oluştu.",
                    500
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"");
                Console.WriteLine($"❌ UNEXPECTED EXCEPTION: {ex.GetType().Name}");
                Console.WriteLine($"   Message: {ex.Message}");
                Console.WriteLine($"   Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"═══════════════════════════════════════════════════════════════════");
                Console.WriteLine("");
                
                _logger.LogError(ex, "Unexpected error during user verification");
                return ServiceResponse<VerifyUserResponse>.FailureResponse(
                    "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                    500
                );
            }
        }

        /// <summary>
        /// Generates OTP code for user authentication
        /// </summary>
        public async Task<ServiceResponse<GenerateOtpResponse>> GenerateOtpAsync(string tckn, string gsm)
        {
            try
            {
                _logger.LogInformation("Generating OTP for TCKN: {TCKN}, GSM: {GSM}", tckn, gsm);

                var request = new GenerateOtpRequest
                {
                    TCKN = tckn,
                    GSM = gsm,
                    UtmId = "5"
                };

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var endpoint = "generate-otp";
                var requestUrl = $"{baseUrl}/{endpoint}?code={_apiSettings.OtpGenerateKey}";

                _logger.LogInformation("OTP Generate URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                
                // DETAILED LOGGING
                Console.WriteLine("");
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║           OTP GENERATE REQUEST - DETAILED LOGGING                 ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════════╝");
                Console.WriteLine($"→ Request URL: {requestUrl}");
                Console.WriteLine($"→ Request Object:");
                Console.WriteLine($"  - TCKN  : {request.TCKN}");
                Console.WriteLine($"  - GSM   : {request.GSM}");
                Console.WriteLine($"  - UtmId : {request.UtmId}");
                Console.WriteLine($"→ JSON Payload (EXACT):");
                Console.WriteLine($"  {requestJson}");
                Console.WriteLine($"→ Bearer Token (first 20): {_apiSettings.BearerToken.Substring(0, 20)}...");
                Console.WriteLine("───────────────────────────────────────────────────────────────────");
                _logger.LogInformation("OTP Generate Request JSON: {Json}", requestJson);
                
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"← Response Status: {response.StatusCode}");
                Console.WriteLine($"← Response Body: {responseContent}");
                Console.WriteLine("═══════════════════════════════════════════════════════════════════");
                Console.WriteLine("");

                _logger.LogInformation("OTP Generate Response: {StatusCode}, Body: {Body}", 
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<GenerateOtpResponse>>(
                        responseContent, _responseJsonOptions);

                    if (apiResponse != null && apiResponse.Success && apiResponse.Value != null)
                    {
                        _logger.LogInformation("OTP generated successfully");
                        return ServiceResponse<GenerateOtpResponse>.SuccessResponse(
                            apiResponse.Value,
                            apiResponse.Message,
                            apiResponse.StatusCode
                        );
                    }
                    else
                    {
                        var errorMsg = apiResponse?.Message ?? "OTP oluşturulamadı.";
                        _logger.LogWarning("OTP generation failed: {Message}", errorMsg);
                        return ServiceResponse<GenerateOtpResponse>.FailureResponse(errorMsg, apiResponse?.StatusCode ?? 500);
                    }
                }
                else
                {
                    _logger.LogError("OTP Generate API error: {StatusCode}, {Body}", 
                        response.StatusCode, responseContent);
                    return ServiceResponse<GenerateOtpResponse>.FailureResponse(
                        "OTP oluşturulurken bir hata oluştu.",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating OTP");
                return ServiceResponse<GenerateOtpResponse>.FailureResponse(
                    "OTP oluşturulurken beklenmeyen bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Sends OTP code via SMS
        /// </summary>
        public async Task<ServiceResponse<SendOtpSmsResponse>> SendOtpSmsAsync(string gsm, string otpCode)
        {
            try
            {
                _logger.LogInformation("Sending OTP SMS to GSM: {GSM}", gsm);

                var request = new SendOtpSmsRequest
                {
                    GSM = gsm,
                    OTPCode = otpCode
                };

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var endpoint = "send-otp-sms";
                var requestUrl = $"{baseUrl}/{endpoint}?code={_apiSettings.OtpSendKey}";

                _logger.LogInformation("OTP Send SMS URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("OTP Send SMS Response: {StatusCode}, Body: {Body}", 
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    // API sometimes returns plain text instead of JSON
                    // If response is "SMS başarıyla gönderildi." or similar, treat as success
                    if (!string.IsNullOrWhiteSpace(responseContent) && 
                        (responseContent.Contains("başarıyla") || responseContent.Contains("success")))
                    {
                        _logger.LogInformation("OTP SMS sent successfully (plain text response)");
                        return ServiceResponse<SendOtpSmsResponse>.SuccessResponse(
                            new SendOtpSmsResponse { Sent = true },
                            "SMS başarıyla gönderildi.",
                            200
                        );
                    }
                    
                    // Try to parse as JSON (for future compatibility)
                    try
                    {
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse<SendOtpSmsResponse>>(
                            responseContent, _responseJsonOptions);

                        if (apiResponse != null && apiResponse.Success && apiResponse.Value != null)
                        {
                            _logger.LogInformation("OTP SMS sent successfully (JSON response)");
                            return ServiceResponse<SendOtpSmsResponse>.SuccessResponse(
                                apiResponse.Value,
                                apiResponse.Message,
                                apiResponse.StatusCode
                            );
                        }
                    }
                    catch (JsonException)
                    {
                        // Not JSON, but 200 OK - treat as success
                        _logger.LogInformation("OTP SMS sent successfully (non-JSON response)");
                        return ServiceResponse<SendOtpSmsResponse>.SuccessResponse(
                            new SendOtpSmsResponse { Sent = true },
                            responseContent,
                            200
                        );
                    }
                    
                    // If we get here, something unexpected happened
                    _logger.LogWarning("SMS sending response unclear: {Response}", responseContent);
                    return ServiceResponse<SendOtpSmsResponse>.FailureResponse("SMS durumu belirsiz.", 500);
                }
                else
                {
                    _logger.LogError("OTP Send SMS API error: {StatusCode}, {Body}", 
                        response.StatusCode, responseContent);
                    return ServiceResponse<SendOtpSmsResponse>.FailureResponse(
                        "SMS gönderilirken bir hata oluştu.",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP SMS");
                return ServiceResponse<SendOtpSmsResponse>.FailureResponse(
                    "SMS gönderilirken beklenmeyen bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Verifies OTP code entered by user
        /// </summary>
        public async Task<ServiceResponse<VerifyOtpResponse>> VerifyOtpAsync(string otpCode)
        {
            try
            {
                _logger.LogInformation("Verifying OTP code");

                var request = new VerifyOtpRequest
                {
                    OTPCode = otpCode
                };

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var endpoint = "verify-otp";
                var requestUrl = $"{baseUrl}/{endpoint}?code={_apiSettings.OtpVerifyKey}";

                _logger.LogInformation("OTP Verify URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("OTP Verify Response: {StatusCode}, Body: {Body}", 
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<VerifyOtpResponse>>(
                        responseContent, _responseJsonOptions);

                    if (apiResponse != null && apiResponse.Success && apiResponse.Value != null)
                    {
                        _logger.LogInformation("OTP verified successfully. Token received.");
                        return ServiceResponse<VerifyOtpResponse>.SuccessResponse(
                            apiResponse.Value,
                            apiResponse.Message,
                            apiResponse.StatusCode
                        );
                    }
                    else
                    {
                        var errorMsg = apiResponse?.Message ?? "OTP doğrulanamadı.";
                        _logger.LogWarning("OTP verification failed: {Message}", errorMsg);
                        return ServiceResponse<VerifyOtpResponse>.FailureResponse(errorMsg, apiResponse?.StatusCode ?? 500);
                    }
                }
                else
                {
                    _logger.LogError("OTP Verify API error: {StatusCode}, {Body}", 
                        response.StatusCode, responseContent);
                    return ServiceResponse<VerifyOtpResponse>.FailureResponse(
                        "OTP doğrulanırken bir hata oluştu.",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying OTP");
                return ServiceResponse<VerifyOtpResponse>.FailureResponse(
                    "OTP doğrulanırken beklenmeyen bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Retrieves KVKK text by ID
        /// </summary>
        public async Task<ServiceResponse<KvkkTextResponse>> GetKvkkTextAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching KVKK text for ID: {KvkkId}", id);

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var endpoint = $"kvkk/text/{id}";
                var requestUrl = $"{baseUrl}/{endpoint}?code={_apiSettings.KvkkTextKey}";

                _logger.LogInformation("KVKK Text URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("KVKK Text Response: {StatusCode}, Body: {Body}", 
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    // KVKK API does NOT use ApiResponse wrapper, returns direct JSON
                    var kvkkData = JsonSerializer.Deserialize<KvkkTextResponse>(
                        responseContent, _responseJsonOptions);

                    if (kvkkData != null && !string.IsNullOrWhiteSpace(kvkkData.Text))
                    {
                        _logger.LogInformation("KVKK text retrieved successfully - Id: {Id}, Title length: {TitleLen}, Text length: {TextLen}", 
                            kvkkData.Id, kvkkData.Title?.Length ?? 0, kvkkData.Text?.Length ?? 0);
                        
                        return ServiceResponse<KvkkTextResponse>.SuccessResponse(
                            kvkkData,
                            "KVKK metni başarıyla alındı.",
                            (int)response.StatusCode
                        );
                    }
                    else
                    {
                        _logger.LogWarning("KVKK text retrieval failed: Empty or null data");
                        return ServiceResponse<KvkkTextResponse>.FailureResponse(
                            "KVKK metni boş veya geçersiz.", 
                            (int)response.StatusCode
                        );
                    }
                }
                else
                {
                    _logger.LogError("KVKK Text API error: {StatusCode}, {Body}", 
                        response.StatusCode, responseContent);
                    return ServiceResponse<KvkkTextResponse>.FailureResponse(
                        "KVKK metni alınırken bir hata oluştu.",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching KVKK text");
                return ServiceResponse<KvkkTextResponse>.FailureResponse(
                    "KVKK metni alınırken beklenmeyen bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Saves KVKK approval for a customer
        /// </summary>
        public async Task<ServiceResponse<bool>> SaveKvkkApprovalAsync(KvkkApprovalRequest request)
        {
            try
            {
                _logger.LogInformation("Saving KVKK approval for CustomerId: {CustomerId}, KvkkId: {KvkkId}, IsOk: {IsOk}", 
                    request.CustomerId, request.KvkkId, request.IsOk);

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var endpoint = "kvkk/onay";
                var requestUrl = $"{baseUrl}/{endpoint}?code={_apiSettings.KvkkSaveKey}";

                _logger.LogInformation("KVKK Save URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                _logger.LogInformation("KVKK Save Request JSON: {Json}", requestJson);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("KVKK Save Response: {StatusCode}, Body: {Body}", 
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    // Try to parse as direct KvkkApprovalResponse (API doesn't wrap in ApiResponse)
                    try
                    {
                        var kvkkResponse = JsonSerializer.Deserialize<KvkkApprovalResponse>(
                            responseContent, _responseJsonOptions);

                        if (kvkkResponse != null && kvkkResponse.Id > 0)
                        {
                            _logger.LogInformation("✅ KVKK approval saved successfully - ID: {Id}, Message: {Message}", 
                                kvkkResponse.Id, kvkkResponse.Message);
                            return ServiceResponse<bool>.SuccessResponse(
                                true,
                                kvkkResponse.Message ?? "KVKK onayı başarıyla kaydedildi.",
                                200
                            );
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogWarning(jsonEx, "Failed to parse KVKK save response as JSON, trying plain text");
                    }

                    // If JSON parsing fails, check for success keywords in plain text
                    if (!string.IsNullOrWhiteSpace(responseContent) && 
                        (responseContent.Contains("başarılı") || responseContent.Contains("success") || 
                         responseContent.Contains("oluşturuldu") || responseContent.ToLower().Contains("ok")))
                    {
                        _logger.LogInformation("KVKK approval saved successfully (plain text response)");
                        return ServiceResponse<bool>.SuccessResponse(
                            true,
                            "KVKK onayı başarıyla kaydedildi.",
                            200
                        );
                    }

                    // If we get here, HTTP 200 but couldn't parse response
                    _logger.LogWarning("KVKK save response unclear: {Response}", responseContent);
                    return ServiceResponse<bool>.FailureResponse("KVKK onayı durumu belirsiz.", 500);
                }
                else
                {
                    _logger.LogError("KVKK Save API error: {StatusCode}, {Body}", 
                        response.StatusCode, responseContent);
                    return ServiceResponse<bool>.FailureResponse(
                        "KVKK onayı kaydedilirken bir hata oluştu.",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving KVKK approval");
                return ServiceResponse<bool>.FailureResponse(
                    "KVKK onayı kaydedilirken beklenmeyen bir hata oluştu.",
                    500
                );
            }
        }

        // ========================================================================
        // DASHBOARD - REPORTS
        // ========================================================================

        /// <summary>
        /// Retrieves dummy report list
        /// </summary>
        public async Task<ServiceResponse<List<Models.Api.Reports.ReportSummary>>> GetReportListAsync()
        {
            try
            {
                _logger.LogInformation("Fetching report list");

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/dummy/report-list?code={_apiSettings.DummyReportKey}";

                _logger.LogInformation("Report List URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Report List Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    // API returns ApiResponse wrapper: { "success": true, "value": [...] }
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Models.Api.Reports.ReportSummary>>>(
                        responseContent, _responseJsonOptions);

                    if (apiResponse?.Success == true && apiResponse.Value != null)
                    {
                        _logger.LogInformation("✅ Report list retrieved successfully - Count: {Count}", apiResponse.Value.Count);
                        return ServiceResponse<List<Models.Api.Reports.ReportSummary>>.SuccessResponse(
                            apiResponse.Value,
                            apiResponse.Message ?? "Raporlar başarıyla getirildi.",
                            apiResponse.StatusCode
                        );
                    }
                }

                _logger.LogError("Failed to fetch report list: {StatusCode}", response.StatusCode);
                return ServiceResponse<List<Models.Api.Reports.ReportSummary>>.FailureResponse(
                    "Raporlar getirilemedi.",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching report list");
                return ServiceResponse<List<Models.Api.Reports.ReportSummary>>.FailureResponse(
                    "Raporlar getirilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Retrieves detailed report by ID
        /// </summary>
        public async Task<ServiceResponse<Models.Api.Reports.ReportDetail>> GetReportDetailAsync(int reportId)
        {
            try
            {
                _logger.LogInformation("Fetching report detail for ID: {ReportId}", reportId);

                var baseUrl = _apiSettings.IdcApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/GetReportDetail?code={_apiSettings.ReportDetailKey}&id={reportId}";

                _logger.LogInformation("Report Detail URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Report Detail Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var reportDetail = JsonSerializer.Deserialize<Models.Api.Reports.ReportDetail>(
                        responseContent, _responseJsonOptions);

                    if (reportDetail != null)
                    {
                        _logger.LogInformation("✅ Report detail retrieved successfully");
                        return ServiceResponse<Models.Api.Reports.ReportDetail>.SuccessResponse(
                            reportDetail,
                            "Rapor detayı başarıyla getirildi.",
                            200
                        );
                    }
                }

                _logger.LogError("Failed to fetch report detail: {StatusCode}", response.StatusCode);
                return ServiceResponse<Models.Api.Reports.ReportDetail>.FailureResponse(
                    "Rapor detayı getirilemedi.",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching report detail");
                return ServiceResponse<Models.Api.Reports.ReportDetail>.FailureResponse(
                    "Rapor detayı getirilirken bir hata oluştu.",
                    500
                );
            }
        }

        // ========================================================================
        // DASHBOARD - CUSTOMER PROFILE (GET)
        // ========================================================================

        /// <summary>
        /// Retrieves customer address information
        /// </summary>
        public async Task<ServiceResponse<AddressResponse>> GetCustomerAddressAsync(long customerId)
        {
            try
            {
                _logger.LogInformation("Fetching customer address for ID: {CustomerId}", customerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/address/{customerId}?code={_apiSettings.CustomerAddressKey}";

                _logger.LogInformation("Customer Address URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Customer Address Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var address = JsonSerializer.Deserialize<AddressResponse>(
                        responseContent, _responseJsonOptions);

                    if (address != null)
                    {
                        _logger.LogInformation("✅ Customer address retrieved successfully");
                        return ServiceResponse<AddressResponse>.SuccessResponse(
                            address,
                            "Adres bilgisi başarıyla getirildi.",
                            200
                        );
                    }
                }

                _logger.LogError("Failed to fetch customer address: {StatusCode}", response.StatusCode);
                return ServiceResponse<AddressResponse>.FailureResponse(
                    "Adres bilgisi getirilemedi.",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer address");
                return ServiceResponse<AddressResponse>.FailureResponse(
                    "Adres bilgisi getirilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Retrieves customer job information
        /// </summary>
        public async Task<ServiceResponse<JobResponse>> GetJobInfoAsync(long customerId)
        {
            try
            {
                _logger.LogInformation("Fetching job info for customer ID: {CustomerId}", customerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/job-info/{customerId}?code={_apiSettings.JobInformationKey}";

                _logger.LogInformation("Job Info URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Job Info Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var jobInfo = JsonSerializer.Deserialize<JobResponse>(
                        responseContent, _responseJsonOptions);

                    if (jobInfo != null)
                    {
                        _logger.LogInformation("✅ Job info retrieved successfully");
                        return ServiceResponse<JobResponse>.SuccessResponse(
                            jobInfo,
                            "İş bilgisi başarıyla getirildi.",
                            200
                        );
                    }
                }

                _logger.LogError("Failed to fetch job info: {StatusCode}", response.StatusCode);
                return ServiceResponse<JobResponse>.FailureResponse(
                    "İş bilgisi getirilemedi.",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching job info");
                return ServiceResponse<JobResponse>.FailureResponse(
                    "İş bilgisi getirilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Retrieves wife/spouse information
        /// </summary>
        public async Task<ServiceResponse<WifeInfoResponse>> GetWifeInfoAsync(long customerId)
        {
            try
            {
                _logger.LogInformation("Fetching wife info for customer ID: {CustomerId}", customerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/wife-info/{customerId}?code={_apiSettings.WifeInformationKey}";

                _logger.LogInformation("Wife Info URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Wife Info Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var wifeInfo = JsonSerializer.Deserialize<WifeInfoResponse>(
                        responseContent, _responseJsonOptions);

                    if (wifeInfo != null)
                    {
                        _logger.LogInformation("✅ Wife info retrieved successfully");
                        return ServiceResponse<WifeInfoResponse>.SuccessResponse(
                            wifeInfo,
                            "Eş bilgisi başarıyla getirildi.",
                            200
                        );
                    }
                }

                _logger.LogError("Failed to fetch wife info: {StatusCode}", response.StatusCode);
                return ServiceResponse<WifeInfoResponse>.FailureResponse(
                    "Eş bilgisi getirilemedi.",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching wife info");
                return ServiceResponse<WifeInfoResponse>.FailureResponse(
                    "Eş bilgisi getirilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Retrieves customer finance and assets information
        /// </summary>
        public async Task<ServiceResponse<FinanceResponse>> GetFinanceInfoAsync(long customerId)
        {
            try
            {
                _logger.LogInformation("Fetching finance info for customer ID: {CustomerId}", customerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/finance-assets/{customerId}?code={_apiSettings.CustomerFinanceKey}";

                _logger.LogInformation("Finance Info URL: {Url}", requestUrl);

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Finance Info Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var financeInfo = JsonSerializer.Deserialize<FinanceResponse>(
                        responseContent, _responseJsonOptions);

                    if (financeInfo != null)
                    {
                        _logger.LogInformation("✅ Finance info retrieved successfully");
                        return ServiceResponse<FinanceResponse>.SuccessResponse(
                            financeInfo,
                            "Finans bilgisi başarıyla getirildi.",
                            200
                        );
                    }
                }

                _logger.LogError("Failed to fetch finance info: {StatusCode}", response.StatusCode);
                return ServiceResponse<FinanceResponse>.FailureResponse(
                    "Finans bilgisi getirilemedi.",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching finance info");
                return ServiceResponse<FinanceResponse>.FailureResponse(
                    "Finans bilgisi getirilirken bir hata oluştu.",
                    500
                );
            }
        }

        // ========================================================================
        // DASHBOARD - CUSTOMER PROFILE (POST/Save)
        // ========================================================================

        /// <summary>
        /// Saves customer address information
        /// </summary>
        public async Task<ServiceResponse<bool>> SaveCustomerAddressAsync(SaveAddressRequest request)
        {
            try
            {
                _logger.LogInformation("Saving customer address for ID: {CustomerId}", request.CustomerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/address?code={_apiSettings.SaveCustomerAddressKey}";

                _logger.LogInformation("Save Customer Address URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                _logger.LogInformation("Save Customer Address Request JSON: {Json}", requestJson);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Save Customer Address Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Customer address saved successfully");
                    return ServiceResponse<bool>.SuccessResponse(
                        true,
                        "Adres bilgisi başarıyla kaydedildi.",
                        200
                    );
                }

                _logger.LogError("Failed to save customer address: {StatusCode}", response.StatusCode);
                return ServiceResponse<bool>.FailureResponse(
                    $"Adres bilgisi kaydedilemedi. Hata: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving customer address");
                return ServiceResponse<bool>.FailureResponse(
                    "Adres bilgisi kaydedilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Saves customer job information
        /// </summary>
        public async Task<ServiceResponse<bool>> SaveJobInfoAsync(SaveJobRequest request)
        {
            try
            {
                _logger.LogInformation("Saving job info for customer ID: {CustomerId}", request.CustomerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/job-info?code={_apiSettings.SaveJobInformationKey}";

                _logger.LogInformation("Save Job Info URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                _logger.LogInformation("Save Job Info Request JSON: {Json}", requestJson);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Save Job Info Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Job info saved successfully");
                    return ServiceResponse<bool>.SuccessResponse(
                        true,
                        "İş bilgisi başarıyla kaydedildi.",
                        200
                    );
                }

                _logger.LogError("Failed to save job info: {StatusCode}", response.StatusCode);
                return ServiceResponse<bool>.FailureResponse(
                    $"İş bilgisi kaydedilemedi. Hata: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving job info");
                return ServiceResponse<bool>.FailureResponse(
                    "İş bilgisi kaydedilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Saves wife/spouse information
        /// </summary>
        public async Task<ServiceResponse<bool>> SaveWifeInfoAsync(long customerId, SaveWifeInfoRequest request)
        {
            try
            {
                _logger.LogInformation("Saving wife info for customer ID: {CustomerId}", customerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/wife-info/{customerId}?code={_apiSettings.SaveWifeInformationKey}";

                _logger.LogInformation("Save Wife Info URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                _logger.LogInformation("Save Wife Info Request JSON: {Json}", requestJson);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Save Wife Info Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Wife info saved successfully");
                    return ServiceResponse<bool>.SuccessResponse(
                        true,
                        "Eş bilgisi başarıyla kaydedildi.",
                        200
                    );
                }

                _logger.LogError("Failed to save wife info: {StatusCode}", response.StatusCode);
                return ServiceResponse<bool>.FailureResponse(
                    $"Eş bilgisi kaydedilemedi. Hata: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving wife info");
                return ServiceResponse<bool>.FailureResponse(
                    "Eş bilgisi kaydedilirken bir hata oluştu.",
                    500
                );
            }
        }

        /// <summary>
        /// Saves customer finance and assets information
        /// </summary>
        public async Task<ServiceResponse<bool>> SaveFinanceInfoAsync(SaveFinanceRequest request)
        {
            try
            {
                _logger.LogInformation("Saving finance info for customer ID: {CustomerId}", request.CustomerId);

                var baseUrl = _apiSettings.CustomersApi.TrimEnd('/');
                var requestUrl = $"{baseUrl}/api/customer/finance-assets?code={_apiSettings.SaveCustomerFinanceKey}";

                _logger.LogInformation("Save Finance Info URL: {Url}", requestUrl);

                var requestJson = JsonSerializer.Serialize(request, _requestJsonOptions);
                _logger.LogInformation("Save Finance Info Request JSON: {Json}", requestJson);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                httpRequest.Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                httpRequest.Headers.Add("Authorization", $"Bearer {_apiSettings.BearerToken}");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Save Finance Info Response: {StatusCode}, Body: {Body}",
                    response.StatusCode, responseContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Finance info saved successfully");
                    return ServiceResponse<bool>.SuccessResponse(
                        true,
                        "Finans bilgisi başarıyla kaydedildi.",
                        200
                    );
                }

                _logger.LogError("Failed to save finance info: {StatusCode}", response.StatusCode);
                return ServiceResponse<bool>.FailureResponse(
                    $"Finans bilgisi kaydedilemedi. Hata: {responseContent}",
                    (int)response.StatusCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving finance info");
                return ServiceResponse<bool>.FailureResponse(
                    "Finans bilgisi kaydedilirken bir hata oluştu.",
                    500
                );
            }
        }
    }
}
