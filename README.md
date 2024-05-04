# SpeedyWheelsSales - online marketplace for buying and selling cars

## Use cases
- User registration and login
  - User can create new account on SpeedyWheelsSales by providing necessary details such as name, phone number, username and password.
  - Also user have ability to log in/register using external auth provider.
  - Unregistered users can only view and search for advertisements. Registration is required to gain access to the additional functionality.
- View profile
  - Registered users have their own profile. Everybody can visit it from ad to view info about seller and his active ads. 
- Managing user profile
  - Every registered user can change his profile details like photo, name, mobile number, bio etc.
- Search and filter options
  - The buyer searches for vehicles based on criteria such as make, model, year, price, mileage, location etc.
- Save searches
  - Allow users to save search with provided criterias, such as make, model, year range, price range, mileage range, location etc.
  - User have ability to delete saved search or go to corresponding ads.
- Listing a vehicle for sale
  - The seller creates a new listing by providing details about the vehicle, including make, model, condition, images, price, etc.
- Add to favourites
  - User have ability to add ad to favourites list, than ad can be removed from favourites.
- Managing listings
  - The seller can edit, delete, or mark as sold their listings.
- Viewing vehicle details
  -  Every ad has details page, where every user can view additional information about vehicle and seller info.
- Personal messaging
  - Users can engage in private conversations to discuss vehicle details, negotiate prices and make an appointments.
- Localization
  - User have ability to change app language.

---
## Stack

### Architecture
- Clean architecture with CQRS pattern.

### Technologies
- ASP.NET Core Web API
- ASP.NET Core Web MVC
- Entity Framework Core (DB - MS SQL SERVER)
- Duende Identity Server
- SignalR
- Docker

### Libraries
- MediatR - to simplify communication between components.
- AutoMapper - for mapping between entities and DTOs.
- Serilog - for structured logging.
- Fluent Validation - for working with object validation.
- Polly - to apply resilience strategies.
- BuildBundlerMinifier - for applying bundling and minification.
- AutoFixture, Moq, xUnit, FluentAssertions - for unit testing.

### 3rd party services
- Cloudinary - for storing and managing images.
- Google OAuth

 ---
## Database schema
<img width="685" alt="Ssms_135URygAS2" src="https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/ff025059-78d4-466b-bddf-3b1fc1a3b612">

---
## UI screenshots
**Main page**
![chrome_S7gzz22y1R](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/ca0bd6e1-3f05-49d4-abf0-e8806fcaa767)


**Sorting and filtering**
<img width="1920" alt="chrome_TOGmM6ZmXs" src="https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/778ac188-3c64-455b-9c17-2676c93be2a2">


**Ad details**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/87337d94-fc7b-4499-bde1-dbcf2cec197a)


**Your profile**
![chrome_tGHtpiYrDL](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/25d66696-4bdb-496f-827d-bc7ec39694d5)


**Other user's profile**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/6a8e5d41-7ddf-4ad5-8008-7ec6d31b5db2)


**Live chat**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/f55657b4-20ab-4337-bd76-50b012cdc679)


**Favourite ads**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/f63ee47e-6627-4875-98a0-e17c191c83b9)


**Edit ad**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/baec0490-adce-47ee-8576-f5d5ac478be1)


**Create ad**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/63d6d791-e0b2-49d8-9485-8c0f4516c982)


**Edit profile**
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/e6d07b5c-d2ed-4520-a56e-b11f0489ca0f)


**Saved searches**
<img width="1920" alt="chrome_I8X6carRym" src="https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/0af6d1b2-7844-4d97-9d0f-131d0afe95fa">







