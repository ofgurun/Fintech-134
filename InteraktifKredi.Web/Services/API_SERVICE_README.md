# API Servis Katmanı - İnteraktif Kredi

## ✅ Tamamlanan Yapı

### 📁 Dosya Yapısı

```
InteraktifKredi.Web/
├── Models/
│   └── Api/
│       ├── ApiSettings.cs              ✅ API Yapılandırması
│       ├── ServiceResponse.cs          ✅ Generic Response Wrapper
│       └── Auth/
│           ├── VerifyUserRequest.cs    ✅ Request Model
│           └── VerifyUserResponse.cs   ✅ Response Model
├── Services/
│   ├── IApiService.cs                  ✅ Interface
│   └── ApiService.cs                   ✅ Implementation (GÜNCELLENDİ)
├── Program.cs                          ✅ DI Yapılandırması
└── appsettings.json                    ✅ API Ayarları
```

---

## 1️⃣ Modeller

### 📄 `VerifyUserRequest.cs`
```csharp
public class VerifyUserRequest
{
    [Required(ErrorMessage = "TCKN zorunludur.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TCKN 11 karakter olmalıdır.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "TCKN sadece rakamlardan oluşmalıdır.")]
    public string TCKN { get; set; } = string.Empty;

    [Required(ErrorMessage = "GSM numarası zorunludur.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [RegularExpression(@"^5\d{9}$", ErrorMessage = "GSM 5 ile başlamalı ve 10 haneli olmalıdır.")]
    public string GSM { get; set; } = string.Empty;
}
```

**Özellikler:**
- ✅ Data Annotations validasyonlar
- ✅ Türkçe hata mesajları
- ✅ TCKN: 11 haneli rakam
- ✅ GSM: 5 ile başlayan 10 haneli

---

### 📄 `VerifyUserResponse.cs`
```csharp
public class VerifyUserResponse
{
    public int CustomerId { get; set; }
    public string TCKN { get; set; } = string.Empty;
    public string GSM { get; set; } = string.Empty;
    public bool IsNewUser { get; set; }
}
```

**Özellikler:**
- ✅ `CustomerId`: Müşteri ID
- ✅ `TCKN`: TC Kimlik No
- ✅ `GSM`: Telefon numarası
- ✅ `IsNewUser`: Yeni kullanıcı mı?

---

### 📄 `ServiceResponse<T>.cs`
```csharp
public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Value { get; set; }
    public int StatusCode { get; set; }

    public static ServiceResponse<T> SuccessResponse(T value, string message = "Success", int statusCode = 200)
    public static ServiceResponse<T> FailureResponse(string message, int statusCode = 400)
}
```

**Özellikler:**
- ✅ Generic wrapper sınıfı
- ✅ `Success`: İşlem başarılı mı?
- ✅ `Message`: Kullanıcı mesajı
- ✅ `Value`: Gerçek veri (T tipi)
- ✅ `StatusCode`: HTTP durum kodu
- ✅ Helper metodlar: `SuccessResponse()`, `FailureResponse()`

---

### 📄 `ApiSettings.cs`
```csharp
public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
}
```

**appsettings.json:**
```json
{
  "ApiSettings": {
    "BaseUrl": "https://customers-api.azurewebsites.net",
    "Timeout": 30
  }
}
```

---

## 2️⃣ Interface (`IApiService.cs`)

```csharp
public interface IApiService
{
    /// <summary>
    /// Verifies user credentials (TCKN and GSM) with the external API
    /// </summary>
    Task<ServiceResponse<VerifyUserResponse>> VerifyUserAsync(VerifyUserRequest request);
}
```

**Özellikler:**
- ✅ Async/await pattern
- ✅ Generic ServiceResponse dönüşü
- ✅ Temiz interface tanımı

---

## 3️⃣ Servis (`ApiService.cs`)

### Yapılandırma:
```csharp
public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}
```

### API Endpoint:
```csharp
var response = await _httpClient.PostAsJsonAsync("/api/customer/tckn-gsm", request, _jsonOptions);
```

**URL:** `https://customers-api.azurewebsites.net/api/customer/tckn-gsm`

### Hata Yönetimi:

#### 1. **Başarılı İstek (200 OK):**
```csharp
if (response.IsSuccessStatusCode)
{
    var result = await response.Content.ReadFromJsonAsync<VerifyUserResponse>(_jsonOptions);
    
    return ServiceResponse<VerifyUserResponse>.SuccessResponse(
        result,
        "Kullanıcı doğrulandı.",
        (int)response.StatusCode
    );
}
```

#### 2. **HTTP Hatası (400, 404, 500 vb.):**
```csharp
return ServiceResponse<VerifyUserResponse>.FailureResponse(
    $"Kullanıcı doğrulama başarısız: {response.ReasonPhrase}",
    (int)response.StatusCode
);
```

#### 3. **HttpRequestException (Ağ Hatası):**
```csharp
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "HTTP request error");
    
    return ServiceResponse<VerifyUserResponse>.FailureResponse(
        "API ile bağlantı kurulamadı. İnternet bağlantınızı kontrol edin.",
        500
    );
}
```

#### 4. **JsonException (Deserialization Hatası):**
```csharp
catch (JsonException ex)
{
    _logger.LogError(ex, "JSON deserialization error");
    
    return ServiceResponse<VerifyUserResponse>.FailureResponse(
        "API yanıtı işlenirken bir hata oluştu.",
        500
    );
}
```

