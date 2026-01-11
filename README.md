# Net9ApiOdev Starter

Bu proje, .NET 9 tabanlı, JWT kimlik doğrulamalı ve katmanlı mimariye sahip bir REST API başlangıç kitidir.

Daha fazla bilgi için [Ödev Dokümantasyonu](Net9_API_Odev_Dokumantasyonu.docx)'na bakabilirsiniz.

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