# epr-payment-service


## Description
Service to calculate fees and manage payment records for EPR

## Getting Started

### Prerequisites
- .NET SDK
- Visual Studio or Visual Studio Code
- MSSQL

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/DEFRA/epr-payment-service.git
    ```
2. Navigate to the project directory:
    ```bash
    cd \src\EPR.Payment.Service
    ```
3. Restore the dependencies:
    ```bash
    dotnet restore
    ```

### Configuration
The application uses appsettings.json for configuration. For development, use appsettings.Development.json.

#### Sample 
appsettings.Development.json

```
{
  "ConnectionStrings": {
    "PaymentConnnectionString": "Data Source=.;Initial Catalog=FeesPayment;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FeatureManagement": {
    "EnablePaymentsFeature": true,
    "EnablePaymentStatusInsert": true,
    "EnablePaymentStatusUpdate": true,
    "EnableGetPaymentByExternalPaymentId": true,
    "EnableRegistrationFeesFeature": true,
    "EnableProducerResubmissionAmount": true,
    "EnableRegistrationFeesCalculation": true
  }
}
```

### Initialise Database

#### Visual Studio
1. Within the Package Manager Console window:
    ```
    Update-Database
    ```

OR

#### Command Line
1. Navigate to the project directory:
    ```bash
    cd \src\EPR.Payment.Service
    ```

2. Install [dotnet EF CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

    ```
    dotnet tool install --global dotnet-ef
    ```
3. Update Database 
    ```
    dotnet ef database update
    ```

### Building the Application
1. Navigate to the project directory:
    ```bash
    cd \src\EPR.Payment.Service
    ```

2. To build the application:
    ```bash
    dotnet build
    ```

### Running the Application
1. Navigate to the project directory:
    ```bash
    cd \src\EPR.Payment.Service
    ```
 
2. To run the service locally:
    ```bash
    dotnet run
    ```

3. Launch Browser:

    Service Health Check:

    [https://localhost:7107/health](https://localhost:7107/health)

    Swagger:

    [https://localhost:7107/swagger/index.html](https://localhost:7107/swagger/index.html)