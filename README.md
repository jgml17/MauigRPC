# MauigRPC - Full-Stack gRPC POC

A comprehensive proof-of-concept demonstrating gRPC communication across multiple platforms and frameworks:
- **.NET gRPC Server** (ASP.NET Core)
- **NestJS gRPC Server** (Node.js/TypeScript)
- **.NET MAUI Client** (Cross-platform desktop/mobile)
- **Next.js Web Client** (Browser-based)

## 📁 Projects Overview

### Backend Servers (gRPC)

#### **GrpcServiceApi** (.NET)
- **Type:** ASP.NET Core gRPC Server
- **Language:** C#
- **Port:** `http://localhost:5194`
- **Services:** 
  - Greeter Service (SayHello)
  - Contact Service (Full CRUD: Create, Read, Update, Delete)
- **Features:**
  - HTTP/2 support
  - Reflection API enabled
  - Contact model with nested Address and PhoneNumbers
  - Proto files: `greet.proto`, `contact.proto`

#### **GrpcJSServiceApi** (NestJS)
- **Type:** NestJS gRPC Server (Node.js)
- **Language:** TypeScript
- **Port:** `http://localhost:5195`
- **Services:** Same as .NET version
  - Greeter Service (SayHello)
  - Contact Service (Full CRUD)
- **Features:**
  - Mirrors .NET implementation
  - Same proto contracts
  - TypeScript-based implementation
  - Documentation for .NET developers

### Clients

#### **MauigRPC** (.NET MAUI)
- **Type:** Cross-platform Desktop/Mobile App
- **Language:** C# + XAML
- **Platforms:** Android, iOS, macOS Catalyst, Windows
- **Features:**
  - Direct gRPC calls (no proxy needed)
  - Native UI with XAML
  - Platform-specific address handling
  - Can connect to both .NET and NestJS servers

