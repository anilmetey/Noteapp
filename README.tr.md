# 📝 NotTrackApi (Noteapp) — Kişisel Not Yönetim Web API

<div align="center">

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC292B?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Güvenli_Kimlik-black?style=for-the-badge&logo=jsonwebtokens)
![Lisans](https://img.shields.io/badge/Lisans-MIT-green?style=for-the-badge)

**ASP.NET Core 8, Entity Framework Core 9 ve JWT kimlik doğrulaması ile geliştirilmiş güvenli, modern RESTful Web API ve tek sayfa (SPA) not takip uygulaması.**

[English README](./README.md)

</div>

---

## 📋 İçindekiler

- [Genel Bakış](#-genel-bakış)
- [Öne Çıkan Özellikler](#-öne-çıkan-özellikler)
- [Teknoloji Yığını](#-teknoloji-yığını)
- [API Uç Noktaları](#-api-uç-noktaları)
- [Yapılandırma](#-yapılandırma)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [Geliştirici](#-geliştirici)
- [Lisans](#-lisans)

---

## 🚀 Genel Bakış

**NotTrackApi**, kullanıcıların notlarını güvenle kaydedebildiği, listeleyebildiği, arayabildiği ve dışa aktarabildiği tam kapsamlı bir not takip platformudur. Token tabanlı yetkilendirme altyapısına sahiptir ve `wwwroot` üzerinden doğrudan sunulan modern bir Bootstrap arayüzü barındırır.

---

## ✨ Öne Çıkan Özellikler

- 🔐 **Güvenli Kimlik Doğrulama & Yetkilendirme**:
  - Kullanıcı kaydı ve girişi: **HMAC-SHA512 tuzlanmış (salted) şifre hashleme**
  - **JWT (JSON Web Token)** Bearer doğrulama mimarisi
  - Kullanıcı veri izolasyonu: Her kullanıcı yalnızca kendi notlarına erişebilir, düzenleyebilir ve silebilir
- 📝 **Eksiksiz Not Yönetimi (CRUD)**:
  - Not oluşturma, okuma, listeleme ve silme
  - Zaman damgaları takibi (`CreatedAt`, `UpdatedAt`)
- 🔍 **Arama ve Canlı Filtreleme**:
  - Arayüz üzerinden notlar arasında anlık arama ve filtreleme
- 📤 **Dışa Aktarma (Export)**:
  - Notları metin belgesi (`.txt`) olarak indirme
  - Notları `jsPDF` kütüphanesiyle PDF (`.pdf`) formatında dışa aktarma
- 🛡️ **Güvenlik Başlıkları (Security Headers)**:
  - Özel Middleware: `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `X-XSS-Protection`
  - Production ortamında HSTS ve HTTPS yönlendirmesi
- 📖 **Etkileşimli API Dokümantasyonu**:
  - JWT Bearer yetkilendirme destekli **Swagger UI** entegrasyonu

---

## 🛠 Teknoloji Yığını

| Bileşen | Teknoloji |
|---|---|
| **Framework** | ASP.NET Core 8.0 (Web API) |
| **Dil** | C# 12 |
| **ORM** | Entity Framework Core 9.0 (Code-First) |
| **Veritabanı** | Microsoft SQL Server / LocalDB |
| **Güvenlik** | JWT Bearer, HMAC-SHA512 |
| **API Dokümantasyonu** | Swashbuckle / Swagger UI |
| **Arayüz** | HTML5, CSS3, JavaScript (ES6+), Bootstrap 5, jsPDF |

---

## 🔌 API Uç Noktaları

### 🔑 Kimlik Doğrulama (`/api/Auth`)

| Metot | Uç Nokta | Açıklama | Yetki Gerekli mi? |
|---|---|---|:---:|
| `POST` | `/api/Auth/register` | Yeni kullanıcı kaydı | Hayır |
| `POST` | `/api/Auth/login` | Giriş yap ve JWT token al | Hayır |
| `GET` | `/api/Auth/me` | Giriş yapmış kullanıcının bilgisi | **Evet** (Bearer) |

### 📋 Notlar (`/api/Notes`)

| Metot | Uç Nokta | Açıklama | Yetki Gerekli mi? |
|---|---|---|:---:|
| `GET` | `/api/Notes` | Kullanıcıya ait tüm notları listele | **Evet** (Bearer) |
| `GET` | `/api/Notes/{id}` | Belirli bir notu getir | **Evet** (Bearer) |
| `POST` | `/api/Notes` | Yeni not ekle | **Evet** (Bearer) |
| `DELETE` | `/api/Notes/{id}` | Belirli bir notu sil | **Evet** (Bearer) |

---

## ⚙️ Yapılandırma

`NotTrackApi/appsettings.json` dosyasını yapılandırın:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyMustBeAtLeast32BytesLong12345!",
    "Issuer": "NotDefterimApi",
    "Audience": "NotDefterimClient"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotTrackDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server) veya SQL Server Express / LocalDB

### Adım Adım Çalıştırma

1. **Repoyu klonlayın:**
   ```bash
   git clone https://github.com/anilmetey/Noteapp.git
   cd Noteapp
   ```

2. **Bağımlılıkları yükleyin:**
   ```bash
   dotnet restore
   ```

3. **Veritabanı Migration'larını Uygulayın:**
   ```bash
   dotnet ef database update --project NotTrackApi
   ```

4. **Uygulamayı Başlatın:**
   ```bash
   dotnet run --project NotTrackApi
   ```

5. **Arayüze Erişin:**
   - **Web Arayüzü:** `http://localhost:5294/index.html`
   - **Swagger Dokümantasyonu:** `http://localhost:5294/swagger`

---

## 👨‍💻 Geliştirici

<div align="center">

**Anıl Mete**  
Yazılım Geliştirici

[![GitHub](https://img.shields.io/badge/GitHub-anilmetey-181717?style=for-the-badge&logo=github)](https://github.com/anilmetey)

</div>

---

## 📄 Lisans

Bu proje [MIT Lisansı](./LICENSE) kapsamında lisanslanmıştır.
