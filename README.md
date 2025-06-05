# OrderProcessingApp

OrderProcessingApp is a .NET console application that processes orders for a pizzeria.

## Project Structure

- **src/**: Application source code (models, processors, validators, loaders, outputs, etc.)
- **input/**: Contains input data files in JSON format:
  - `orders.json`
  - `products.json`
  - `ingredients.json`
- **tests/**: Unit tests for the application

## Input Files

All input files are located in the `input` folder at the project root:

- **orders.json**: List of orders, each with properties like `OrderId`, `ProductId`, `Quantity`, `CreatedAt`, `DeliveryAt`, and `DeliveryAddress`.
- **products.json**: List of products, each with `ProductId`, `Name`, and `Price`.
- **ingredients.json**: Dictionary mapping each `ProductId` to a list of required ingredients and their amounts.

## How to Run

1. **Ensure .NET 8 SDK is installed** on your machine.
2. **Restore dependencies** (if needed): 
   ```bash
   dotnet restore
   ```
3. **Build the project**:
   ```bash
   dotnet build
   ```

### Run the Application (Console)

You can run the application from the console in two ways:

#### **1. From the project root (recommended):**
   ```bash
   dotnet run --project src/OrderProcessingApp.csproj
   ```

#### **2. From the `src` directory:**
   ```bash
   cd src
   dotnet run
   ```

### Run from Visual Studio

- You can also run the application using **the _Run_ button** from the top menu in Visual Studio or your preferred IDE.  


## Console Output

When you run the application, it will:

- Load orders, products, and ingredients from the `input` folder.
- Validate and Process each order.
- For valid orders, print:
   - Valid order details.
   - The total price for each order.
   - The total amount of each ingredient required for all valid orders.
- If no valid orders are found, it will display a message accordingly.


## Running Tests

You can run the unit tests in two ways:

### 1. Using the .NET CLI

From the project root, run:
   ```bash
   dotnet test
   ```

### 2. Using Visual Studio Test Explorer

- Go to **Test > Test Explorer** from the top menu.
- Click the **Run All** button to execute all tests, or right-click individual tests to run/debug them.