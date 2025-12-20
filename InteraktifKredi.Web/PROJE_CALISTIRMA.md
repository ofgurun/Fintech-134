# 🚀 PROJE ÇALIŞTIRMA REHBERİ

## Yöntem 1: Visual Studio Code ile (Önerilen)

### Adımlar:
1. **Visual Studio Code'u aç**
2. **Projeyi aç:**
   - File > Open Folder
   - `C:\Users\Baki\Desktop\FinTech\Fintech-134\InteraktifKredi.Web` klasörünü seç

3. **Terminal aç:**
   - `Ctrl + ~` (tilde tuşu) veya
   - View > Terminal

4. **Projeyi çalıştır:**
   ```powershell
   dotnet run
   ```

5. **Tarayıcıda aç:**
   - Terminal'de görünen URL'i kopyala (örn: `http://localhost:5257`)
   - Tarayıcıda aç veya otomatik açılır

---

## Yöntem 2: PowerShell ile (Hızlı)

### Adımlar:
1. **PowerShell'i aç** (Windows tuşu + X > Windows PowerShell)

2. **Proje klasörüne git:**
   ```powershell
   cd "C:\Users\Baki\Desktop\FinTech\Fintech-134\InteraktifKredi.Web"
   ```

3. **Projeyi çalıştır:**
   ```powershell
   dotnet run
   ```

4. **Tarayıcıda aç:**
   - Terminal'de görünen URL'i kopyala
   - Tarayıcıda aç: `http://localhost:5257` veya `https://localhost:7071`

---

## Yöntem 3: Visual Studio ile

### Adımlar:
1. **Visual Studio'yu aç**
2. **Projeyi aç:**
   - File > Open > Project/Solution
   - `InteraktifKredi.Web.csproj` dosyasını seç

3. **Çalıştır:**
   - `F5` tuşuna bas veya
   - Run > Start Debugging

---

## 🔧 SCSS DERLEME (Stil değişikliklerinden sonra)

### SCSS'i CSS'e derle:
```powershell
cd "C:\Users\Baki\Desktop\FinTech\Fintech-134\InteraktifKredi.Web"
sass Styles/main.scss wwwroot/css/site.css
```

**Not:** SCSS değişikliklerinden sonra bu komutu çalıştırman gerekir.

---

## 📝 HIZLI KOMUTLAR

### Projeyi çalıştır:
```powershell
cd "C:\Users\Baki\Desktop\FinTech\Fintech-134\InteraktifKredi.Web"
dotnet run
```

### SCSS derle:
```powershell
cd "C:\Users\Baki\Desktop\FinTech\Fintech-134\InteraktifKredi.Web"
sass Styles/main.scss wwwroot/css/site.css
```

### Projeyi durdur:
- Terminal'de `Ctrl + C` tuşlarına bas

---

## 🌐 TEST URL'LERİ

Proje çalıştıktan sonra şu adresleri test edebilirsin:

- **Ana Sayfa:** `http://localhost:5257/`
- **Dashboard:** `http://localhost:5257/Dashboard`
- **Hizmetler:** `http://localhost:5257/Services`
- **SSS:** `http://localhost:5257/FAQ`
- **Kredi Hesaplama:** `http://localhost:5257/Loan/Apply`
- **Gizlilik Politikası:** `http://localhost:5257/Privacy`
- **Kullanım Koşulları:** `http://localhost:5257/Terms`
- **İletişim:** `http://localhost:5257/Contact`

---

## ⚠️ SORUN GİDERME

### Port zaten kullanılıyor hatası:
```powershell
# Çalışan process'i durdur
Get-Process -Name "InteraktifKredi.Web" | Stop-Process -Force
```

### SCSS derleme hatası:
```powershell
# Sass yüklü mü kontrol et
sass --version

# Yüklü değilse yükle
npm install -g sass
```

### Proje derlenmiyor:
```powershell
# Projeyi temizle ve yeniden derle
dotnet clean
dotnet build
```

