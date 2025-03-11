# TaskManagementAPI

## Overview

Task Management API is a .NET Core-based application for managing tasks, including functionalities like adding and updating tasks. 
It interacts with a SQL Server 2019 database for persistent data storage and includes RESTful API endpoints for integration with client applications.

## Features

- Create tasks with attributes like name, description, due date, start date, end date, priority, and status.
- Update tasks using unique IDs.
- Validates tasks with specific business rules.
- Swagger support for API documentation.

---

## Prerequisites

Ensure the following are installed:

1. **.NET Core SDK 6.0 or higher**
2. **Microsoft SQL Server 2019**
3. **SQL Server Management Studio (SSMS)** for database setup.
4. **Visual Studio** (for development).

---

## Setting Up the Application

### 1. Clone the Repository

```bash
git clone <repository-url>
cd TaskManagementAPI
```

---

### 2. Configure the Database

Ensure **SQL Server 2019** is installed and running on your system. Use SQL Server Management Studio (SSMS) to create the database and set up the schema.

#### Create Database
Run the following command in SSMS to create the database:

```sql
CREATE DATABASE TaskManagementDb;
```

#### Create Table:
```sql
USE TaskManagementDb;

CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DueDate DATETIME NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NULL,
    Priority NVARCHAR(10) NOT NULL,
    Status NVARCHAR(15) NOT NULL
);
```

#### Stored Procedures:
**Add Task:**
```sql
CREATE PROCEDURE sp_AddTask
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME,
    @StartDate DATETIME,
    @Priority NVARCHAR(10),
    @Status NVARCHAR(15)
AS
BEGIN
    INSERT INTO Tasks (Name, Description, DueDate, StartDate, Priority, Status)
    VALUES (@Name, @Description, @DueDate, @StartDate, @Priority, @Status);
END;
```

**Update Task:**
```sql
CREATE PROCEDURE sp_UpdateTask
    @Id INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME,
    @StartDate DATETIME,
    @EndDate DATETIME,
    @Priority NVARCHAR(10),
    @Status NVARCHAR(15)
AS
BEGIN
    UPDATE Tasks
    SET Name = @Name,
        Description = @Description,
        DueDate = @DueDate,
        StartDate = @StartDate,
        EndDate = @EndDate,
        Priority = @Priority,
        Status = @Status
    WHERE Id = @Id;
END;
```

---

### 3. Configure the Application

Update the `appsettings.json` file with your database connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=<YOUR_SERVER>;Initial Catalog=TaskManagementDb;Integrated Security=True;TrustServerCertificate=True"
}
```

Replace `<YOUR_SERVER>` with your SQL Server 2019 instance name.

---

### 4. Build and Run the Application

#### Using Visual Studio:

1. Open the project in Visual Studio.
2. Restore dependencies using **`nuget`**:
   ```bash
   dotnet restore
   ```
3. Set `TaskManagementAPI` as the startup project.
4. Press **F5** or click **Start** to run the application.

#### Using Command Line:

```bash
dotnet build
dotnet run
```

---

## API Endpoints

Swagger is available at:

```
https://localhost:7251/swagger
```

### Add Task (POST `/api/Tasks`)

**Request Body:**
```json
{
  "Name": "Sample Task",
  "Description": "Task description",
  "DueDate": "2025-01-20",
  "StartDate": "2025-01-15",
  "Priority": "High",
  "Status": "New"
}
```

### Update Task (PUT `/api/Tasks/{id}`)

**Request Body:**
```json
{
  "Name": "Updated Task",
  "Description": "Updated description",
  "DueDate": "2025-01-25",
  "StartDate": "2025-01-20",
  "EndDate": "2025-01-25",
  "Priority": "Medium",
  "Status": "In Progress"
}
```

---

## Additional Details

- **Validation Rules:**
  - `DueDate` must not be in the past, on weekends, or holidays.
  - Maximum of 100 high-priority tasks with the same due date.
- Uses `Dapper` for database operations.

---

## Troubleshooting

- Ensure SQL Server is running and accessible.
- Verify database configuration in `appsettings.json`.
- Check Swagger for valid API requests and responses.

---

## License

This project is licensed under the MIT License.
