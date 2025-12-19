using InteraktifKredi.Web.Models.Api;
using InteraktifKredi.Web.Models.Api.Auth;
using System.Net.Http.Json;
using System.Text.Json;

namespace InteraktifKredi.Web.Services
{
    /// <summary>
    /// Implementation of API service for consuming external REST API
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="httpClient">HTTP client for making API requests</param>
        /// <param name="logger">Logger for tracking operations</param>
        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            // Configure JSON serialization options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        /// <summary>
        /// Verifies user credentials (TCKN and GSM) with the external API
        /// </summary>
        public async Task<ServiceResponse<VerifyUserResponse>> VerifyUserAsync(VerifyUserRequest request)
        {
            try
            {
                _logger.LogInformation("Verifying user with TCKN: {TCKN}", request.TCKN);

                // Send POST request to the API endpoint
                var response = await _httpClient.PostAsJsonAsync("/api/ep/tckn-gsm", request, _jsonOptions);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<VerifyUserResponse>(_jsonOptions);

                    if (result != null)
                    {
                        _logger.LogInformation("User verification successful for CustomerId: {CustomerId}", result.CustomerId);
                        
                        return ServiceResponse<VerifyUserResponse>.SuccessResponse(
                            result,
                            "Kullanıcı doğrulandı.",
                            (int)response.StatusCode
                        );
                    }
                    else
                    {
                        _logger.LogWarning("User verification returned null result");
                        
                        return ServiceResponse<VerifyUserResponse>.FailureResponse(
                            "API'den geçersiz yanıt alındı.",
                            (int)response.StatusCode
                        );
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("User verification failed with status {StatusCode}: {Error}", 
                        response.StatusCode, errorContent);

                    return ServiceResponse<VerifyUserResponse>.FailureResponse(
                        $"Kullanıcı doğrulama başarısız: {response.ReasonPhrase}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error during user verification");
                
                return ServiceResponse<VerifyUserResponse>.FailureResponse(
                    "API ile bağlantı kurulamadı. Lütfen internet bağlantınızı kontrol edin.",
                    500
                );
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error during user verification");
                
                return ServiceResponse<VerifyUserResponse>.FailureResponse(
                    "API yanıtı işlenirken bir hata oluştu.",
                    500
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user verification");
                
                return ServiceResponse<VerifyUserResponse>.FailureResponse(
                    "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                    500
                );
            }
        }
    }
}

