MSLR – Online Referendum Voting System
Module Coursework – Web Application & REST API

-----Project Overview

MSLR (Modern Shangri-La Referendum) is a secure web-based voting system that allows registered voters to participate in online referendums and enables the Election Commission to manage referendums and view results.

This project is developed as part of the coursework submission and implements Task 1 (Web Application) and Task 2 (REST API) as specified.

-----Technology Stack

Language: C#

Framework: ASP.NET Core MVC (.NET 8)

ORM: Entity Framework Core

Database: SQL Server

Frontend: Razor Views + Bootstrap 5

Architecture: MVC + RESTful API

Authentication: Session-based authentication

Password Security: SHA-256 hashing

-----User Roles
1️. Voter

Register using a valid SCC code

Login securely

View available (open) referendums

Vote only once per referendum

Cannot vote again after submission

2️. Election Commission (Admin)

Predefined admin account:

Email: ec@referendum.gov.sr

Password: Shangrilavote&2025@

Create and manage referendums

Open / close referendums

Manage referendum options

View voting results

View winning option per referendum

------Task 1 – Web Application Features
1. Voter Features

Voter registration with SCC validation

Email uniqueness validation

Secure login/logout

Voter dashboard to view open referendums

Vote submission with duplicate-vote prevention

Success and error feedback messages

2. Election Commission Features

Admin login (role-based access)

Admin dashboard with:

Total voters

Total votes cast

Total referendums

Create referendum

Manage referendum options

Open / close referendums

View results and winner per referendum

3. SCC (Secure Citizen Code) Handling

SCC codes are pre-generated

Each SCC:

Exists in the ValidSCC table

Can be used only once

During registration:

SCC is validated against database

SCC is marked as IsUsed = true

Prevents multiple registrations with the same SCC

4. Winner Logic

Votes are grouped by referendum option

Option with highest vote count is declared winner

If a referendum is closed automatically or manually, results remain viewable

Winner is displayed via Admin Dashboard

----- Task 2 – REST API 
1. API Base Path
/mslr

🔹 2.1 Get Referendums by Status

Request

GET /mslr/referendums?status=open
GET /mslr/referendums?status=closed


Response (JSON Example)

{
  "referendums": [
    {
      "referendum_id": 1,
      "status": "open",
      "referendum_title": "Should Shangri-La expand its boundaries?",
      "referendum_desc": "description",
      "referendum_options": [
        { "option_id": 1, "option_text": "Yes", "votes": 15 },
        { "option_id": 2, "option_text": "No", "votes": 30 }
      ]
    }
  ]
}

🔹 2.2 Get Referendum by ID

Request

GET /mslr/referendum/1


Response

{
  "referendum_id": 1,
  "status": "open",
  "referendum_title": "Should Shangri-La expand its boundaries?",
  "referendum_desc": "description",
  "referendum_options": [
    { "option_id": 1, "option_text": "Yes", "votes": 15 },
    { "option_id": 2, "option_text": "No", "votes": 30 }
  ]
}

-----Error Handling

The system provides clear feedback for:

Invalid SCC during registration

SCC already used

Email already registered

Invalid login credentials

Attempting to vote more than once

Unauthorized access to admin pages

-----Database Setup
1️. Create Database
CREATE DATABASE MSLR_DB;

2️. Update Connection String

In appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MSLR_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}

3️. Run Migrations
Add-Migration InitialCreate
Update-Database

📁 Project Structure
MSLR.web
│
├── Controllers
│   ├── AuthController.cs
│   ├── VoteController.cs
│   ├── AdminController.cs
│   └── ApiController.cs
│
├── Models
├── ViewModels
├── Views
│   ├── Auth
│   ├── Vote
│   ├── Admin
│   ├── Home
│   └── Shared
│
├── wwwroot
├── appsettings.json
└── README.md

4. Bonus Features Implemented

Bootstrap responsive UI

Auto-dismiss success/error alerts

Role-based navigation menu

Vote duplication prevention

Winner calculation logic

Clean admin dashboard layout

REST API endpoints

5. Screenshots Included

Login Page

Voter Dashboard

Voting Page

Admin Dashboard

Results Page

-----Coursework Checklist
Section 	                       Sub-task	         Status
Task 1 – Voter                     Register	          A
	                               Login	          A
	                               Voter Page	      A
Task 1 – Election Commission       Login     	      A
	                              Create & Manage
                                  Referendum	      A
	                              Visualise Results	  A
Task 2 – REST API	              2.1              	  A
	                              2.2                 A
Bonus	                          Extra Features   	  A


👤 Student 

Name: Shubham
Course: MSc Advanced Computer Science
University: University of Leicester