# HBS WEB API and Client 

This project was generated using WEB API and .Net Core MVC using .NET 5

## Description

This project is for the interview usecase of KOVAI.CO API and Client Module

## Development server

### Run Migration and Setup DB
Download and open the solutoin in Visual Studio 2019 or higher

Open Package Manage console in Visual Studio
Set Default Project to "Source\Infrastructure\HBS.Persistence" and run below command
Add-MIgration "Iniital" -context "ApplicationDbContext"
Update-Database

Set Default Project to "Source\Infrastructure\HBS.Identity" and run below command

Add-MIgration "Iniital" -context "IdentityContext"
Update-Database


### Setup Running Environment

Right click on the solution folder and set the Multiple Startup project as in the below image and Apply.

Hit F5 and run the application.

![image](https://user-images.githubusercontent.com/83652337/145748956-384e85e6-84ac-46a4-826c-ba991c2fafb1.png)


## Default Login

Use Google Authentication to login or create a new user using the Register button in the Login page