#### **NextJSgRPC** (Next.js)
- **Type:** Web Application (Browser-based)
- **Language:** TypeScript + React
- **Port:** `http://localhost:3005`
- **Features:**
  - Modern web UI with Tailwind CSS
  - API routes as gRPC proxy (browsers can't call gRPC directly)
  - Full contact management UI
  - Switch between .NET and NestJS servers
  - Comprehensive documentation for .NET developers

## 🚀 Quick Start

### Option A: .NET Stack

#### 1. Start .NET gRPC Server
```bash
cd GrpcServiceApi
dotnet run --launch-profile http
# Server runs on http://localhost:5194
```

#### 2. Run MAUI Client
```bash
# macOS
dotnet build MauigRPC/MauigRPC.csproj -t:Run -f net9.0-maccatalyst

# Or use Rider: Select "MauigRPC" → Click Run ▶️
```

#### 3. Test
- Enter name and click "Call gRPC Service"
- Navigate to Contacts page for full CRUD operations

---

### Option B: NestJS + Next.js Stack

#### 1. Start NestJS gRPC Server
```bash
cd GrpcJSServiceApi
npm install          # First time only
npm run start:dev
# Server runs on http://localhost:5195
```

#### 2. Run Next.js Web Client
```bash
cd NextJSgRPC
npm install          # First time only
npm run dev
# Web app runs on http://localhost:3005
```

#### 3. Test
- Open browser: http://localhost:3005
- Test Greeter service on home page
- Navigate to /contacts for full CRUD operations
- Switch between .NET and NestJS servers in UI

---

### Option C: Run Everything

```bash
# Terminal 1: .NET Server
cd GrpcServiceApi && dotnet run --launch-profile http

# Terminal 2: NestJS Server
cd GrpcJSServiceApi && npm run start:dev

# Terminal 3: Next.js Client
cd NextJSgRPC && npm run dev

# Terminal 4: MAUI Client
cd MauigRPC && dotnet build -t:Run -f net9.0-maccatalyst
```

Now you can test:
- MAUI client → .NET server
- MAUI client → NestJS server
- Next.js client → .NET server
- Next.js client → NestJS server

## 🌐 Architecture Comparison

### Communication Patterns

#### .NET MAUI (Direct gRPC)
```
MAUI App (C#) → gRPC Client → gRPC Server (.NET or NestJS)
```
- Direct gRPC calls
- No proxy needed
- Works on all platforms (desktop, mobile)

#### Next.js (HTTP + gRPC)
```
Browser (React) → HTTP → Next.js API Route → gRPC Client → gRPC Server
```
- Browser calls HTTP API routes
- API routes proxy to gRPC
- Necessary because browsers can't call gRPC directly

### Technology Stack

| Component | .NET Stack | JavaScript/TypeScript Stack |
|-----------|------------|-----------------------------|
| **Server** | ASP.NET Core | NestJS |
| **Client** | .NET MAUI | Next.js |
| **Language** | C# | TypeScript |
| **UI** | XAML | React + Tailwind CSS |
| **Platform** | Desktop/Mobile | Web Browser |
| **gRPC** | Direct | Via API Route Proxy |
| **Port (Server)** | 5194 | 5195 |
| **Port (Client)** | N/A (native app) | 3005 (web) |

## 📊 Project Details

### Proto Files (Shared Contract)

Both servers implement the same proto contracts:

#### **greet.proto**
```protobuf
service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}
```

#### **contact.proto**
```protobuf
service ContactService {
  rpc CreateContact (CreateContactRequest) returns (ContactReply);
  rpc GetContact (GetContactRequest) returns (ContactReply);
  rpc GetAllContacts (GetAllContactsRequest) returns (ContactListReply);
  rpc UpdateContact (UpdateContactRequest) returns (ContactReply);
  rpc DeleteContact (DeleteContactRequest) returns (DeleteContactReply);
}

message Contact {
  int32 id = 1;
  string name = 2;
  Address address = 3;
  repeated PhoneNumber phone_numbers = 4;
}
```

### Key Features

✅ **Full CRUD Operations** - Create, Read, Update, Delete contacts  
✅ **Nested Objects** - Address with street, city, state, zip, country  
✅ **Repeated Fields** - Multiple phone numbers per contact  
✅ **Enums** - Phone types (MOBILE, HOME, WORK)  
✅ **Server Switching** - Both clients can connect to either server  
✅ **Type Safety** - TypeScript (NestJS/Next.js) and C# (.NET)  
✅ **Modern UI** - XAML (MAUI) and React/Tailwind (Next.js)  
✅ **Hot Reload** - Fast development in all projects  

### Platform-Specific Notes

#### Android Emulator (MAUI)
- Uses `10.0.2.2` to reach host machine's localhost
- Configure in `Services/GrpcGreeterService.cs`

#### iOS Simulator (MAUI)
- Uses `localhost:5194` directly
- Simulator can access host's localhost

#### Web Browser (Next.js)
- Uses API routes as proxy
- No direct gRPC support in browsers
- API routes run on Next.js server (Node.js)

#### Physical Devices (MAUI)
- Update server address to your machine's IP
- Ensure both devices on same network

## 🛠️ Requirements

### For .NET Projects (GrpcServiceApi + MauigRPC)
- .NET 9.0 SDK
- For iOS: Xcode and iOS SDK
- For Android: Android SDK and emulator
- For macOS: macOS 12.0 or later
- IDE: Visual Studio, Rider, or VS Code

### For Node.js Projects (GrpcJSServiceApi + NextJSgRPC)
- Node.js 18+ and npm
- No additional platform requirements (web-based)
- IDE: Rider, VS Code, or WebStorm

### Development Tools
- **grpcurl** - Command-line tool to test gRPC services
- **Rider** (recommended) - Run configurations included for all projects
- **Browser DevTools** - For debugging Next.js client

## 📚 Documentation

### General Documentation
- **[WARP.md](./WARP.md)** - Development commands and architecture overview
- **[GRPC_VS_REST_GUIDE.md](./GRPC_VS_REST_GUIDE.md)** - Comprehensive guide comparing gRPC with REST API
- **[GRPC_QUICK_REFERENCE.md](./GRPC_QUICK_REFERENCE.md)** - Quick reference cheat sheet for gRPC
- **[TROUBLESHOOTING.md](./TROUBLESHOOTING.md)** - Solutions for common issues
- **[DOCUMENTATION_SUMMARY.md](./DOCUMENTATION_SUMMARY.md)** - Code documentation overview

### Project-Specific Documentation

#### **GrpcJSServiceApi** (NestJS Server)
- **[GrpcJSServiceApi/README.md](./GrpcJSServiceApi/README.md)** - NestJS setup and usage
- **[GrpcJSServiceApi/docs/](./GrpcJSServiceApi/docs/)** - Detailed guides:
  - `NESTJS_VS_DOTNET.md` - Framework comparison for .NET developers
  - `GRPC_SETUP_EXPLAINED.md` - How gRPC works in NestJS
  - `CONTACT_SERVICE_EXPLAINED.md` - Implementation details

#### **NextJSgRPC** (Next.js Web Client) ⭐ **For .NET Developers**
- **[NextJSgRPC/README.md](./NextJSgRPC/README.md)** - Quick start guide
- **[NextJSgRPC/NEXTJS_VS_DOTNET.md](./NextJSgRPC/NEXTJS_VS_DOTNET.md)** - Complete framework comparison
- **[NextJSgRPC/docs/](./NextJSgRPC/docs/)** - Detailed guides:
  - `CONTACT_DATA_EXPLAINED.md` - Data flow and naming conventions 🔥
  - `QUICK_REFERENCE.md` - Syntax cheat sheet for .NET developers
  - `INDEX.md` - Documentation master index

### 💡 For .NET Developers Learning Node.js/TypeScript

If you only know .NET and want to understand the NestJS and Next.js projects:

1. **Start with NestJS Server:**
   - Read `GrpcJSServiceApi/docs/NESTJS_VS_DOTNET.md`
   - Compare with your C# server code
   - See how TypeScript mirrors C# concepts

2. **Then Learn Next.js Client:**
   - Read `NextJSgRPC/NEXTJS_VS_DOTNET.md`
   - Compare with your MAUI client code
   - Read `NextJSgRPC/docs/CONTACT_DATA_EXPLAINED.md` for data flow

3. **Keep as Reference:**
   - `NextJSgRPC/docs/QUICK_REFERENCE.md` - For quick syntax lookups

**Everything is explained in .NET terms with side-by-side code examples!**

### 🎯 Code Documentation (IntelliSense)
All C# classes and methods have comprehensive XML documentation comments (///).
**Hover over any class or method in your IDE to see:**
- Purpose and functionality
- Parameter descriptions
- Return value details
- Code examples
- Performance notes
- Platform-specific guidance
