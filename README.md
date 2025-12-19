# 🏦 Fintech-Jam-134 - InteraktifKredi.Web

## 📋 Proje Hakkında

**InteraktifKredi.Web**, modern fintech uygulamaları için geliştirilmiş, .NET 8 tabanlı bir **Interaktif Kredi Başvuru Sistemi**'dir. Interface-based Service Layer Architecture kullanarak dış REST API'leri ile entegrasyon sağlar.

## 🚀 Özellikler

### ✨ Teknik Özellikler
- **.NET 8** Razor Pages
- **Interface-based Architecture** (Repository/Service Pattern)
- **Dependency Injection**
- **HttpClient** ile REST API entegrasyonu
- **System.Text.Json** serialization
- **Bootstrap 5** UI Framework
- **SCSS** modular stil yönetimi
- **Data Annotations** validation

### 🔐 Güvenlik
- HTTPS enforcement
- Input validation
- Session management
- Secure error handling
- Comprehensive logging

### 📱 Modüller

#### 1. **Kimlik Doğrulama (Auth)**
- TCKN ve GSM ile giriş
- OTP doğrulama
- Session yönetimi

#### 2. **Dashboard (Web Şube)**
- Hizmet listesi
- Kullanıcı profili
- Navigasyon

#### 3. **Kredi İşlemleri (Loan)**
- Kredi başvuru sihirbazı
- Başvuru sonucu görüntüleme
- Form validasyonu

#### 4. **Profil Yönetimi (Account)**
- Kullanıcı bilgileri güncelleme
- Kişisel veri yönetimi

## 🏗️ Proje Yapısı

```
Fintech-Jam-134/
├── InteraktifKredi.Web/
│   ├── Models/
│   │   ├── Api/
│   │   │   ├── ServiceResponse.cs          # Generic API response wrapper
│   │   │   ├── ApiSettings.cs              # Configuration model
│   │   │   └── Auth/
│   │   │       ├── VerifyUserRequest.cs    # Login request DTO
│   │   │       └── VerifyUserResponse.cs   # Login response DTO
│   │   └── ...
│   ├── Services/
│   │   ├── IApiService.cs                  # Service interface
│   │   └── ApiService.cs                   # Service implementation
│   ├── Pages/
│   │   ├── Auth/                           # Authentication pages
│   │   ├── Dashboard/                      # Dashboard pages
│   │   ├── Loan/                           # Loan application pages
│   │   ├── Account/                        # Profile management
│   │   └── Shared/                         # Layout & partials
│   ├── Styles/                             # SCSS modular styles
│   │   ├── abstracts/
│   │   ├── base/
│   │   ├── components/
│   │   ├── pages/
│   │   └── main.scss
│   └── wwwroot/                            # Static files
│       ├── css/
│       ├── js/
│       ├── img/
│       └── lib/
├── .gitignore
├── cursorrules.md
└── README.md
```

## 🔧 Kurulum

### Gereksinimler
- **.NET 8 SDK** ([İndir](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Visual Studio 2022** veya **Visual Studio Code**
- **Git**

### Adımlar

1. **Repository'yi klonlayın:**
```bash
git clone https://github.com/ofgurun/Fintech-Jam-134.git
cd Fintech-Jam-134
```

2. **Projeyi restore edin:**
```bash
cd InteraktifKredi.Web
dotnet restore
```

3. **Uygulamayı çalıştırın:**
```bash
dotnet run
```

4. **Tarayıcıda açın:**
```
https://localhost:5001
```

## ⚙️ Yapılandırma

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "https://customers-api.azurewebsites.net",
    "Timeout": 30
  }
}
```

### Environment Variables
Geliştirme ortamında `appsettings.Development.json` kullanılır:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://customers-api.azurewebsites.net",
    "Timeout": 60
  }
}
```

## 📚 API Entegrasyonu

### Service Layer Kullanımı

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
        
        // Error handling
        ErrorMessage = response.Message;
        return Page();
    }
}
```

### Yeni Endpoint Ekleme

1. **DTO'ları oluşturun** (`Models/Api/`)
2. **Interface'e method ekleyin** (`IApiService.cs`)
3. **Implementation yazın** (`ApiService.cs`)
4. **Razor Page'de kullanın**

Detaylı bilgi için: [InteraktifKredi.Web/README.md](InteraktifKredi.Web/README.md)

## 🎨 Stil Yönetimi

SCSS modular yapısı:
```scss
// main.scss
@import 'abstracts/variables';
@import 'abstracts/mixins';
@import 'base/reset';
@import 'base/typography';
@import 'components/buttons';
@import 'components/forms';
@import 'pages/login';
@import 'pages/dashboard';
```

## 🧪 Test

```bash
# Projeyi derle
dotnet build

# Testleri çalıştır
dotnet test

# Projeyi çalıştır
dotnet run
```

## 📖 Dokümantasyon

- [Service Layer Architecture Dokümantasyonu](InteraktifKredi.Web/README.md)
- [API Entegrasyon Rehberi](#api-entegrasyonu)
- [Cursor Rules](cursorrules.md)

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'feat: Add amazing feature'`)
4. Push edin (`git push origin feature/amazing-feature`)
5. Pull Request açın

### Commit Mesaj Formatı
- `feat:` Yeni özellik
- `fix:` Hata düzeltme
- `docs:` Dokümantasyon
- `style:` Kod formatı
- `refactor:` Kod iyileştirme
- `test:` Test ekleme
- `chore:` Bakım işleri

## 🔒 Güvenlik

Bu proje GitHub Secret Scanning kullanmaktadır. Hassas bilgileri (API keys, secrets) asla kodda saklamayın. Environment variables veya Azure Key Vault kullanın.

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 👥 Ekip

- **Developer**: Senior .NET 8 Developer
- **Architecture**: Clean Architecture, Service Layer Pattern
- **Framework**: .NET 8, Razor Pages, Bootstrap 5

## 📞 İletişim

- **GitHub**: [ofgurun/Fintech-Jam-134](https://github.com/ofgurun/Fintech-Jam-134)
- **Issues**: [GitHub Issues](https://github.com/ofgurun/Fintech-Jam-134/issues)

---

⭐ Bu projeyi beğendiyseniz, yıldız vermeyi unutmayın!

**Built with ❤️ using .NET 8**

