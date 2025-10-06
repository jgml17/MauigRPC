# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

This is a proof-of-concept (POC) project demonstrating gRPC integration with .NET MAUI (Multi-platform App UI). The solution consists of two projects:

1. **GrpcServiceApi** - An ASP.NET Core gRPC server that provides greeting services
2. **MauigRPC** - A .NET MAUI cross-platform client application (Android, iOS, macOS Catalyst, Windows)

## Technology Stack

- **.NET 9.0** - Target framework for both projects
- **gRPC & Protocol Buffers** - For efficient cross-platform communication
- **.NET MAUI** - For building cross-platform mobile/desktop applications
- **ASP.NET Core** - For the gRPC server

## Development Commands

### Building the Solution

```bash
# Build entire solution
dotnet build MauigRPC.sln

# Build specific projects
dotnet build GrpcServiceApi/GrpcServiceApi.csproj
dotnet build MauigRPC/MauigRPC.csproj
```

### Running the gRPC Server

```bash
# Run the gRPC server (HTTP)
dotnet run --project GrpcServiceApi/GrpcServiceApi.csproj --launch-profile http

# Run with HTTPS
dotnet run --project GrpcServiceApi/GrpcServiceApi.csproj --launch-profile https
```

The server runs on:
- HTTP: `http://localhost:5194`
- HTTPS: `https://localhost:7231`

### Running the MAUI Application

```bash
# Run on specific platform
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-android
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-ios
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-maccatalyst
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-windows10.0.19041.0
```

### Cleaning Build Artifacts

```bash
# Clean entire solution
dotnet clean MauigRPC.sln

# Clean specific project
dotnet clean GrpcServiceApi/GrpcServiceApi.csproj
```

### Regenerating gRPC Code from Proto Files

When modifying `.proto` files, rebuild the project to regenerate C# classes:

```bash
dotnet build GrpcServiceApi/GrpcServiceApi.csproj
```

The proto file is located at `GrpcServiceApi/Protos/greet.proto`.

## Architecture

### gRPC Server Architecture (GrpcServiceApi)

- **Entry Point**: `Program.cs` - Configures ASP.NET Core with gRPC services
- **Service Implementation**: `Services/GreeterService.cs` - Implements the `Greeter` gRPC service
- **Protocol Definitions**: `Protos/greet.proto` - Defines service contracts and message types
- **Configuration**: 
  - `appsettings.json` - Configures Kestrel to use HTTP/2 protocol required by gRPC
  - `launchSettings.json` - Development server configurations

The server uses ASP.NET Core's built-in gRPC support with the `Grpc.AspNetCore` package. Services inherit from generated base classes (e.g., `Greeter.GreeterBase`) and override RPC methods.

### MAUI Client Architecture (MauigRPC)

- **Entry Point**: `MauiProgram.cs` - Configures the MAUI application
- **Application Shell**: `App.xaml` / `App.xaml.cs` - Application lifecycle management
- **Navigation**: `AppShell.xaml` - Shell-based navigation structure
- **Main UI**: `MainPage.xaml` / `MainPage.xaml.cs` - Primary user interface
- **Platform-Specific Code**: `Platforms/` directory contains platform-specific implementations:
  - `Android/` - Android-specific entry points and configurations
  - `iOS/` - iOS-specific entry points
  - `MacCatalyst/` - macOS Catalyst implementations
  - `Windows/` - Windows-specific implementations
  - `Tizen/` - Tizen platform support (commented out by default)

The MAUI project uses a single codebase approach with platform-specific code organized in the Platforms folder. The project targets multiple frameworks simultaneously via the `<TargetFrameworks>` property.

### Communication Flow

1. Proto file defines the gRPC contract shared between server and client
2. C# code is auto-generated from the proto file during build
3. Server implements the service interface (GreeterService extends Greeter.GreeterBase)
4. Client can consume the service using the generated client classes

### Key Configuration Notes

- **gRPC requires HTTP/2**: Configured in `appsettings.json` via Kestrel settings
- **Cross-platform targeting**: MAUI project uses conditional compilation for platform-specific code
- **Proto compilation**: The `.proto` file is marked with `GrpcServices="Server"` in the csproj, auto-generating server stubs

## Project Structure

```
MauigRPC/
├── GrpcServiceApi/           # gRPC Server Project
│   ├── Services/             # gRPC service implementations
│   ├── Protos/               # Protocol Buffer definitions
│   └── Program.cs            # Server configuration
├── MauigRPC/                 # MAUI Client Project
│   ├── Platforms/            # Platform-specific code
│   ├── Resources/            # App resources (images, fonts, styles)
│   ├── MainPage.xaml         # Main UI page
│   └── MauiProgram.cs        # App configuration
└── MauigRPC.sln              # Solution file
```

## Development Notes

### Adding New gRPC Services

1. Define service and messages in a `.proto` file under `GrpcServiceApi/Protos/`
2. Add the proto file to `GrpcServiceApi.csproj` with `<Protobuf Include>` tag
3. Rebuild the project to generate C# classes
4. Implement the service by inheriting from the generated base class
5. Register the service in `Program.cs` using `app.MapGrpcService<YourService>()`

### Platform-Specific Minimum Versions

- iOS: 15.0
- macOS Catalyst: 15.0  
- Android: API 21
- Windows: 10.0.17763.0
- Tizen: 6.5

### MAUI Resource Management

Resources are organized under `MauigRPC/Resources/`:
- `AppIcon/` - Application icons
- `Splash/` - Splash screen assets
- `Images/` - Image resources
- `Fonts/` - Custom fonts
- `Styles/` - XAML styles and colors
- `Raw/` - Raw assets accessible at runtime
