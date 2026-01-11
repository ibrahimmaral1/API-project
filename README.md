# Net9ApiOdev Starter

Bu proje, .NET 9 kullanılarak geliştirilmiş, Katmanlı Mimari (N-Tier) prensiplerine uygun, JWT tabanlı güvenlik ve gelişmiş veritabanı özelliklerine sahip bir RESTful API uygulamasıdır.

## 🚀 Öne Çıkan Özellikler 
- **Framework:** .NET 9 & Minimal API
- **Güvenlik:** JWT Auth & Rol Tabanlı Yetkilendirme (Admin/User) 🛡️
- **Veritabanı:** SQLite & EF Core (Soft Delete & Seed Data sistemi) 💾
- **Standartlar:** Generic Response Wrapper & DTO Kullanımı
- **Validasyon:** FluentValidation entegrasyonu

## 🏗️ Mimari Yapı (N-Tier Architecture)
Proje, sorumlulukların ayrılması ilkesine göre 3 ana katmandan oluşur:
- **Data (Repository):** DB Context, Migrations ve Veri erişim katmanı.
- **Service:** İş mantığının (Business Logic) yürütüldüğü katman.
- **API (Controllers):** Minimal API endpoint'lerinin ve DTO eşlemelerinin yapıldığı yer.

## 📡 API Endpoint Listesi & Yetkiler

| Metot | Endpoint | Açıklama | Yetki |
| :--- | :--- | :--- | :--- |
| POST | `/users` | Yeni Kullanıcı Kaydı | **Herkes** |
| POST | `/auth/login` | Giriş ve Token Alımı | **Herkes** |
| GET | `/users` | Tüm Kullanıcıları Listele | 🔒 Admin |
| GET | `/users/{id}` | ID ile Kullanıcı Getir | 🔒 User/Admin |
| DELETE| `/users/{id}` | Kullanıcıyı Sil (Soft Delete) | 🔒 Admin |

## 📝 API Yanıt Örneği (Standard Response)
Tüm istekler ödevde istendiği gibi şu formatta döner:
```json
{
  "success": true,
  "message": "İşlem başarıyla tamamlandı.",
  "data": { "id": 1, "username": "admin", "role": "Admin" }
}

## Kurulum (Setup)

Gerekli bağımlılıkların yüklendiğinden emin olun:

```bash
# Projeyi klonlayın
git clone [https://github.com/KULLANICI_ADIN/REPO_ADIN.git](https://github.com/KULLANICI_ADIN/REPO_ADIN.git)

# Proje klasörüne girin
cd Net9ApiOdev

# Paketleri yükleyin (Restore)
dotnet restore

##Veritabanı Hazırlığı (Database)

# Migration'ı uygulayın ve veritabanını oluşturun
dotnet ef database update

##Geliştirme Sunucusu (Development Server)

# API'yi başlatın (http://localhost:5000)
dotnet run

##Üretim (Production)

# Release modunda derleyin
dotnet build --configuration Release
