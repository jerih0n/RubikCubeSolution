# Rubik's Cube Solution

A web application that simulates and visualizes a Rubik's Cube with interactive rotation controls. The application allows users to rotate cube faces, view the cube state in a 2D matrix representation, and run automated test sequences.

## Prerequisites

- **.NET 8.0 SDK** or later
  - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
  - Verify installation: `dotnet --version`

- **IDE** (optional but recommended):
  - Visual Studio 2022 or later
  - Visual Studio Code with C# extension
  - JetBrains Rider

## Project Structure

```
RubikCubeSolution/
├── RubikCubeSolution.Logic/          # Core business logic
│   ├── Configuration/                # Cube configuration
│   ├── Constants/                    # Matrix and side constants
│   ├── Enums/                        # Enumerations
│   ├── Helpers/                      # Helper classes (rotations, transforms)
│   ├── Models/                       # Domain models
│   └── Services/                     # Business services
├── RubicCubeSolution.Web/            # ASP.NET Core Web Application
│   ├── Controllers/                  # MVC Controllers
│   ├── Models/                       # View models
│   ├── Services/                     # Application services
│   ├── Views/                        # Razor views
│   └── wwwroot/                      # Static files (CSS, JS)
└── RubikCubeSolution.Test/           # Unit tests
    ├── Controllers/                  # Controller tests
    ├── Helpers/                      # Helper tests
    ├── Models/                       # Model tests
    └── Services/                     # Service tests
```

## Running the Project Locally

### Option 1: Using .NET CLI (Command Line)

1. **Navigate to the solution directory:**
   ```bash
   cd RubikCubeSolution
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the solution:**
   ```bash
   dotnet build
   ```

4. **Run the web application:**
   ```bash
   dotnet run --project RubicCubeSolution.Web/RubikCubeSolution.Web.csproj
   ```

   Or navigate to the Web project directory:
   ```bash
   cd RubicCubeSolution.Web
   dotnet run
   ```

5. **Open your browser:**
   - The application will typically run on `https://localhost:5001` or `http://localhost:5000`
   - Check the console output for the exact URL

### Option 2: Using Visual Studio

1. **Open the solution:**
   - Double-click `RubikCubeSolution.sln` or open it from Visual Studio

2. **Set startup project:**
   - Right-click on `RubicCubeSolution.Web` project
   - Select "Set as Startup Project"

3. **Run the application:**
   - Press `F5` or click the "Run" button
   - The application will open in your default browser

### Option 3: Using Visual Studio Code

1. **Open the workspace:**
   ```bash
   code RubikCubeSolution
   ```

2. **Open the terminal** (Ctrl+` or Terminal → New Terminal)

3. **Run the application:**
   ```bash
   dotnet run --project RubicCubeSolution.Web/RubikCubeSolution.Web.csproj
   ```

## Running the Tests

### Option 1: Using .NET CLI

1. **Run all tests:**
   ```bash
   dotnet test
   ```

2. **Run tests with detailed output:**
   ```bash
   dotnet test --verbosity normal
   ```

3. **Run tests for a specific project:**
   ```bash
   dotnet test RubikCubeSolution.Test/RubikCubeSolution.Tests.csproj
   ```

4. **Run tests with code coverage** (if coverlet is installed):
   ```bash
   dotnet test /p:CollectCoverage=true
   ```

### Option 2: Using Visual Studio

1. **Open Test Explorer:**
   - Go to `Test` → `Test Explorer` (or press `Ctrl+E, T`)

2. **Run all tests:**
   - Click "Run All Tests" in Test Explorer
   - Or use `Test` → `Run All Tests` (Ctrl+R, A)

3. **Run specific tests:**
   - Right-click on a test class or method
   - Select "Run Tests"

### Option 3: Using Visual Studio Code

1. **Install the .NET Test Explorer extension** (optional but recommended)

2. **Run tests from terminal:**
   ```bash
   dotnet test
   ```

## Features

- **Interactive Cube Rotation:**
  - Rotate any face clockwise (F, R, U, B, L, D)
  - Rotate any face counter-clockwise (F', R', U', B', L', D')
  - Visual representation of the cube state

- **Test Sequence:**
  - Automated test sequence button that performs: F (CW), R' (CCW), U (CW), B' (CCW), L (CW), D' (CCW)
  - 1-second delay between each rotation
  - Real-time matrix updates

- **Reset Functionality:**
  - Reset the cube to its initial solved state

- **Rotation History:**
  - View all performed rotations in chronological order

## API Endpoints

- `GET /Home/Index` - Main page displaying the cube matrix
- `POST /Home/Rotate` - Rotate a cube face
  - Request body: `{ "side": 3, "clockwise": true }`
  - Returns: `{ "matrix": [[...]] }`
- `POST /Home/Reset` - Reset the cube to initial state
  - Returns: `{ "matrix": [[...]] }`

## Testing

The test suite includes:

- **Model Tests:** Cube initialization, rotation, and reset functionality
- **Service Tests:** Rotation service and processor tests
- **Controller Tests:** API endpoint tests with mocking
- **Helper Tests:** Rotation helper and lookup table tests
- **Consecutive Rotation Tests:** Multiple rotation sequence tests

Total: ~19 unit tests covering core functionality.

## Troubleshooting

### Port Already in Use

If you get a port conflict error:
```bash
dotnet run --project RubicCubeSolution.Web/RubikCubeSolution.Web.csproj --urls "http://localhost:5001"
```

### Build Errors

1. **Clean and rebuild:**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Check .NET version:**
   ```bash
   dotnet --version
   ```
   Should be 8.0 or later.

### Test Failures

1. **Restore test packages:**
   ```bash
   dotnet restore RubikCubeSolution.Test/RubikCubeSolution.Tests.csproj
   ```

2. **Run tests with detailed output:**
   ```bash
   dotnet test --verbosity detailed
   ```

## Technologies Used

- **.NET 8.0** - Framework
- **ASP.NET Core MVC** - Web framework
- **MSTest** - Testing framework
- **Moq** - Mocking framework for unit tests
- **Bootstrap** - Frontend styling
- **JavaScript** - Client-side interactivity

## License

See LICENSE.txt file for details.
