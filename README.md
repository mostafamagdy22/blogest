# Blogest

**Blogest** is a modern, scalable blogging platform built with **ASP.NET Core (.NET 9)**. It supports advanced features like authentication, posts, comments, likes, categories, search, background jobs, and more. The project follows **Clean Architecture** principles and is production-ready for **cloud deployment**.

---

## 🚀 Features

- **Authentication & Authorization**  
  JWT, Google OAuth, ASP.NET Identity, and custom policies.

- **Posts & Comments**  
  CRUD operations, pagination, edit history, and moderation.

- **Likes & Saves**  
  Users can like and save posts.

- **Categories**  
  Posts are organized into categories.

- **Search**  
  Full-text search for posts,using Elasticsearch.

- **Background Jobs**  
  Integrated with Hangfire for tasks like image uploads.

- **Media Uploads**  
  Cloudinary integration for image storage.

- **Notifications & Emails**  
  SMTP support for sending reset password emails.

- **Logging & Monitoring**  
  Serilog, Seq, and Elasticsearch for centralized logging.

- **API Versioning & Documentation**  
  Swagger/OpenAPI, with XML comments for every endpoint.

- **Testing**  
  The structure supports unit and integration tests.

---

## 📁 Project Structure


---

## ⚙️ Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- SQL Server (local or cloud)
- Node.js (optional, if frontend is used)
- [Seq](https://datalust.co/seq) (optional, for logging)
- Cloudinary account (for image uploads)

---

## 🔧 Configuration

1- cp .env.example .env:

DB_USER=
DB_PASSWORD=
DB_SERVER=
DB_NAME=

ISSUER=
SECRET=
GOOGLE_CLIENT_ID=
GOOGLE_CLIENT_SECRET=
Cloudinary__CloudName=
Cloudinary__ApiKey=
Cloudinary__ApiSecret=


Smtp__Host=sandbox.smtp.mailtrap.io
Smtp__Port=587
Smtp__UserName=
Smtp__Password=
App__ClientBaseUrl=https://localhost:3000

LOG_PATH=/var/log/blogest

ELASTICSEARCH_PASSWORD=
CERTIFICATE=

2. Update the values for:
   - Database connection strings (for both command and query DBs)
   - Cloudinary credentials
   - SMTP settings
   - Seq / Elasticsearch endpoints (for logging)

---

## 🧩 Database

Run the following command to apply EF Core migrations:

```bash
dotnet ef database update --project blogest.infrastructure


