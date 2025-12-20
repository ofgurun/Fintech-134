# 📋 GÖREV DAĞILIMI VE SAYFA ROUTING ÖZETİ

## 🎯 SENİN GÖREVİN (feature/dashboard-ui branch)

### ✅ Tamamlanan İşler:
1. ✅ `Pages/Shared/_Header.cshtml` - Header partial view
2. ✅ `Pages/Shared/_Footer.cshtml` - Footer partial view
3. ✅ `Styles/layout/_header.scss` - Header stilleri
4. ✅ `Styles/layout/_footer.scss` - Footer stilleri
5. ✅ `wwwroot/js/site.js` - Mobil menü ve dropdown jQuery kodu
6. ✅ `Pages/Shared/_Layout.cshtml` - Layout güncellemesi

### 📍 Sorumluluğundaki Bileşenler:

#### HEADER (_Header.cshtml)
Header'da bulunan **tüm linkler ve butonlar**:

| Buton/Link | Hedef Sayfa | Route | Sorumlu Ekip |
|------------|-------------|-------|--------------|
| **Logo** | Dashboard | `/Dashboard` | ✅ Sen (Header'da) |
| **Ana Ekran** (menü) | Dashboard | `/Dashboard` | ✅ Sen (Header'da) |
| **Hizmetler** (menü) | Dashboard | `/Dashboard` | ✅ Sen (Header'da) |
| **Kredi Hesaplama** (menü) | Loan Apply | `/Loan/Apply` | 🔵 ARKADAŞ2 |
| **SSS** (menü) | Dashboard | `/Dashboard` | ✅ Sen (Header'da) |
| **Giriş Yap** (giriş yapmamış) | Login | `/Auth/Login` | 🟢 ARKADAŞ1 |
| **Kullanıcı Avatar** (dropdown toggle) | - | - | ✅ Sen (Header'da) |
| **Profil Ayarları** (dropdown) | Account Profile | `/Account/Profile` | ⚪ Diğer ekip |
| **Çıkış Yap** (dropdown) | Logout | `/Auth/Logout` | ✅ Sen (Header'da) |
| **Hamburger Menü** (mobil) | - | - | ✅ Sen (Header'da) |

#### FOOTER (_Footer.cshtml)
Footer'da bulunan **tüm linkler**:

| Link | Hedef Sayfa | Route | Sorumlu Ekip |
|------|-------------|-------|--------------|
| **Gizlilik Politikası** | Privacy | `/Privacy` | ⚪ Diğer ekip |
| **Kullanım Koşulları** | Dashboard | `/Dashboard` | ✅ Sen (Footer'da) |
| **İletişim** | Dashboard | `/Dashboard` | ✅ Sen (Footer'da) |

---

## 🟢 ARKADAŞ1 - GÖREVİ (feature/auth-screens branch)

### Sorumlu Olduğu Sayfalar:
1. **`Pages/Auth/Login.cshtml`** - Login sayfası
   - Route: `/Auth/Login`
   - Header'dan açılır: "Giriş Yap" butonu (giriş yapmamış kullanıcılar için)
   - Görsel referans: `Light Mode - Desktop - Login Page.jpg`

2. **`Pages/Auth/OtpVerify.cshtml`** - OTP doğrulama sayfası
   - Route: `/Auth/OtpVerify`
   - Login sayfasından yönlendirilir

### Header'dan Tetiklenen:
- ✅ Header'da "Giriş Yap" butonu → `/Auth/Login` sayfasına gider

---

## 🔵 ARKADAŞ2 - GÖREVİ (feature/loan-wizard branch)

### Sorumlu Olduğu Sayfalar:
1. **`Pages/Loan/Apply.cshtml`** - Kredi başvuru formu
   - Route: `/Loan/Apply`
   - Header'dan açılır: "Kredi Hesaplama" menü linki
   - Görsel referans: `image_9d09d1.png` (input stilleri)

2. **`Pages/Loan/Result.cshtml`** - Kredi başvuru sonucu
   - Route: `/Loan/Result`
   - Apply sayfasından yönlendirilir

### Sorumlu Olduğu Stiller:
- **`Styles/components/_forms.scss`** - Form input stilleri
  - Tüm formlarda kullanılacak özel input stilleri
  - Görsel referans: `image_9d09d1.png`

### Header'dan Tetiklenen:
- ✅ Header'da "Kredi Hesaplama" menü linki → `/Loan/Apply` sayfasına gider

---

## 📊 TÜM SAYFALAR VE ROUTING

### ✅ SENİN SORUMLULUĞUNDAKİ SAYFALAR (Header/Footer):

| Sayfa | Route | Nasıl Açılır | Durum |
|-------|-------|--------------|-------|
| **Dashboard** | `/Dashboard` | Logo tıklama, "Ana Ekran" menü, "Hizmetler" menü | ✅ Header'da link var |
| **Privacy** | `/Privacy` | Footer "Gizlilik Politikası" linki | ✅ Footer'da link var |
| **Logout** | `/Auth/Logout` | Header kullanıcı dropdown "Çıkış Yap" | ✅ Header'da link var |

### 🟢 ARKADAŞ1 SORUMLULUĞUNDAKİ SAYFALAR:

| Sayfa | Route | Nasıl Açılır | Durum |
|-------|-------|--------------|-------|
| **Login** | `/Auth/Login` | Header "Giriş Yap" butonu (giriş yapmamış) | 🟢 ARKADAŞ1 yapacak |
| **OtpVerify** | `/Auth/OtpVerify` | Login'den yönlendirilir | 🟢 ARKADAŞ1 yapacak |

### 🔵 ARKADAŞ2 SORUMLULUĞUNDAKİ SAYFALAR:

| Sayfa | Route | Nasıl Açılır | Durum |
|-------|-------|--------------|-------|
| **Loan Apply** | `/Loan/Apply` | Header "Kredi Hesaplama" menü linki | 🔵 ARKADAŞ2 yapacak |
| **Loan Result** | `/Loan/Result` | Apply'den yönlendirilir | 🔵 ARKADAŞ2 yapacak |

### ⚪ DİĞER SAYFALAR (Henüz atanmamış):

| Sayfa | Route | Nasıl Açılır | Durum |
|-------|-------|--------------|-------|
| **Account Profile** | `/Account/Profile` | Header kullanıcı dropdown "Profil Ayarları" | ⚪ Atanmamış |
| **Error** | `/Error` | Sistem hatalarında | ⚪ Sistem sayfası |
| **Index** | `/` | Ana sayfa | ✅ Mevcut |

---

## 🔗 HEADER'DAKİ TÜM LİNKLER VE HEDEFLERİ

### Giriş Yapmış Kullanıcılar İçin:

```
HEADER MENÜSÜ:
├── Logo → /Dashboard (✅ Sen)
├── Ana Ekran → /Dashboard (✅ Sen)
├── Hizmetler → /Dashboard (✅ Sen)
├── Kredi Hesaplama → /Loan/Apply (🔵 ARKADAŞ2)
├── SSS → /Dashboard (✅ Sen)
└── Kullanıcı Dropdown:
    ├── Profil Ayarları → /Account/Profile (⚪ Diğer)
    └── Çıkış Yap → /Auth/Logout (✅ Sen)
```

### Giriş Yapmamış Kullanıcılar İçin:

```
HEADER:
├── Logo → /Dashboard (✅ Sen)
└── Giriş Yap Butonu → /Auth/Login (🟢 ARKADAŞ1)
```

---

## 🔗 FOOTER'DAKİ TÜM LİNKLER VE HEDEFLERİ

```
FOOTER:
├── Gizlilik Politikası → /Privacy (⚪ Diğer)
├── Kullanım Koşulları → /Dashboard (✅ Sen)
└── İletişim → /Dashboard (✅ Sen)
```

---

## ✅ SENİN YAPMAN GEREKENLER (Özet)

### 1. Header (_Header.cshtml) - ✅ TAMAMLANDI
- Logo ve navigasyon menüsü
- Kullanıcı profil dropdown
- Mobil hamburger menü
- Giriş yapmamış kullanıcılar için "Giriş Yap" butonu

### 2. Footer (_Footer.cshtml) - ✅ TAMAMLANDI
- Telif hakkı bilgisi
- Footer linkleri

### 3. Stiller - ✅ TAMAMLANDI
- `Styles/layout/_header.scss`
- `Styles/layout/_footer.scss`

### 4. JavaScript - ✅ TAMAMLANDI
- `wwwroot/js/site.js` - Mobil menü ve dropdown işlevselliği

### 5. Layout - ✅ TAMAMLANDI
- `Pages/Shared/_Layout.cshtml` - Header ve Footer'ı include ediyor

---

## 🎯 ÖNEMLİ NOTLAR

1. **Header ve Footer tüm sayfalarda görünecek** çünkü `_Layout.cshtml` içinde include ediliyor.

2. **Header'daki linkler:**
   - `/Loan/Apply` → ARKADAŞ2'nin sayfası (henüz yapılmadı, link hazır)
   - `/Auth/Login` → ARKADAŞ1'in sayfası (henüz yapılmadı, link hazır)
   - `/Account/Profile` → Henüz atanmamış

3. **Footer'daki linkler:**
   - `/Privacy` → Henüz atanmamış
   - `/Dashboard` → Mevcut (senin header'ında link var)

4. **Mobil menü:** Header'da hamburger menü var, jQuery ile çalışıyor ✅

5. **Aktif sayfa tespiti:** Header'da aktif sayfa için `header__nav_link--active` class'ı ekleniyor ✅

---

## 📝 SONUÇ

**SENİN GÖREVİN TAMAMLANDI! ✅**

Header ve Footer partial view'ları hazır. Diğer ekip arkadaşlarının sayfaları hazır olduğunda, Header'daki linkler otomatik olarak çalışacak.

**Yapman gereken ek bir şey yok** - Header ve Footer tüm sayfalarda otomatik görünecek çünkü `_Layout.cshtml` içinde include ediliyor.

