# Real Estate API Solution

## Overview

This repository contains a solution for a technical test for a Senior .NET Developer position. The task was to create an API for managing property information for a large Real Estate company. The solution includes services to handle the creation, updating, and listing of properties, as well as managing property images and prices.

## Solution Details

### Architecture

- **Projects**: The solution is divided into several projects to maintain a clean separation of concerns:
  - **RealEstate.API**: Contains the API controllers and configuration.
  - **RealEstate.Core**: Contains the business logic, DTOs, and interfaces.
  - **RealEstate.Infrastructure**: Contains the data access layer and implementations of the services.
  - **RealEstate.Tests**: Contains unit tests for the application.

### Features

1. **Create Property Building**
   - **Endpoint**: `POST /api/properties`
   - **Description**: Adds a new property to the database.
   - **Request Body**: JSON object with property details.

2. **Add Image to Property**
   - **Endpoint**: `POST /api/properties/{propertyId}/images`
   - **Description**: Uploads and associates an image with a specific property.
   - **Request Body**: JSON object with image details in Base64 format.

3. **Change Property Price**
   - **Endpoint**: `PUT /api/properties/{propertyId}/price`
   - **Description**: Updates the price of a property.
   - **Request Body**: JSON object with new price.

4. **Update Property Details**
   - **Endpoint**: `PUT /api/properties/{propertyId}`
   - **Description**: Updates other details of a property.
   - **Request Body**: JSON object with updated property details.

5. **List Properties with Filters**
   - **Endpoint**: `GET /api/properties`
   - **Description**: Retrieves a list of properties with various filtering options.
   - **Query Parameters**: Filters such as price range, location, and property type.

### Technology Stack

- **.NET 5**: Framework for building the API.
- **SQL Server**: Database for storing property information.
- **C#**: Programming language used.
- **NUnit**: Framework for unit testing.

### Setup Instructions

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/real-estate-api.git
   cd real-estate-api
