# CryptoPredictorAPI

CryptoPredictorAPI is a work-in-progress project aimed at predicting cryptocurrency prices using machine learning models and fetching real-time and historical data from the Binance API.

## Features

- Fetch real-time price data from Binance, export the data to csv to train models.
- Retrieve historical K-line data.
- Analyze K-line patterns.
- Predict future prices using TensorFlow models.

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
   cd .\TensorProject\
   ```

3. **Update the `appsettings.json` file**. Replace the placeholders for Binance API keys, Csv export file path:
   ```json
   "BinanceSettings": {
       "ApiKey": "YOUR_BINANCE_API_KEY",
       "ApiSecret": "YOUR_BINANCE_API_SECRET"
   },
   "CsvExportSettings": {
    "FilePath": "PLACEHOLDER"
   },
  
   ```

4. **Add your TensorFlow model** (`model.pb`) to the `TensorFlowModelService` directory. Ensure you replace any placeholders with the actual column names. To view your column names, you can use TensorBoard or print them in the console.

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
## Fetching Historical Binance Data for BTC-USDT

For a systematic retrieval of historical K-line data for the BTC-USDT pair from Binance, utilize the following UNIX timestamps in the specified order, for example:

1. 
   - `startTime`: `1608130279000`
   - `endTime`: `1693996399000`

2. 
   - `startTime`: `1521763200000`
   - `endTime`: `1608076800000`

3. 
   - `startTime`: `1434563200000`
   - `endTime`: `1520876800000`

Each set of timestamps allows for sequential and non-overlapping data retrieval for the given time intervals. Make sure to fetch data in the order provided to ensure data continuity.

### Running the API

You can use Microsoft Visual Studio to run the API.
