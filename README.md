# 🗳️ MSLR – Online Referendum Voting System

A secure web-based voting platform built with **ASP.NET Core MVC** and a **RESTful API**, enabling online referendums with role-based access for voters and administrators.

---

## 🚀 What This Project Does

- Allows registered voters to securely participate in online referendums
- Enables administrators to create, manage, and close referendums
- Prevents duplicate voting and ensures data integrity
- Exposes REST API endpoints for retrieving referendum data

---

## 🛠 Tech Stack

- **Backend:** ASP.NET Core MVC (.NET 8), C#
- **API:** RESTful Web API
- **Database:** SQL Server, Entity Framework Core
- **Frontend:** Razor Views, Bootstrap 5
- **Architecture:** MVC + REST
- **Security:** Session-based authentication, SHA-256 password hashing

---

## 👥 Roles & Features

### 👤 Voter
- Secure registration with validation
- View open referendums
- Vote once per referendum (enforced at database & logic level)

### 🏛️ Admin
- Create and manage referendums
- Open / close voting
- View real-time results and winning options

---

## 🔌 REST API (Example)

```http
GET /mslr/referendums?status=open
GET /mslr/referendum/{id}
```
---

##⭐ Key Highlights

- Role-based access control
- Duplicate vote prevention
- Clean MVC architecture
- REST API design and JSON responses
- Production-style error handling
- Database-driven winner calculation

---
##📁 Project Structure
```text

Controllers | Models | ViewModels | Views | wwwroot
```
---
##👨‍💻 Author

Shubham Hariyale
📧 shubham.azure11@gmail.com

🔗 LinkedIn: https://www.linkedin.com/in/shubham-hariyale

---
