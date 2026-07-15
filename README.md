# MiniBlog

A simple blogging application built with ASP.NET Core (Razor Pages + Web API) to demonstrate .NET development skills and modern backend development practices.

### Features

- User registration and login with JWT Authentication
- Browse blog posts with search and pagination
- Create new posts (authenticated users only)
- View post details
- Add comments to posts

### Architecture

This project follows a Clean (Layered) Architecture, separating responsibilities into distinct layers to improve maintainability, scalability, and testability.

### Typical layers include:

- Presentation – Razor Pages & Web API
- Application – Business logic and use cases
- Domain – Core entities and business rules
- Infrastructure – Database, authentication, and external services

### Technologies

- ASP.NET Core
- Razor Pages
- ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- SQL Server

### Purpose

The goal of this project is to showcase practical backend development skills, including authentication, CRUD operations, layered architecture, and clean code principles.