using InteraktifKredi.Web.Models.Api;
using InteraktifKredi.Web.Models.Api.Auth;
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
                WriteIndented = false
            };
            
            // Configure JSON serialization options for RESPONSE
            // API returns camelCase (success, statusCode, message, value)
            _responseJsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Match API response
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
                    
                    // Try to deserialize with camelCase options
                    ApiResponse<VerifyUserResponse>? apiResponse = null;
                    try
                    {
                        apiResponse = JsonSerializer.Deserialize<ApiResponse<VerifyUserResponse>>(responseContent, _responseJsonOptions);
                        Console.WriteLine($"✅ Deserialization successful!");
                        Console.WriteLine($"  - Success     : {apiResponse?.Success}");
                        Console.WriteLine($"  - StatusCode  : {apiResponse?.StatusCode}");
                        Console.WriteLine($"  - Message     : {apiResponse?.Message}");
                        Console.WriteLine($"  - Value is null: {apiResponse?.Value == null}");
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
                            Console.WriteLine("═══════════════════════════════════════════════════════════════════");
                            Console.WriteLine("");
                            
                            _logger.LogInformation("User verified successfully. CustomerId: {CustomerId}", apiResponse.Value.CustomerId);
                            
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
    }
}
