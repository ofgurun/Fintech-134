# 📋 TAMAMLANMAMIŞ GÖREVLER - Header & Footer

## 🔴 YÜKSEK ÖNCELİK

### 1. ✅ Logo Görseli Ekleme
**Durum:** Header'da şu an placeholder text var
**Yapılacaklar:**
- [ ] `wwwroot/img/` klasörü oluştur
- [ ] Logo görselini ekle (örn: `logo.svg` veya `logo.png`)
- [ ] Header'da logo görselini göster
- [ ] Responsive için mobil logo boyutu ayarla

**Dosya:** `Pages/Shared/_Header.cshtml` (satır 12-15)

---

### 2. ✅ Aktif Sayfa Tespiti İyileştirme
**Durum:** Sadece Dashboard ve Loan için aktif state var
**Yapılacaklar:**
- [ ] Tüm sayfalar için aktif state kontrolü ekle:
  - `/Account/Profile` → "Profil Ayarları" aktif olmalı
  - `/Auth/Login` → Giriş yapmamış kullanıcılar için
  - `/Privacy` → Footer linki için
- [ ] Daha doğru aktif sayfa tespiti (exact match)

**Dosya:** `Pages/Shared/_Header.cshtml` (satır 6, 24, 48)

---

### 3. ✅ Footer Linklerini Düzeltme
**Durum:** "Kullanım Koşulları" ve "İletişim" Dashboard'a gidiyor
**Yapılacaklar:**
- [ ] Kullanım Koşulları sayfası oluştur (`/Terms`) veya mevcut sayfaya yönlendir
- [ ] İletişim sayfası oluştur (`/Contact`) veya mevcut sayfaya yönlendir
- [ ] Footer linklerini güncelle

**Dosya:** `Pages/Shared/_Footer.cshtml` (satır 14, 17)

---

## 🟡 ORTA ÖNCELİK

### 4. ✅ Session Yönetimi - UserName
**Durum:** UserName session'a kaydedilmiyor, Login'den gelmeli
**Yapılacaklar:**
- [ ] Login sayfasından sonra UserName session'a kaydedilmeli
- [ ] Header'da kullanıcı adı doğru gösterilmeli
- [ ] Fallback olarak "Kullanıcı" gösteriliyor (şu an çalışıyor)

**Dosya:** 
- `Pages/Shared/_Header.cshtml` (satır 5)
- Login sayfasından sonra session'a kaydedilmeli (ARKADAŞ1'in görevi ama koordinasyon gerekli)

---

### 5. ✅ wwwroot/img Klasörü Oluşturma
**Durum:** Logo görselleri için klasör yok
**Yapılacaklar:**
- [ ] `wwwroot/img/` klasörü oluştur
- [ ] Logo görsellerini ekle
- [ ] README veya dokümantasyonda klasör yapısını belirt

---

## 🟢 DÜŞÜK ÖNCELİK (İyileştirmeler)

### 6. ✅ Responsive Test ve İyileştirmeler
**Durum:** Kod hazır ama test edilmeli
**Yapılacaklar:**
- [ ] Mobil menü açılıp kapanma testi
- [ ] Dropdown menü mobilde çalışıyor mu test et
- [ ] Tablet görünümü test et
- [ ] Farklı ekran boyutlarında test et

**Test Edilecekler:**
- Hamburger menü toggle
- Overlay click
- ESC tuşu ile kapatma
- Window resize handling
- User dropdown mobilde

---

### 7. ✅ Accessibility İyileştirmeleri
**Durum:** Temel ARIA labels var ama iyileştirilebilir
**Yapılacaklar:**
- [ ] Tüm interaktif elementler için ARIA labels kontrol et
- [ ] Keyboard navigation test et
- [ ] Focus states iyileştir
- [ ] Screen reader uyumluluğu test et

**Dosya:** `Pages/Shared/_Header.cshtml`

---

## 📝 NOTLAR

### Diğer Ekip Arkadaşlarıyla Koordinasyon Gerekenler:

1. **ARKADAŞ1 (Login):**
   - Login başarılı olduğunda `UserName` session'a kaydedilmeli
   - Session key: `"UserName"`

2. **ARKADAŞ2 (Loan):**
   - `/Loan/Apply` sayfası hazır olduğunda Header'daki link çalışacak
   - Aktif state zaten hazır

3. **Diğer Ekip:**
   - `/Account/Profile` sayfası hazır olduğunda Header dropdown'daki link çalışacak
   - `/Privacy` sayfası hazır olduğunda Footer linki çalışacak

---

## 🎯 ÖNCELİK SIRASI

1. **Logo görseli ekleme** (Hızlı, görsel iyileştirme)
2. **Aktif sayfa tespiti iyileştirme** (UX iyileştirme)
3. **Footer linklerini düzeltme** (Fonksiyonel)
4. **Session yönetimi** (Koordinasyon gerekli)
5. **Responsive testler** (Kalite kontrol)
6. **Accessibility** (İyileştirme)

