# 📥 MAIN BRANCH ÇEKME REHBERİ

## 🔄 ADIM ADIM İŞLEM

### 1. Mevcut Değişiklikleri Kaydet

**Seçenek A: Commit Et (Önerilen)**
```powershell
cd "C:\Users\Baki\Desktop\FinTech\Fintech-134"
git add .
git commit -m "feat: Şemaya göre header ve footer linkleri güncellendi"
```

**Seçenek B: Stash Et (Geçici sakla)**
```powershell
cd "C:\Users\Baki\Desktop\FinTech\Fintech-134"
git stash push -m "Şema güncellemeleri - geçici"
```

### 2. Main Branch'ine Geç
```powershell
git checkout main
```

### 3. Main'i Güncelle (Pull)
```powershell
git pull origin main
```

### 4. Feature Branch'ine Geri Dön
```powershell
git checkout feature/dashboard-ui
```

### 5. Main'deki Değişiklikleri Merge Et
```powershell
git merge main
```

**Eğer conflict varsa:**
- Conflict'leri çöz
- `git add .`
- `git commit`

---

## ⚠️ ÖNEMLİ NOTLAR

- **Commit etmeden önce:** Tüm değişikliklerinizin kaydedildiğinden emin olun
- **Merge conflict:** Eğer main'de değişiklikler varsa conflict çıkabilir
- **Backup:** Önemli değişikliklerinizi yedekleyin

---

## 🚀 HIZLI YÖNTEM (Tek Komut)

```powershell
cd "C:\Users\Baki\Desktop\FinTech\Fintech-134"
git add .
git commit -m "feat: Şemaya göre header ve footer güncellemeleri"
git checkout main
git pull origin main
git checkout feature/dashboard-ui
git merge main
```

