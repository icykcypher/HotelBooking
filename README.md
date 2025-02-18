# Hotel Booking Management System

## Description
This project is a hotel room booking management system built with .NET WPF. It allows users to manage rooms, guests, bookings, and payments efficiently.

## Prerequisites
- .NET 8 or later
- SQL Server or another supported database
- Entity Framework Core

## Installation
1. Clone the repository:
   ```sh
   download it from Moodle
   ```
2. Navigate to the project directory:
   ```sh
   cd HotelBooking
   ```
3. Install dependencies:
   ```sh
   dotnet restore
   ```

## Configuration
Before running the application, update the database connection string in `HotelBooking/App.config`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="DataSource" value="YOUR_SERVER"/>
		<add key="Database" value="YOUR_DATABASE"/>
		<add key="Name" value="YOUR_ACCOUNT"/>
		<add key="Password" value="YOUR_PASSWORD"/>
	</appSettings>
</configuration>
```
Replace `YOUR_*` with your actual data.

## Database Migration
Run the following commands to apply migrations and update the database:
```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Running the Application
To run the application, execute:
```sh
dotnet run
```

## Features
- Room management
- Guest management
- Booking system
- Payment integration
- Data import/export via JSON

## License
This project is licensed under the MIT License.

## Contact
For any issues, please create an issue on GitHub or contact the maintainer.

