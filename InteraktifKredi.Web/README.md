# InteraktifKredi.Web - Service Layer Architecture

## 📋 Genel Bakış

Bu proje, **Interface-based Service Layer Architecture** kullanarak dış REST API'lerini tüketen bir .NET 8 Razor Pages uygulamasıdır.

## 🏗️ Mimari Yapı

### Katmanlar

```
InteraktifKredi.Web/
├── Models/
│   └── Api/
│       ├── ServiceResponse.cs         # Generic API response wrapper
│       ├── ApiSettings.cs             # Configuration model
│       └── Auth/
│           ├── VerifyUserRequest.cs   # TCKN/GSM verification request DTO
│           └── VerifyUserResponse.cs  # User verification response DTO
├── Services/
│   ├── IApiService.cs                 # Service interface
│   └── ApiService.cs                  # Service implementation
└── Pages/
    └── Auth/
        ├── Login.cshtml               # Login view
        └── Login.cshtml.cs            # Login page model (uses IApiService)
```

## 🔧 Teknolojiler

- **.NET 8** - Framework
- **Razor Pages** - UI
- **HttpClient** - HTTP requests
- **System.Text.Json** - JSON serialization
- **Dependency Injection** - Service registration

## 📦 Servis Katmanı

### ServiceResponse<T>
Generic wrapper sınıfı, tüm API yanıtlarını standartlaştırır:

```csharp
public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Value { get; set; }
    public int StatusCode { get; set; }
}
```

### IApiService
API işlemlerini tanımlayan interface:

```csharp
public interface IApiService
{
    Task<ServiceResponse<VerifyUserResponse>> VerifyUserAsync(VerifyUserRequest request);
}
```

### ApiService
HttpClient kullanarak API isteklerini gerçekleştiren implementasyon:

- Constructor Injection ile HttpClient ve ILogger alır
- JSON serialization için camelCase naming policy kullanır
- Tüm HTTP hatalarını gracefully handle eder
- Detaylı logging sağlar

## 🚀 Kullanım

### 1. Configuration (appsettings.json)

```json
{
  "ApiSettings": {
    "BaseUrl": "https://customers-api.azurewebsites.net",
    "Timeout": 30
  }
}
```

### 2. Dependency Injection (Program.cs)

```csharp
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(apiSettings.Timeout);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
```

### 3. Razor Page'de Kullanım

```csharp
public class LoginModel : PageModel
{
    private readonly IApiService _apiService;

    public LoginModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var response = await _apiService.VerifyUserAsync(VerifyRequest);
        
        if (response.Success && response.Value != null)
        {
            // Success logic
            return RedirectToPage("/Auth/OtpVerify");
        }
        else
        {
            // Error handling
            ErrorMessage = response.Message;
            return Page();
        }
    }
}
```

## 🔐 Güvenlik

- HTTPS zorunlu
- Session cookie'leri HttpOnly
- Input validation (Data Annotations)
- Error handling ve logging

## 📝 Validation

Request modelleri Data Annotations ile doğrulanır:

```csharp
[Required(ErrorMessage = "TCKN zorunludur.")]
[StringLength(11, MinimumLength = 11, ErrorMessage = "TCKN 11 karakter olmalıdır.")]
[RegularExpression(@"^\d{11}$", ErrorMessage = "TCKN sadece rakamlardan oluşmalıdır.")]
public string TCKN { get; set; }
```

## 🎯 Avantajlar

1. **Loose Coupling**: UI, API implementasyonundan bağımsızdır
2. **Testability**: Interface'ler mock edilebilir
3. **Maintainability**: Tek bir yerde API mantığı yönetilir
4. **Reusability**: Servisler birden fazla sayfada kullanılabilir
5. **Error Handling**: Merkezi hata yönetimi
6. **Logging**: Tüm API çağrıları loglanır

## 🧪 Test Etme

Projeyi çalıştırmak için:

```bash
cd InteraktifKredi.Web
dotnet run
```

Login sayfası: `https://localhost:5001/Auth/Login`

## 📚 Yeni Endpoint Ekleme

1. **DTO'ları oluştur** (`Models/Api/...`)
2. **Interface'e method ekle** (`IApiService.cs`)
3. **Implementation yaz** (`ApiService.cs`)
4. **Razor Page'de kullan**

Örnek:

```csharp
// IApiService.cs
Task<ServiceResponse<OtpResponse>> SendOtpAsync(OtpRequest request);

// ApiService.cs
public async Task<ServiceResponse<OtpResponse>> SendOtpAsync(OtpRequest request)
{
    // Implementation
}
```

## 🤝 Katkıda Bulunma

Yeni özellikler eklerken:
- PascalCase naming convention kullanın
- XML documentation ekleyin
- Error handling implementasyonu yapın
- Logging ekleyin

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

