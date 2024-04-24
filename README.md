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

 
## Database schema
![image](https://github.com/IhorArkh/SpeedyWheelsSales/assets/118860688/45c3beb8-b9e2-474f-b5ec-f6f8ee7e0a02)

