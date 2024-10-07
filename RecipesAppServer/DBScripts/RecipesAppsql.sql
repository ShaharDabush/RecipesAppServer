Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'RecipesAppDB')
BEGIN
    DROP DATABASE RecipesAppDB;
END
Go
Create Database RecipesAppDB
Go
Use RecipesAppDB
Go
Create Table AppUsers
(
	Id int Primary Key Identity,
	UserName nvarchar(50) Not Null,
	UserLastName nvarchar(50) Not Null,
	UserEmail nvarchar(50) Unique Not Null,
	UserPassword nvarchar(50) Not Null,
	IsManager bit Not Null Default 0
)
Insert Into AppUsers Values('admin', 'admin', 'kuku@kuku.com', '1234', 1)
Go
-- Create a login for the admin user
CREATE LOGIN [RecipesAppAdminLogin] WITH PASSWORD = 'thePassword';
Go

-- Create a user in the YourProjectNameDB database for the login
CREATE USER [RecipesAppAdminUser] FOR LOGIN [RecipesAppAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [RecipesAppAdminUser];
Go

-- scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=RecipesAppDB;User ID=RecipesAppAdminLogin;Password=thePassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context RecipesAppDbContext -DataAnnotations –force
