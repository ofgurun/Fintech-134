# Login.cshtml.cs - Backend İşlem Akışı

## ✅ Tamamlanan Yapı

### 📋 Dosya: `Pages/Auth/Login.cshtml.cs`

---

## 1️⃣ Constructor Injection (Dependency Injection)

```csharp
public class LoginModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IApiService apiService, ILogger<LoginModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }
}
```

**Injected Services:**
- ✅ `IApiService` - API çağrıları için
- ✅ `ILogger<LoginModel>` - Loglama için

---

## 2️⃣ Properties

```csharp
[BindProperty]
public VerifyUserRequest VerifyRequest { get; set; } = new();

public string? ErrorMessage { get; set; }
```

**Özellikler:**
- ✅ `VerifyRequest` - Form'dan gelen TCKN ve GSM
- ✅ `[BindProperty]` - POST sırasında otomatik doldurulur
- ✅ `ErrorMessage` - Hata mesajı göstermek için

---

## 3️⃣ OnGet() - Sayfa Yüklenme

```csharp
public void OnGet()
{
    // Clear any previous TempData
    TempData.Clear();
}
```

**İşlem:**
- ✅ Önceki TempData verilerini temizler
- ✅ Sayfa ilk kez yüklendiğinde çalışır

---

## 4️⃣ OnPostAsync() - Form Submit İşlemi

### Akış Diyagramı:

```
┌──────────────────────┐
│   Form Submit        │
│  (TCKN + GSM)        │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ ModelState.IsValid?  │
└──────────┬───────────┘
           │
    ┌──────┴──────┐
    │             │
  ❌ No         ✅ Yes
    │             │
    │             ▼
    │   ┌────────────────────┐
    │   │ _apiService.       │
    │   │ VerifyUserAsync()  │
    │   └────────┬───────────┘
    │            │
    │            ▼
    │   ┌────────────────────┐
    │   │  API Response      │
    │   └────────┬───────────┘
    │            │
    │   ┌────────┴────────┐
    │   │                 │
    │ ✅ Success        ❌ Failure
    │   │                 │
    │   │                 │
    │   ▼                 ▼
    │ ┌─────────┐   ┌──────────┐
    │ │TempData │   │AddModel  │
    │ │Store    │   │Error     │
    │ │         │   │          │
    │ │Redirect │   │return    │
    │ │/OtpVerify│  │Page()    │
    │ └─────────┘   └──────────┘
    │                     ▲
    ▼                     │
┌──────────┐             │
│ return   │─────────────┘
│ Page()   │
└──────────┘
```

---

### Kod Detayları:

#### ✅ **Adım 1: ModelState Validation**

```csharp
if (!ModelState.IsValid)
{
    _logger.LogWarning("Login form validation failed");
    return Page();
}
```

**Kontroller:**
- TCKN: 11 haneli rakam mı?
- GSM: 5 ile başlayan 10 haneli mi?
- Required alanlar dolu mu?

**Sonuç:**
- ❌ Geçersiz → `return Page()` (Sayfa tekrar gösterilir, validation mesajları görünür)

---

#### ✅ **Adım 2: API Service Call**

```csharp
_logger.LogInformation("Attempting to verify user with TCKN: {TCKN}", VerifyRequest.TCKN);

var response = await _apiService.VerifyUserAsync(VerifyRequest);
```

**İstek:**
```json
POST https://customers-api.azurewebsites.net/api/customer/tckn-gsm
{
  "tckn": "12345678901",
  "gsm": "5551234567"
}
```

---

#### ✅ **Senaryo 1: Başarılı (Success = true)**

```csharp
if (response.Success && response.Value != null)
{
    // Store user information in TempData
    TempData["CustomerId"] = response.Value.CustomerId;
    TempData["IsNewUser"] = response.Value.IsNewUser;
    TempData["TCKN"] = response.Value.TCKN;
    TempData["GSM"] = response.Value.GSM;

    _logger.LogInformation("User verification successful for CustomerId: {CustomerId}", 
        response.Value.CustomerId);

    // Redirect to OTP verification page
    return RedirectToPage("/Auth/OtpVerify");
}
```

