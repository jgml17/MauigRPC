# MauigRPC - gRPC + .NET MAUI Integration POC

A proof-of-concept demonstrating gRPC communication between a .NET MAUI client application and an ASP.NET Core gRPC server.

## Projects

- **GrpcServiceApi** - ASP.NET Core gRPC server providing greeting services
- **MauigRPC** - .NET MAUI cross-platform client (Android, iOS, macOS Catalyst, Windows)

## Quick Start

### 1. Start the gRPC Server

```bash
dotnet run --project GrpcServiceApi/GrpcServiceApi.csproj --launch-profile http
```

The server will run on `http://localhost:5194`

### 2. Run the MAUI Client

Choose your target platform:

```bash
# macOS Catalyst
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-maccatalyst

# iOS Simulator
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-ios

# Android Emulator
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-android
```

### 3. Test the Integration

1. Enter your name in the text field (default: "MAUI Client")
2. Click "Call gRPC Service" button
3. The app will call the gRPC server and display the greeting response

## Platform-Specific Notes

### Android Emulator
- The app uses `10.0.2.2` to reach the host machine's localhost
- Ensure the gRPC server is running on your host machine

### iOS Simulator
- The app uses `localhost:5194` to reach the server
- The simulator can directly access the host's localhost

### Physical Devices
- Update the server address in `Services/GrpcGreeterService.cs`
- Replace `localhost` or `10.0.2.2` with your machine's IP address
- Ensure both devices are on the same network

## Architecture

### Communication Flow
1. Proto file (`greet.proto`) defines the gRPC contract
2. C# client/server code is auto-generated during build
3. Server implements `Greeter.GreeterBase` service
4. Client uses `Greeter.GreeterClient` to make calls

### Key Components

**Server:**
- `Services/GreeterService.cs` - Implementation of the gRPC service
- `Protos/greet.proto` - Service and message definitions
- HTTP/2 configured in `appsettings.json`

**Client:**
- `Services/GrpcGreeterService.cs` - Wrapper for gRPC client calls
- `MainPage.xaml` - UI with input field and call button
- Platform-specific addressing for different environments

## Requirements

- .NET 9.0 SDK
- For iOS: Xcode and iOS SDK
- For Android: Android SDK and emulator
- For macOS: macOS 12.0 or later

## Documentation

### 📖 Guides and References
- **[WARP.md](./WARP.md)** - Development commands and architecture overview
- **[GRPC_VS_REST_GUIDE.md](./GRPC_VS_REST_GUIDE.md)** - Comprehensive guide comparing gRPC with REST API (📚 Start here!)
- **[GRPC_QUICK_REFERENCE.md](./GRPC_QUICK_REFERENCE.md)** - Quick reference cheat sheet for gRPC development
- **[TROUBLESHOOTING.md](./TROUBLESHOOTING.md)** - Solutions for common issues
- **[DOCUMENTATION_SUMMARY.md](./DOCUMENTATION_SUMMARY.md)** - Code documentation overview (XML doc comments)

### 💡 Code Documentation (IntelliSense)
All classes and methods have comprehensive XML documentation comments (///). 
**Hover over any class or method in your IDE to see:**
- Purpose and functionality
- Parameter descriptions
- Return value details
- Code examples
- Performance notes
- Platform-specific guidance
- Troubleshooting tips

**100% Documentation Coverage**: 7 files, 7 classes, 12 methods, 3 fields - all documented!
