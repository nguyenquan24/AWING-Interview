# Treasure Hunt Solver

A web application that helps find the optimal path to collect treasures with minimum fuel consumption.

## Running Locally

### Prerequisites

Before running the application locally, you need to install:

1. **.NET 8.0 SDK**
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   - After installation, verify by running: `dotnet --version`

2. **Node.js (v18 or later)**
   - Download from: https://nodejs.org/
   - After installation, verify by running: `node --version`

3. **SQL Server**
   - Download SQL Server Express from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Or SQL Server Developer Edition if you prefer
   - Remember the connection string details you set during installation

### Backend (API)

1. Navigate to the API project directory:
   ```bash
   cd TreasureHunt.API
   ```

2. Create and update the database:
   ```bash
   dotnet ef database update
   ```

3. Start the API:
   ```bash
   dotnet run
   ```
   The API will start at: https://localhost:7294

### Frontend

1. Navigate to the frontend project directory:
   ```bash
   cd TreasureHunt.Frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the frontend application:
   ```bash
   npm start
   ```
   The application will open at: http://localhost:3000

## Running with Docker

If you don't want to install the prerequisites, you can run the application using Docker:

1. **Install Docker**
   - Download and install Docker Desktop from: https://www.docker.com/products/docker-desktop
   - Start Docker Desktop

2. **Run the Application**
   - Open a terminal in the project root directory
   - Run the following commands:
   ```bash
   docker-compose up -d --build
   ```
   - Wait for the containers to start (this might take a few minutes on first run)
   - Open your browser and navigate to: http://localhost

3. **Stop the Application**
   ```bash
   docker-compose down
   ```

## Using the Application
![image](https://github.com/user-attachments/assets/7cbbef86-484e-4b0f-9d00-186dd6d71bf5)

1. **Enter Matrix Dimensions**
   - Rows (N): Enter the number of rows (maximum 500)
   - Columns (M): Enter the number of columns (maximum 500)
   - Max Chest Number (P): Enter the maximum chest number (maximum 9)

2. **Fill the Treasure Map Matrix**
   - Enter numbers from 1 to P in the matrix cells
   - Each number represents a chest that must be collected in sequence

3. **Solve**
   - Click the "SOLVE" button to find the optimal path
   - The result will show the minimum fuel needed

4. **View History**
   - Previous solutions are saved and displayed below
   - Each entry shows the matrix size, number of chests, and result

## Troubleshooting

- If you see a database connection error, ensure SQL Server is running
- If the frontend can't connect to the API, check if both applications are running
- For Docker issues, try:
  ```bash
  docker-compose down
  docker system prune -f
  docker-compose up -d --build
  ```

## Support

If you encounter any issues or need help, please create an issue in the repository. 