**İşlemler:**
1. ✅ TempData'ya kullanıcı bilgileri kaydedilir:
   - `CustomerId` - Müşteri ID
   - `IsNewUser` - Yeni kullanıcı mı?
   - `TCKN` - TC Kimlik No
   - `GSM` - Telefon numarası

2. ✅ Loglama yapılır

3. ✅ `/Auth/OtpVerify` sayfasına yönlendirme

**API Response Örneği:**
```json
{
  "success": true,
  "message": "Kullanıcı doğrulandı.",
  "value": {
    "customerId": 123,
    "tckn": "12345678901",
    "gsm": "5551234567",
    "isNewUser": false
  },
  "statusCode": 200
}
```

---

#### ❌ **Senaryo 2: Başarısız (Success = false)**

```csharp
else
{
    _logger.LogWarning("User verification failed: {Message} (Status: {StatusCode})", 
        response.Message, response.StatusCode);

    // Add model error to display in validation summary
    ModelState.AddModelError(string.Empty, 
        "Giriş bilgileri doğrulanamadı. Lütfen TCKN ve GSM bilgilerinizi kontrol ediniz.");
    
    // Also set ErrorMessage for custom display
    ErrorMessage = response.Message;

    return Page();
}
```

**İşlemler:**
1. ❌ Loglama yapılır (Warning level)
2. ❌ `ModelState.AddModelError()` - Validation summary'de gösterilir
3. ❌ `ErrorMessage` property'sine mesaj atanır
4. ❌ `return Page()` - Sayfa tekrar gösterilir

**Hata Mesajı:**
```
"Giriş bilgileri doğrulanamadı. Lütfen TCKN ve GSM bilgilerinizi kontrol ediniz."
```

**API Response Örneği:**
```json
{
  "success": false,
  "message": "Kullanıcı bulunamadı",
  "value": null,
  "statusCode": 404
}
```

---

#### 🔥 **Exception Handling**

##### 1. **HttpRequestException (Ağ Hatası)**

```csharp
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "Network error during login attempt");
    
    ModelState.AddModelError(string.Empty, 
        "Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edip tekrar deneyin.");
    ErrorMessage = "Bağlantı hatası oluştu.";
    
    return Page();
}
```

**Sebep:** API sunucusuna erişilemiyor, internet bağlantısı yok

---

##### 2. **Generic Exception (Beklenmeyen Hata)**

```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error during login attempt");
    
    ModelState.AddModelError(string.Empty, 
        "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
    ErrorMessage = "Giriş yapılırken bir hata oluştu. Lütfen tekrar deneyin.";
    
    return Page();
}
```

**Sebep:** Beklenmeyen sistem hatası, deserializasyon hatası vb.

---

## 5️⃣ TempData Kullanımı

### TempData Nedir?
- Tek bir redirect süresi boyunca veri taşır
- Session-based çalışır
- Bir kez okunduktan sonra silinir

### Kullanım:

**Login.cshtml.cs (Veri Kaydı):**
```csharp
TempData["CustomerId"] = response.Value.CustomerId;
TempData["IsNewUser"] = response.Value.IsNewUser;
```

**OtpVerify.cshtml.cs (Veri Okuma):**
```csharp
public void OnGet()
{
    var customerId = TempData["CustomerId"] as int?;
    var isNewUser = TempData["IsNewUser"] as bool?;
    
    if (customerId == null)
    {
        // Redirect back to login
        RedirectToPage("/Auth/Login");
    }
}
```

---

## 6️⃣ Logging Stratejisi

### Log Seviyeleri:

#### 🟢 **Information:**
```csharp
_logger.LogInformation("Attempting to verify user with TCKN: {TCKN}", VerifyRequest.TCKN);
_logger.LogInformation("User verification successful for CustomerId: {CustomerId}", customerId);
```