#### 5. **Beklenmeyen Hata:**
```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    
    return ServiceResponse<VerifyUserResponse>.FailureResponse(
        "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
        500
    );
}
```

---

## 4️⃣ Dependency Injection (`Program.cs`)

### API Settings Yapılandırması:
```csharp
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>() ?? new ApiSettings();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
```

### HttpClient Kayıt:
```csharp
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(apiSettings.Timeout);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
```

**Özellikler:**
- ✅ `BaseAddress`: API base URL
- ✅ `Timeout`: 30 saniye timeout
- ✅ `Accept` header: `application/json`
- ✅ Typed HttpClient pattern

### Session Yapılandırması:
```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

---

## 5️⃣ Kullanım Örneği (Login.cshtml.cs)

```csharp
public class LoginModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    public VerifyUserRequest VerifyRequest { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public LoginModel(IApiService apiService, ILogger<LoginModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // API çağrısı
            var response = await _apiService.VerifyUserAsync(VerifyRequest);

            if (response.Success && response.Value != null)
            {
                // Başarılı - OTP sayfasına yönlendir
                TempData["CustomerId"] = response.Value.CustomerId;
                TempData["IsNewUser"] = response.Value.IsNewUser;
                
                return RedirectToPage("/Auth/OtpVerify");
            }
            else
            {
                // Hatalı - Hata mesajı göster
                ErrorMessage = response.Message;
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ErrorMessage = "Giriş yapılırken bir hata oluştu.";
            return Page();
        }
    }
}
```

---

## 🧪 Test Senaryoları

### 1. Başarılı İstek:
```json
Request:
{
  "tckn": "12345678901",
  "gsm": "5551234567"
}

Response:
{
  "customerId": 123,
  "tckn": "12345678901",
  "gsm": "5551234567",
  "isNewUser": false
}

ServiceResponse:
{
  "success": true,
  "message": "Kullanıcı doğrulandı.",
  "value": { ... },
  "statusCode": 200
}
```

### 2. Kullanıcı Bulunamadı (404):
```json
ServiceResponse:
{
  "success": false,
  "message": "Kullanıcı doğrulama başarısız: Not Found",
  "value": null,
  "statusCode": 404
}
```

### 3. Ağ Hatası:
```json
ServiceResponse:
{
  "success": false,
  "message": "API ile bağlantı kurulamadı. İnternet bağlantınızı kontrol edin.",
  "value": null,
  "statusCode": 500
}
```

### 4. Validation Hatası (Client-Side):
```
ModelState.IsValid = false
→ return Page();
→ Validation mesajları gösterilir
```

---

## 📊 Akış Diyagramı

```
┌─────────────────────────────────────────────────────────────┐
│                     Login Form                              │
│  (TCKN: 12345678901, GSM: 5551234567)                      │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      │ Form Submit
                      ▼
         ┌────────────────────────┐
         │   ModelState.IsValid?  │
         └────────┬───────────────┘
                  │
        ┌─────────┴─────────┐
        │                   │
      ❌ No               ✅ Yes
        │                   │
        │                   │
        ▼                   ▼
  ┌──────────┐    ┌────────────────────┐
  │ Show     │    │  ApiService.       │
  │ Errors   │    │  VerifyUserAsync() │
  └──────────┘    └────────┬───────────┘
                           │
                           │ HttpClient.PostAsJsonAsync()
                           │ POST /api/customer/tckn-gsm
                           │
                           ▼
              ┌────────────────────────┐
              │   API Response         │
              └────────┬───────────────┘
                       │
        ┌──────────────┴──────────────┐
        │                             │
      ✅ Success (200)              ❌ Error (4xx/5xx)
        │                             │
        │                             │
        ▼                             ▼
┌────────────────┐          ┌──────────────────┐
│ ServiceResponse│          │ ServiceResponse  │
│ Success = true │          │ Success = false  │
│ Value = data   │          │ Message = error  │
└───────┬────────┘          └────────┬─────────┘
        │                            │
        │                            │
        ▼                            ▼
┌────────────────┐          ┌──────────────────┐
│ TempData Store │          │ Show Error       │
│ Redirect to    │          │ Message          │
│ /Auth/OtpVerify│          │ return Page();   │
└────────────────┘          └──────────────────┘
```

---

## 🔒 Güvenlik Özellikleri

1. **HTTPS:** Tüm API istekleri HTTPS üzerinden
2. **Timeout:** 30 saniye timeout ile DoS koruması
3. **Validation:** Server-side ve client-side validation
4. **Logging:** Tüm işlemler loglama ile izlenir
5. **Error Handling:** Hassas bilgi sızdırılmaz
6. **HttpOnly Cookies:** Session cookie güvenliği

---

## 🚀 Performans Optimizasyonları

1. **Typed HttpClient:** HttpClient pool yönetimi
2. **JSON Options:** Reusable JsonSerializerOptions
3. **Async/Await:** Non-blocking I/O operations
4. **Connection Pooling:** HttpClient otomatik connection pooling
5. **Timeout Configuration:** 30 saniye timeout

---

## 📝 Sonraki Adımlar

- [ ] OTP verification endpoint'i ekle
- [ ] Retry policy ekle (Polly)
- [ ] Circuit breaker pattern (Polly)
- [ ] API response caching
- [ ] Rate limiting
- [ ] API versioning
- [ ] Health check endpoint

---

**Geliştirici**: AI Assistant  
**Tarih**: 20 Aralık 2025  
**Durum**: ✅ API Servis Katmanı Hazır ve Çalışıyor

