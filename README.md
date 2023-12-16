# VehicleDetails

## Project Description
Vehicle Details API is a Web API developed using ASP.net web API core, allowing API consumer to check the vehicle details by passing License plate or Merk or both through a RESTful service. It is also using caching services to reduce load and incerease efficiency.
This is following a clean architecture with some factory pattern. The infra layer is independent of the core. Application layer is having API endpoint (controllers).
The API is secured with an API key and is hosted in Docker container. To run the application you must entered below API key in appsettings.json in place of 'YOUR_API_KEY'.
```key
YmEyMmQ3ZDU4NjUyYzY0YzY0MTNlOTNhYTIwNDE2OTZjNTE5YTg0M2JhNjBlY2U4N2FjMTM4MmVlZTYzNmE3Mw==
```

## Functionality
The VehicleDetails API provides the following functionalities:
- **/api/VehicleDetails:** API consumer can view lvehicle details information.
[HttpPost]
```json
{
  "kenteken": "string",
  "merk": "string"
}
```

## Additional Dependencies
It is dependent on RDW api.

## Usage
To use the API solution, follow these steps:
1. Clone the GitHub repository or obtain the source files.
2. Compile the solution using your preferred C# development environment, ensuring that any necessary dependencies are installed and configured as specified in the additional document.
3. Run the application, and start consuming the API through the provided functionalities.

