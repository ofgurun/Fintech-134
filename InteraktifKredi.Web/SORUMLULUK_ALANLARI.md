# 📋 SORUMLULUK ALANLARI - Header & Footer

## ✅ SENİN SORUMLULUĞUN (feature/dashboard-ui branch)

### Sadece Header ve Footer Bileşenleri

**Sorumlu Olduğun:**
- ✅ `Pages/Shared/_Header.cshtml` - Header partial view
- ✅ `Pages/Shared/_Footer.cshtml` - Footer partial view
- ✅ `Styles/layout/_header.scss` - Header stilleri
- ✅ `Styles/layout/_footer.scss` - Footer stilleri
- ✅ `wwwroot/js/site.js` - Mobil menü ve dropdown jQuery
- ✅ `Pages/Shared/_Layout.cshtml` - Layout (Header ve Footer'ı include ediyor)

**Sorumlu Olmadığın:**
- ❌ Sayfa içerikleri (Dashboard, Login, Loan, Account vb.)
- ❌ Sayfa içi stiller (sadece Header/Footer stilleri bizim)
- ❌ Sayfa içi JavaScript (sadece Header/Footer JS bizim)

---

## 📄 SAYFA İÇERİKLERİ - SORUMLULUK DAĞILIMI

### Dashboard Sayfası (`/Dashboard`)
- **İçerik:** ⚪ **Diğer ekip** (henüz atanmamış)
- **Header/Footer:** ✅ **SEN** (tüm sayfalarda görünür)

**Not:** Dashboard sayfasındaki mevcut içerik (Hoş Geldiniz + 3 kart) sadece **test amaçlı** oluşturuldu. Gerçek içerik başka bir ekip üyesi tarafından yapılacak.

### Login Sayfası (`/Auth/Login`)
- **İçerik:** 🟢 **ARKADAŞ1** (feature/auth-screens branch)
- **Header/Footer:** ✅ **SEN** (tüm sayfalarda görünür)

### Loan Apply Sayfası (`/Loan/Apply`)
- **İçerik:** 🔵 **ARKADAŞ2** (feature/loan-wizard branch)
- **Header/Footer:** ✅ **SEN** (tüm sayfalarda görünür)

### Diğer Sayfalar
- **Account/Profile:** ⚪ Diğer ekip
- **Privacy:** ⚪ Diğer ekip
- **Terms:** ✅ **SEN** (Footer linki için oluşturuldu)
- **Contact:** ✅ **SEN** (Footer linki için oluşturuldu)

---

## 🎯 ÖNEMLİ NOTLAR

### 1. Header ve Footer Tüm Sayfalarda Görünür
- `_Layout.cshtml` içinde Header ve Footer include ediliyor
- Bu yüzden **tüm sayfalarda** Header ve Footer görünür
- Ama sayfa içerikleri başkalarının sorumluluğunda

### 2. Dashboard İçeriği
- Şu anki Dashboard içeriği (Hoş Geldiniz + kartlar) **test amaçlı**
- Gerçek Dashboard içeriği başka bir ekip üyesi tarafından yapılacak
- Sen sadece Header ve Footer'dan sorumlusun

### 3. Terms ve Contact Sayfaları
- Bu sayfalar Footer linkleri için oluşturuldu
- Basit içerikler var (Footer linklerinin çalışması için)
- İçerikler daha sonra genişletilebilir

---

## ✅ SONUÇ

**SENİN SORUMLULUĞUN:**
- ✅ Header (üst menü)
- ✅ Footer (alt linkler)
- ✅ Layout (Header ve Footer'ı include ediyor)

**SENİN SORUMLULUĞUN DEĞİL:**
- ❌ Sayfa içerikleri (Dashboard, Login, Loan vb.)
- ❌ Sayfa içi stiller (sadece Header/Footer stilleri)

**Özet:** Header ve Footer tüm sayfalarda görünür ama sayfa içerikleri başkalarının sorumluluğunda.

