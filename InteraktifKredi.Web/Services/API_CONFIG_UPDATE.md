# API Konfigürasyon Güncellemesi - Yeni Format

## ✅ Güncellenen Yapı

API konfigürasyon yapısı yeni formata başarıyla güncellendi.

---

## 📋 Yapılan Değişiklikler

### 1. **ApiSettings.cs** ✅

**Eski Format:**
```csharp
public class ApiSettings
{
    public string BaseUrl { get; set; }          // KALDIRILDI
    public string FunctionKey { get; set; }       // KALDIRILDI
    public int Timeout { get; set; }
}
```

**Yeni Format:**
```csharp
public class ApiSettings
{
    public string CustomersApi { get; set; }     // YENİ
    public string IdcApi { get; set; }           // YENİ
    public string DefaultToken { get; set; }     // YENİ (FunctionKey yerine)
    public int Timeout { get; set; }
}
```

---

### 2. **appsettings.json** ✅

**Yeni Format:**
```json
{
  "ApiSettings": {
    "CustomersApi": "https://customers-api.azurewebsites.net",
    "IdcApi": "https://api-idc.azurewebsites.net",
    "DefaultToken": "gww5m66SOHBjQ9LY58dM5Gu1q2giauLokvvIX4y1R4O5AzFu7CUbIA=="
  }
}
```

**Not:** `CustomersApi` artık `/api/` ile bitmiyor.

---

### 3. **ApiService.cs** ✅

**Eski URL Oluşturma:**
```csharp
// BaseUrl: https://customers-api.azurewebsites.net/api/
var endpoint = $"customer/tckn-gsm?code={_apiSettings.FunctionKey}";
var response = await _httpClient.PostAsJsonAsync(endpoint, request, _jsonOptions);
// Sonuç: https://customers-api.azurewebsites.net/api/customer/tckn-gsm?code=...
```

**Yeni URL Oluşturma:**
```csharp
// CustomersApi: https://customers-api.azurewebsites.net (slash yok)
var fullUrl = $"{_apiSettings.CustomersApi}/api/customer/tckn-gsm?code={_apiSettings.DefaultToken}";
var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUrl);
var response = await _httpClient.SendAsync(httpRequest);
// Sonuç: https://customers-api.azurewebsites.net/api/customer/tckn-gsm?code=...
```

---

### 4. **Program.cs** ✅

**Değişiklik:**
```csharp
// ESKI
client.BaseAddress = new Uri(apiSettings.BaseUrl);

// YENİ (BaseAddress artık kullanılmıyor)
// Full URL doğrudan ApiService içinde oluşturuluyor
```

---

## ❌ Hala 401 Unauthorized Hatası Alınıyor

### Sorun:

Test scriptinde ve uygulamada hala **401 Unauthorized** hatası alınıyor. Bu, **DefaultToken'ın geçersiz** olduğu anlamına geliyor.

### Test Sonucu:

```
Full URL: https://customers-api.azurewebsites.net/api/customer/tckn-gsm?code=gww5m66SOHBjQ9LY58dM5Gu1q2giauLokvvIX4y1R4O5AzFu7CUbIA==
Status Code: 401
Status Description: Unauthorized
```

---

## 🔑 Çözüm: Yeni Token Almanız Gerekiyor

### Azure Portal'dan Yeni Token Alma Adımları:

1. **Azure Portal'a Gidin:**
   ```
   https://portal.azure.com
   ```

2. **Function App'i Bulun:**
   - "All resources" veya "Function Apps"
   - `customers-api` adlı Function App

3. **App Keys Bölümüne Gidin:**
   - Function App > Settings > **"App keys"**
   - Veya Function App > **"Functions"** > **"App keys"**

4. **Key'i Kopyalayın:**
   - **"Host keys"** bölümünde:
     - `default` key
     - veya `_master` key
   - **"Show values"** butonuna tıklayın
   - Key'i kopyalayın

5. **appsettings.json'u Güncelleyin:**
   ```json
   {
     "ApiSettings": {
       "CustomersApi": "https://customers-api.azurewebsites.net",
       "IdcApi": "https://api-idc.azurewebsites.net",
       "DefaultToken": "BURAYA_YENİ_TOKEN_YAPIŞTIRIN"
     }
   }
   ```

6. **appsettings.Development.json'u da Güncelleyin:**
   Aynı token'ı buraya da yazın.

---

## 🧪 Test Etme

### PowerShell Test Scripti:

```powershell
cd InteraktifKredi.Web
.\test-api-key.ps1
```

**Başarılı Yanıt:**
```
========================================
  SUCCESS! ✅
========================================

Response:
{
  "customerId": 123,
  "tckn": "12345678901",
  "gsm": "5551112233",
  "isNewUser": false
}
```

---

## 📊 Değişiklik Özeti

| Öğe | Eski Değer | Yeni Değer | Durum |
|-----|-----------|-----------|-------|
| **ApiSettings Property** | `BaseUrl` | `CustomersApi` | ✅ Güncellendi |
| **ApiSettings Property** | `FunctionKey` | `DefaultToken` | ✅ Güncellendi |
| **ApiSettings Property** | - | `IdcApi` | ✅ Eklendi |
| **URL Format** | `{BaseUrl}customer/tckn-gsm?code=...` | `{CustomersApi}/api/customer/tckn-gsm?code=...` | ✅ Güncellendi |
| **HttpClient BaseAddress** | Kullanılıyordu | Kaldırıldı | ✅ Güncellendi |
| **Token Değeri** | `gww5m66SOHBjQ9LY58dM5...` | Geçersiz (401 hata) | ❌ Yenilenmeli |

---

## ✅ Tamamlanan Görevler

- ✅ `ApiSettings.cs` güncellendi
- ✅ `ApiService.cs` güncellendi  
- ✅ `Program.cs` güncellendi
- ✅ `appsettings.json` formatı güncellendi
- ✅ `appsettings.Development.json` formatı güncellendi
- ✅ `test-api-key.ps1` güncellendi
- ✅ URL oluşturma mantığı düzeltildi
- ✅ Linter hataları yok

---

## ⏳ Bekleyen Görevler

- ❌ **Geçerli DefaultToken alınmalı** (Azure Portal'dan)
- ⏳ Token güncellendikten sonra test edilmeli
- ⏳ Login sayfası test edilmeli

---

## 🔒 Güvenlik Notu

- DefaultToken hassas bir bilgidir
- Production'da environment variable kullanın
- appsettings.json dosyası git'e commit edilmemeli (veya token kaldırılmalı)
- Azure Key Vault entegrasyonu önerilir

---

**Özet:** Kod yapısı başarıyla yeni formata güncellendi. Ancak kullandığınız DefaultToken geçersiz. Azure Portal'dan yeni bir token almanız gerekiyor.

**Geliştirici**: AI Assistant  
**Tarih**: 20 Aralık 2025  
**Durum**: ✅ Kod Güncellemesi Tamamlandı | ❌ Token Geçersiz

