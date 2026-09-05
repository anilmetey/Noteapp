# 📝 NotTrackApi (Noteapp) — Personal Note Management API

<div align="center">

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC292B?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Secure_Auth-black?style=for-the-badge&logo=jsonwebtokens)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**A secure, modern RESTful Web API and single-page note tracking application built with ASP.NET Core 8, Entity Framework Core, and JWT authentication.**

[Türkçe Dokümantasyon](./README.tr.md)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Tech Stack](#-tech-stack)
- [Architecture & Security](#-architecture--security)
- [API Endpoints](#-api-endpoints)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Developer](#-developer)
- [License](#-license)

---

## 🚀 Overview

**NotTrackApi** is a full-stack note-taking platform. It provides a secure backend REST API with token-based authorization and a built-in interactive single-page web interface hosted directly via ASP.NET Core Static Files (`wwwroot`).

---

## ✨ Key Features

- 🔐 **Authentication & Authorization**:
  - User registration & login with **HMAC-SHA512 salted password hashing**
  - **JWT (JSON Web Token)** bearer token authentication
  - User data isolation (users can only access, modify, and delete their own notes)
- 📝 **Full Note Management (CRUD)**:
  - Create, read, list, and delete personal notes
  - Timestamp tracking (`CreatedAt`, `UpdatedAt`)
- 🔍 **Search & Filtering**:
  - Instant client-side note search and live filtering
- 📤 **Export Capabilities**:
  - Export notes to plain text (`.txt`)
  - Export notes to PDF (`.pdf`) via `jsPDF`
- 🛡️ **Security Hardening**:
  - Custom Security Headers Middleware (`X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `X-XSS-Protection`)
  - HSTS & HTTPS redirection enabled in production
- 📖 **Interactive API Documentation**:
  - Built-in **Swagger / OpenAPI** UI with JWT Bearer authorization support

---

## 🛠 Tech Stack

| Component | Technology |
|---|---|
| **Framework** | ASP.NET Core 8.0 (Web API) |
| **Language** | C# 12 |
| **ORM** | Entity Framework Core 9.0 (Code-First) |
| **Database** | Microsoft SQL Server / LocalDB |
| **Security** | JWT Bearer, HMAC-SHA512 |
| **API Docs** | Swashbuckle / Swagger UI |
| **Frontend** | HTML5, CSS3, JavaScript (ES6+), Bootstrap 5, jsPDF |

---

## 🔌 API Endpoints

### 🔑 Authentication (`/api/Auth`)

| Method | Endpoint | Description | Auth Required |
|---|---|---|:---:|
| `POST` | `/api/Auth/register` | Register a new user | No |
| `POST` | `/api/Auth/login` | Login and receive JWT token | No |
| `GET` | `/api/Auth/me` | Retrieve authenticated user profile | **Yes** (Bearer) |

### 📋 Notes (`/api/Notes`)

| Method | Endpoint | Description | Auth Required |
|---|---|---|:---:|
| `GET` | `/api/Notes` | List all notes belonging to the user | **Yes** (Bearer) |
| `GET` | `/api/Notes/{id}` | Get a specific note by ID | **Yes** (Bearer) |
| `POST` | `/api/Notes` | Create a new note | **Yes** (Bearer) |
| `DELETE` | `/api/Notes/{id}` | Delete a note | **Yes** (Bearer) |

---

## ⚙️ Configuration

Copy `appsettings.example.json` or configure `NotTrackApi/appsettings.json`:

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

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server) or SQL Server Express / LocalDB

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/anilmetey/Noteapp.git
   cd Noteapp
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Apply Database Migrations:**
   ```bash
   dotnet ef database update --project NotTrackApi
   ```

4. **Run the Application:**
   ```bash
   dotnet run --project NotTrackApi
   ```

5. **Access the application:**
   - **Web UI:** `http://localhost:5294/index.html`
   - **Swagger Docs:** `http://localhost:5294/swagger`

---

## 👨‍💻 Developer

<div align="center">

**Anıl Mete**  
Software Developer

[![GitHub](https://img.shields.io/badge/GitHub-anilmetey-181717?style=for-the-badge&logo=github)](https://github.com/anilmetey)

</div>

---

## 📄 License

This project is licensed under the [MIT License](./LICENSE) — see the LICENSE file for details.