**Ne zaman:** Normal işlem akışı

---

#### 🟡 **Warning:**
```csharp
_logger.LogWarning("Login form validation failed");
_logger.LogWarning("User verification failed: {Message}", response.Message);
```

**Ne zaman:** Validasyon hatası, API'den hata dönmesi

---

#### 🔴 **Error:**
```csharp
_logger.LogError(ex, "Network error during login attempt");
_logger.LogError(ex, "Unexpected error during login attempt");
```

**Ne zaman:** Exception oluştuğunda

---

## 7️⃣ Kullanıcı Deneyimi Akışı

### ✅ Başarılı Senaryo:

```
1. Kullanıcı TCKN ve GSM girer
2. "Devam Et" butonuna tıklar
3. Client-side validation ✅
4. Form POST edilir
5. Server-side validation ✅
6. API çağrısı yapılır
7. API başarılı yanıt döner ✅
8. TempData'ya veriler kaydedilir
9. /Auth/OtpVerify sayfasına yönlendirilir
10. OTP girişi için SMS gönderilir
```

### ❌ Başarısız Senaryo:

```
1. Kullanıcı yanlış TCKN girer
2. "Devam Et" butonuna tıklar
3. Client-side validation ✅ (11 haneli)
4. Form POST edilir
5. Server-side validation ✅
6. API çağrısı yapılır
7. API hata yanıtı döner ❌
8. ModelState.AddModelError() çağrılır
9. Sayfa tekrar gösterilir
10. Kırmızı hata mesajı görünür:
    "Giriş bilgileri doğrulanamadı. Lütfen TCKN ve GSM bilgilerinizi kontrol ediniz."
```

---

## 8️⃣ Test Senaryoları

### Test 1: Geçersiz TCKN (9 karakter)
```
Input: TCKN = "123456789", GSM = "5551234567"
Expected: ModelState.IsValid = false
Result: Sayfa tekrar gösterilir, validation mesajı
```

### Test 2: Boş Form
```
Input: TCKN = "", GSM = ""
Expected: ModelState.IsValid = false
Result: "TCKN zorunludur", "GSM zorunludur" mesajları
```

### Test 3: API Başarılı
```
Input: TCKN = "12345678901", GSM = "5551234567"
API Response: Success = true, CustomerId = 123
Expected: RedirectToPage("/Auth/OtpVerify")
Result: TempData["CustomerId"] = 123
```

### Test 4: API Hata (Kullanıcı bulunamadı)
```
Input: TCKN = "99999999999", GSM = "5559999999"
API Response: Success = false, Message = "Kullanıcı bulunamadı"
Expected: return Page()
Result: "Giriş bilgileri doğrulanamadı" mesajı
```

### Test 5: Ağ Hatası
```
Input: TCKN = "12345678901", GSM = "5551234567"
Exception: HttpRequestException
Expected: return Page()
Result: "Sunucuya bağlanılamadı" mesajı
```

---

## 9️⃣ Güvenlik Özellikleri

1. ✅ **Server-side Validation**: Client-side atlanabilir, server her zaman kontrol eder
2. ✅ **HTTPS**: Tüm API çağrıları güvenli kanal üzerinden
3. ✅ **Logging**: Tüm işlemler loglanır (monitoring için)
4. ✅ **Error Handling**: Hassas bilgi sızdırılmaz
5. ✅ **TempData**: Session-based, güvenli veri taşıma
6. ✅ **Model Binding**: SQL injection koruması

---

## 🔄 Sonraki Adım: OTP Verify

TempData ile taşınan bilgiler:
- `CustomerId` - OTP göndermek için
- `IsNewUser` - Yeni kullanıcı flow'u için
- `TCKN` - OTP doğrulama için
- `GSM` - SMS gönderim için

---

**Geliştirici**: AI Assistant  
**Tarih**: 20 Aralık 2025  
**Durum**: ✅ Login Backend Entegrasyonu Tamamlandı

