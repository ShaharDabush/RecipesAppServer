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
IsAdmin Bit,
--RecipesMade int Not Null,
--RateAmout int Not Null,
--LookRecipesAmout int Not Null,
)

Create Table Recipes

(
Id int Primary Key Identity,
RecipesName nvarchar(100) Not Null,
RecipeDescription nvarchar(500) Not Null,
RecipeImage nvarchar(30) Not Null,
MadeBy int Foreign Key References Users(Id) Not Null,
Rating int Not Null,
IsKosher Bit Not Null,
IsGloten Bit Not Null,
HowManyMadeIt int Not Null,
ContainsMeat Bit Not Null,
ContainsDairy Bit Not Null,
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
IsKosher Bit Not Null,
IsGloten Bit Not Null,
IsMeat Bit Not Null,
IsDairy Bit Not Null,
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
 RecipeId int Foreign Key References Recipes(Id) Not Null,
 Amount int Not Null,
 MeasureUnits nvarchar(20) Not Null,
)

Create Table IngredientStorage
(
 IngredientId int Foreign Key References Ingredients(Id) Not Null,
 StorageId int Foreign Key References Storage(Id) Not Null,
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
 TextLevel nvarchar(500) Not Null,
 LevelCount int Not Null,
 RecipeId int Foreign Key References Recipes(Id) Not Null,
)

Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('admin', 'kuku@kuku.com', '1234','Image',1)
Go
Insert Into Storage Values('ManegerStorage','ABCDE',1)
Go
Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('NormalUser', 'N@U.com', '123','Image',0)
Go
Insert Into Storage Values('UserStorage','FGHIJ',2)
Go
Insert Into Recipes Values ('Chocolate Chip Cookies', 'Chocolate Chip Cookies','chocolatechipcookies.png',1,0,1,1,0,0,1,'Any time')
Go
Insert Into Recipes Values ('Omelette Recipe', 'This easy, fail-proof omelette recipe is truly the best. With a variety of filling suggestions, you will learn how to make an omelet at home that’s even more delicious than your favorite breakfast restaurant (but so simple)!','omeletterecipe.png',1,3,0,0,3,1,1,'Any time')
Go
Insert Into Recipes Values ('Apple Pie', 'This homemade apple pie recipe is the best I’ve ever made! With a golden, flaky pie crust filled with the most delicious, perfectly spiced apple filling, your search for the perfect apple pie is over.','applepie.png',1,0,1,1,0,0,1,'Any time')
Go
Insert Into Recipes Values ('steak', 'This Pan-Seared Steak has a garlic rosemary-infused butter that makes it taste steakhouse quality. You’ll be impressed at how easy it is to make the perfect steak – seared and caramelized on the outside, and so juicy inside. Thank you to  on behalf of the Beef Checkoff for sponsoring this garlic butter steak recipe. I received compensation, but all opinions are my own.','steak.png',1,0,0,0,0,1,1,'Evening')
Go
Insert Into Levels Values ('The first step in making these easy chocolate chip cookies to to combine the dry ingredients in a medium size bowl.',1,1)
Go
Insert Into Levels Values ('Next, cream together butter and sugars, make sure to soften the butter early by taking it out of the fridge at least two hours before baking so it’s ready to go when you need it. You can also warm it in the microwave for about 7 seconds, but be very careful not to melt it.',2,1)
Go
Insert Into Levels Values ('Once butter/sugar mixture is beaten well, add the eggs & vanilla and beat to combine.',3,1)
Go
Insert Into Levels Values ('Add dry ingredients and stir until just combined. Then add the chocolate chips and beat until they are evenly distributed throughout the dough.',4,1)
Go
Insert Into Levels Values ('Use LOTS of chocolate chips. You want at least two gooey chocolate chips in every bite.',5,1)
Go
Insert Into Levels Values ('The chocolate chip cookie dough should be easy to roll and not sticky. It should not be dry or crumbly.',6,1)
Go
Insert Into Levels Values ('Once the cookie dough is finished, it’s time to portion and roll the dough. I know many people eyeball it when making cookies, however I highly recommend using a cookie scoop.',7,1)
Go
Insert Into Levels Values ('Once the cookie dough is finished, it’s time to portion and roll the dough. I know many people eyeball it when making cookies, however I highly recommend using a cookie scoop.',8,1)
Go
Insert Into Levels Values ('simply roll them into balls, place them evenly apart on a baking sheet (about 1.5 to 2 inches apart) and bake at 375 degrees for 8-10 minutes.',9,1)
Go
Insert Into Levels Values ('Cool the chocolate chip cookies on the baking sheet for 5 minutes before removing to a wire rack to cool completely (or just eat them warm while the chocolate chips are melty)!',10,1)
Go
alter Table Users Add StorageId int null Foreign Key References Storage(Id)
Go
UPDATE Users SET StorageId = 1 WHERE id = 1;
UPDATE Users SET StorageId = 2 WHERE id = 2;
 Go

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
Go
select * from Storage
Go
select * from Recipes
Go
select * from Levels
Go
