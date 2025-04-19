﻿﻿
-- REPLACE YOUR DATABASE LOGIN AND PASSWORD IN THE SCRIPT BELOW 

Use master
Go

-- Create a login for the admin user
CREATE LOGIN [RecipesAppAdminLogin] WITH PASSWORD = 'pass';
Go

--so user can restore the DB!
ALTER SERVER ROLE sysadmin ADD MEMBER [RecipesAppAdminLogin];
Go

Create Database RecipesAppDB
go