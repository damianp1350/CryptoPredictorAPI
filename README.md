# CryptoPredictorAPI

CryptoPredictorAPI is a work in progress project aimed at interacting with the Binance and Binance Testnet APIs to fetch real-time and historical data, as well as trying to predict future prices using your machine learning models.

## Features

- Fetch real-time price data from Binance and Binance Testnet, with the ability to automatically retrieve data at specified intervals.
- Export the fetched data to CSV for further analysis or training machine learning models.
- Utilize your TensorFlow model to predict future price trends.
- Investment trigger service to simulate investment scenarios with Binance Testnet.

## Setup

### Prerequisites

- .NET 6.0
- A Binance account to obtain API keys

### Configuration

1. **Clone the repository**:
   ```bash
   git clone https://github.com/damianp1350/CryptoPredictorAPI.git
   ```

2. **Navigate to the project directory**:
   ```bash
   cd .\CryptoPredictorAPI\
   ```

3. **Update the `appsettings.json` file**. Replace the placeholders for Binance API keys, Binance Testnet API keys, CSV export file path, and the database connection string:
   ```json
   "BinanceSettings": {
       "ApiKey": "YOUR_BINANCE_API_KEY",
       "ApiSecret": "YOUR_BINANCE_API_SECRET"
   },
   "BinanceTestnetSettings": {
    "ApiKey": "YOUR_BINANCE_TESTNET_API_KEY",
    "ApiSecret": "YOUR_BINANCE_TESTNET_API_SECRET"
   },
     "CsvExportSettings": {
    "FilePath": "YOUR_CSV_EXPORT_FILE_PATH"
   },
     "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=Binance;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. **You can connect your flask api to fetch predicted price from .h5 models**.

5. **Database Migrations**:
   
   - If you haven't installed the Entity Framework Core tools, you can do so with the following command:
     ```bash
     dotnet tool install --global dotnet-ef
     ```

   - **Create a new migration**. This will generate the necessary code to update the database schema:
     ```bash
     dotnet ef migrations add InitialCreate
     ```

   - **Update the database** with the new migration:
     ```bash
     dotnet ef database update
     ```

### Running the API

You can use Microsoft Visual Studio to run the API.
