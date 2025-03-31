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
UserImage nvarchar(1500),
IsAdmin Bit,
--RecipesMade int Not Null,
--RateAmout int Not Null,
--LookRecipesAmout int Not Null,
)

Create Table Recipes

(
Id int Primary Key Identity,
RecipesName nvarchar(100) Not Null,
RecipeDescription nvarchar(1000) Not Null,
RecipeImage nvarchar(1500) Not Null,
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
IngredientImage nvarchar(1500) Not Null,
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
 primary key (IngredientId, RecipeId)
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

Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('admin', 'kuku@kuku.com', '1234','maritest.png',1)
Go
Insert Into Storage Values('ManegerStorage','ABCDE',1)
Go
Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('NormalUser', 'N@U.com', '123','Image',0)
Go
Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('NormalUserManeger', 'N@UM.com', '1235','Image5',0)
Go
Insert Into Storage Values('UserStorage','FGHIJ',3)
Go
Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('NormalUserWithSameStorageAsAdmin', 'N@U2.com', '7890','Image2',0)
Go
Insert Into Users (UserName, Email, UserPassword, UserImage,IsAdmin) Values('NormalUserWithSameStorageAsAdmin2', 'N@U22.com', '789','Image22',0)
Go
Insert Into Kind Values ('Eggs')
Go
Insert Into Kind Values ('Butter')
Go
Insert Into Kind Values ('Flour')
Go
Insert Into Kind Values ('Sugar')
Go
Insert Into Kind Values ('Olive oil')
Go
Insert Into Ingredients Values('Eggs','egg.png',1,1,0,0,0,'Barkod')
Go
Insert Into Ingredients Values('Butter','butter.png',2,1,0,0,1,'Barkod2')
Go
Insert Into Ingredients Values('Flour','flour.png',3,1,1,0,0,'Barkod3')
Go
Insert Into Ingredients Values('Sugar','sugar.png',4,1,0,0,0,'Barkod4')
Go
Insert Into Recipes Values ('Chocolate Chip Cookies', 'Chocolate Chip Cookies','chocolatechipcookies.png',1,0,1,1,0,0,1,'Any time')
Go
Insert Into Recipes Values ('Omelette', 'This easy, fail-proof omelette recipe is truly the best. With a variety of filling suggestions, you will learn how to make an omelet at home that’s even more delicious than your favorite breakfast restaurant (but so simple)!','omeletterecipe.png',1,3,0,0,3,1,1,'Any time')
Go
Insert Into Recipes Values ('Apple Pie', 'This homemade apple pie recipe is the best I’ve ever made! With a golden, flaky pie crust filled with the most delicious, perfectly spiced apple filling, your search for the perfect apple pie is over.','applepie.png',1,0,1,1,0,0,1,'Any time')
Go
Insert Into Recipes Values ('steak', 'This Pan-Seared Steak has a garlic rosemary-infused butter that makes it taste steakhouse quality. You’ll be impressed at how easy it is to make the perfect steak – seared and caramelized on the outside, and so juicy inside. Thank you to  on behalf of the Beef Checkoff for sponsoring this garlic butter steak recipe. I received compensation, but all opinions are my own.','steak.png',1,0,0,0,0,1,1,'Evening')
Go
Insert Into Recipes Values ('Macarons', 'Indulging in dessert is a sweet pleasure; whether its cookies, cake, or pie, each bite is an escape. But next time you crave something sweet, consider macarons. These delicate sandwich cookies elevate any occasion with their alluring appearance, crisp exteriors, and chewy, nougat-like texture. They may seem too challenging to make, but with our step-by-step instructions, youll master the art of macarons in no time. The magic lies in their preparation; without chemical leaveners, these cookies rely solely on properly beaten egg whites for their airy lift. To make them your own, add a splash of food coloring for a vibrant array of hues, or infuse the icing with extract for a flavorful twist.','macarons.png',2,10,1,1,50,0,1,'Any time')
Go
Insert Into Recipes Values ('Ramen', 'Take the usual ramen up a notch with this quick homemade ramen. Fresh veggies and herbs make this extra delicious, healthy, and cozy!','ramen.png',1,5,1,1,4,1,0,'Any time')
Go
Insert Into Recipes Values ('Alfajores', 'These alfajores (also called dulce de leche cookies) are buttery and sweet with a touch of vanilla. They are to die for! Given to me by a chef who sweet-talked the recipe out of a street vendor in Peru.','alfajores.png',2,2,1,1,8,0,1,'Any time')
Go
Insert Into Recipes Values ('Pasta Rose', 'This pasta is absolutely without a doubt my signature dish! So good for any day, but it has a naughty weekend vibe - I’d prob pay $100 for it during a hangover. Tell me if you try it and love it as much as I do!','pastarose.png',1,8,1,1,5,0,1,'Evening')
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
Insert Into Levels Values ('Mix flour, 1/2 cup confectioners sugar, cornstarch, and salt together in a bowl.',1,7)
Go
Insert Into Levels Values ('Beat butter with an electric mixer in a separate bowl until soft and fluffy. Mix in white sugar and vanilla until well combined.',2,7)
Go
Insert Into Levels Values ('Add dry ingredients to wet ingredients in 3 batches, beating until just blended after each addition. Divide dough in half, roll each half into a log, and refrigerate until firm, about 2 hours.',3,7)
Go
Insert Into Levels Values ('Preheat the oven to 350 degrees F (175 degrees C).',4,7)
Go
Insert Into Levels Values ('Slice dough logs into forty-eight 1/4-inch-thick cookies and place onto ungreased baking sheets.',5,7)
Go
Insert Into Levels Values ('Bake in the preheated oven until just starting to turn golden around the edges, 8 to 10 minutes.',6,7)
Go
Insert Into Levels Values ('Remove from the oven and let cool for 1 minute on the baking sheet. Transfer cookies to wire racks to cool completely, 25 to 30 minutes.',7,7)
Go
Insert Into Levels Values ('Use a knife to generously spread dulce de leche onto 1/2 of the cooled cookies. Sandwich remaining cookies on top and place onto a serving tray.',8,7)
Go
Insert Into Levels Values ('Lightly dust finished cookies with remaining confectioners sugar.',9,7)
Go
Insert Into IngredientRecipe Values(1,2,3,'units')
Go
Insert Into IngredientRecipe Values(1,1,2,'units')
Go
Insert Into IngredientRecipe Values(2,1,150,'gr')
Go
Insert Into IngredientRecipe Values(2,4,50,'gr')
Go
Insert Into IngredientRecipe Values(2,7,200,'gr')
Go
Insert Into IngredientRecipe Values(3,7,240,'gr')
Go
Insert Into IngredientRecipe Values(4,7,50,'gr')
Go
alter Table Users Add StorageId int null Foreign Key References Storage(Id)
Go
UPDATE Users SET StorageId = 1 WHERE id = 1;
UPDATE Users SET StorageId = 2 WHERE id = 2;
UPDATE Users SET StorageId = 2 WHERE id = 3;
UPDATE Users SET StorageId = 1 WHERE id = 4;
UPDATE Users SET StorageId = 1 WHERE id = 5;
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
select * from Ingredients
Go
select * from Kind
Go
select * from IngredientRecipe
Go  
