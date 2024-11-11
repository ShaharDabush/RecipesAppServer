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

Create Table Users

(
Id int Primary Key Identity,
UserName nvarchar(50) Not Null,
Email nvarchar(50) Unique Not Null,
UserPassword nvarchar(50) Not Null,
UserImage nvarchar(30),
IsAdmin int,
--RecipesMade int Not Null,
--RateAmout int Not Null,
--LookRecipesAmout int Not Null,
)

Create Table Recipes

(
Id int Primary Key Identity,
RecipesName nvarchar(100) Not Null,
RecipeDescription nvarchar(100) Not Null,
RecipeImage nvarchar(30) Not Null,
MadeBy int Foreign Key References Users(Id) Not Null,
Rating int Not Null,
IsKosher nvarchar(5) Not Null,
IsGloten nvarchar(5) Not Null,
TimeOfDay nvarchar(20) Not Null,
)
Create Table Kind
(
Id int Primary Key Identity,
KindName nvarchar(100) Not Null,
)

Create Table Ingredients

(
Id int Primary Key Identity,
IngredientName nvarchar(100) Not Null,
IngredientImage nvarchar(30) Not Null,
KindId int Foreign Key References Kind(Id) Not Null,
MeatOrDariy nvarchar(20) Not Null,
IsKosher nvarchar(5) Not Null,
IsGloten nvarchar(5) Not Null,
Barkod nvarchar(200) Not Null,
)

Create Table Storage
(
 Id int Primary Key Identity,
 StorageName nvarchar(30) Not Null,
 StorageCode nvarchar(5) Not Null,
 Manager int Foreign Key References Users(Id) Not Null,
)

Create Table IngredientRecipe
(
 IngredientId int Foreign Key References Ingredients(Id) Not Null,
 StorageId int Foreign Key References Storage(Id) Not Null,
)

Create Table IngredientStorage
(
 IngredientId int Foreign Key References Ingredients(Id) Not Null,
 RecipeId int Foreign Key References Recipes(Id) Not Null,
 Amount int Not Null,
 MeasureUnits nvarchar(20) Not Null,
)

Create Table Barkod 
(
 Id int Primary Key Identity,
 BarkodImage nvarchar(30) Not Null,
 IngredientId int Foreign Key References Ingredients(Id) Not Null,
 IngredientImage nvarchar(30) Not Null,
)

Create Table Allergy 
(
 Id int Primary Key Identity,
 AllergyName nvarchar(50) Not Null,
)

Create Table AllergyUser
(
  UserId int Foreign Key References Users(Id) Not Null,
  AllergyId int Foreign Key References Allergy(Id) Not Null,
)

Create Table Comments
(
 Id int Primary Key Identity,
 Comment nvarchar(300) Not Null,
 UserId int Foreign Key References Users(Id) Not Null,
 RecipeId int Foreign Key References Recipes(Id) Not Null,
)

Create Table Rating
(
 Id int Primary Key Identity,
 Rate int Not Null,
 UserId int Foreign Key References Users(Id) Not Null,
 RecipeId int Foreign Key References Recipes(Id) Not Null,
)

Create Table Levels
(
 Id int Primary Key Identity,
 TextLevel nvarchar(300) Not Null,
 LevelCount int Not Null,
 RecipeId int Foreign Key References Recipes(Id) Not Null,
)

Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('admin', 'kuku@kuku.com', '1234','Image',1)
Go
Insert Into Storage Values('ManegerStorage','ABCDE',1)
Go
alter Table Users Add StorageId int null Foreign Key References Storage(Id)

 

--If EXISTS (Select * From Users where UserName = N'admin') Drop User [admin]
--Create Table AppUsers
--(
--	Id int Primary Key Identity,
--	UserName nvarchar(50) Not Null,
--	UserLastName nvarchar(50) Not Null,
--	UserEmail nvarchar(50) Unique Not Null,
--	UserPassword nvarchar(50) Not Null,
--	IsManager bit Not Null Default 0
--)



-- Create a login for the admin user
CREATE LOGIN [RecipesAppAdminLogin] WITH PASSWORD = 'pass';
Go
--select * from Users
-- Create a User in the YourProjectNameDB database for the login
CREATE USER [RecipesAppAdminUser] FOR LOGIN [RecipesAppAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [RecipesAppAdminUser];
--Go

--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=RecipesAppDB;User ID=RecipesAppAdminLogin;Password=pass;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context RecipesAppDbContext -DataAnnotations –force

--Select suser_sid ('admin')


--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=RecipesAppDB;User ID=RecipesAppAdminLogin;Password=pass;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context RecipesAppDbContext -DataAnnotations –force
select * from Users
select * from Storage
