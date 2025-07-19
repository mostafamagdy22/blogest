Blogest
Blogest is a modern, scalable blogging platform built with ASP.NET Core (.NET 9), supporting advanced features like authentication, posts, comments, likes, categories, search, background jobs, and more. The project follows clean architecture principles and is ready for production and cloud deployment.

Features
Authentication & Authorization: JWT, Google OAuth, ASP.NET Identity, custom policies.
Posts & Comments: CRUD operations, pagination, edit history, and moderation.
Likes & Saves: Users can like and save posts.
Categories: Organize posts by categories.
Search: Full-text search for posts, comments, and users.
Background Jobs: Hangfire integration for tasks like image uploads.
Media Uploads: Cloudinary integration for image storage.
Notifications & Emails: SMTP support for transactional emails.
Logging & Monitoring: Serilog, Seq, and Elasticsearch integration.
API Versioning & Documentation: Swagger/OpenAPI, XML comments for all endpoints.
Unit Testing: Project structure ready for unit and integration tests.
Project Structure

blogest.slnblogest.api/           # ASP.NET Core Web API (main entry point)blogest.application/   # Application layer (business logic, CQRS, MediatR)blogest.domain/        # Domain layer (entities, value objects, constants)blogest.infrastructure/# Infrastructure (EF Core, Identity, Cloudinary, Hangfire, Serilog, etc.)unittests/             # Unit and integration testsvscDB/                 # Database scripts and migrations
Getting Started
Prerequisites
.NET 9 SDK
SQL Server (local or cloud)
Node.js (if you want to run a frontend)
Seq (for logging, optional)
Cloudinary account (for image uploads)
Configuration
Copy .env and appsettings.json files and update with your credentials (DB, Cloudinary, SMTP, etc).
Set up connection strings for both command and query databases.
Configure logging endpoints (Seq, Elasticsearch) as needed.
Database
Run EF Core migrations to set up the database:

dotnet ef database update --project blogest.infrastructure
Running the API
Start the API:

dotnet run --project blogest.api
The API will be available at https://localhost:5294 (or your configured port).
Background Jobs
Hangfire dashboard is available at /hangfire endpoint.
API Documentation
Swagger UI is available at /swagger.
All endpoints are documented with XML comments.
Testing
Unit tests are located in the unittests folder.
Run tests with:

dotnet test
Contributing
Contributions are welcome! Please open issues or submit pull requests for improvements, bug fixes, or new features.

License
This project is licensed under the MIT License.

If you want this README saved as README.md in your repo, let me know!

