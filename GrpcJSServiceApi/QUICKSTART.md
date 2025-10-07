# Quick Start Guide

Get the NestJS gRPC service running in 5 minutes!

---

## Prerequisites

You need Node.js installed. Check if you have it:

```bash
node --version   # Should show v18 or higher
npm --version    # Should show v9 or higher
```

If you don't have Node.js, install it from: https://nodejs.org/

---

## Step 1: Install Dependencies

```bash
cd GrpcJSServiceApi
npm install
```

This downloads all the required packages (like `dotnet restore`).

**What's happening?**
- Reads `package.json` (like `.csproj`)
- Downloads packages from npm (like NuGet)
- Creates `node_modules/` folder (like `packages/`)

---

## Step 2: Run the Server

### Development Mode (Recommended)

```bash
npm run start:dev
```

This runs the server with **hot reload** - it automatically restarts when you change code!

**You should see:**
```
🚀 gRPC Server is running on http://localhost:5195

Available Services:
  - greet.Greeter
  - contact.ContactService
```

### Production Mode

```bash
# Build first
npm run build

# Then run
npm run start:prod
```

---

## Step 3: Test the Services

### Option 1: Using grpcurl (Recommended)

Install grpcurl if you don't have it:
```bash
# macOS
brew install grpcurl
```

Test the services:

```bash
# Test Greeter service
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello

# Get all contacts
grpcurl -plaintext -d '{}' localhost:5195 contact.ContactService/GetAllContacts

# Get contact by ID
grpcurl -plaintext -d '{"id": 1}' localhost:5195 contact.ContactService/GetContact

# Create a new contact
grpcurl -plaintext -d '{
  "name": "Alice Johnson",
  "address": {
    "street": "789 Pine St",
    "city": "New York",
    "state": "NY",
    "zip_code": "10001",
    "country": "USA"
  },
  "phone_numbers": [
    {"number": "+1-555-1111", "type": "MOBILE"}
  ]
}' localhost:5195 contact.ContactService/CreateContact
```

### Option 2: Using Postman

1. Open Postman
2. Create a new gRPC request
3. Enter server URL: `localhost:5195`
4. Import proto files from `src/protos/`
5. Select a method and send

---

## Project Structure Explained

```
GrpcJSServiceApi/
│
├── src/                      # Your source code
│   ├── protos/              # Proto files (same as .NET)
│   │   ├── greet.proto
│   │   └── contact.proto
│   │
│   ├── services/            # Service implementations
│   │   ├── greeter.controller.ts
│   │   └── contact.controller.ts
│   │
│   ├── app.module.ts        # App configuration
│   └── main.ts             # Entry point (starts server)
│
├── dist/                    # Compiled JavaScript (after build)
├── node_modules/           # Downloaded packages
│
├── package.json            # Dependencies (like .csproj)
├── tsconfig.json          # TypeScript settings
└── nest-cli.json          # NestJS CLI config
```

---

## Common Commands

```bash
# Install dependencies
npm install

# Run in development (auto-reload)
npm run start:dev

# Build the project
npm run build

# Run in production
npm run start:prod

# Install a new package
npm install package-name

# View available scripts
npm run
```

---

## Making Changes

### 1. Modify a Service

Edit `src/services/greeter.controller.ts`:

```typescript
@GrpcMethod('Greeter', 'SayHello')
sayHello(request: HelloRequest): HelloReply {
  return {
    message: `Olá ${request.name}!`,  // Changed to Portuguese!
  };
}
```

If running in dev mode (`npm run start:dev`), it will **auto-reload**!

Test:
```bash
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
# Response: {"message": "Olá World!"}
```

### 2. Add a New Method

**Step 1:** Add to proto file (`src/protos/greet.proto`):
```protobuf
service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
  rpc SayGoodbye (HelloRequest) returns (HelloReply);  // NEW
}
```

**Step 2:** Add method to controller:
```typescript
@GrpcMethod('Greeter', 'SayGoodbye')
sayGoodbye(request: HelloRequest): HelloReply {
  return {
    message: `Goodbye ${request.name}!`,
  };
}
```

**Step 3:** Restart server and test:
```bash
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayGoodbye
```

---

## Troubleshooting

### Port Already in Use
```
Error: listen EADDRINUSE: address already in use :::5195
```

**Solution:** Kill the existing process:
```bash
# Find process using port 5195
lsof -ti:5195

# Kill it
kill -9 $(lsof -ti:5195)
```

Or change the port in `src/main.ts`:
```typescript
url: '0.0.0.0:5196',  // Use different port
```

### Dependencies Not Found
```
Error: Cannot find module '@nestjs/core'
```

**Solution:** Install dependencies:
```bash
npm install
```

### TypeScript Errors

**Solution:** Rebuild:
```bash
npm run build
```

---

## Comparing with .NET

| Task | .NET | NestJS |
|------|------|--------|
| **Install dependencies** | `dotnet restore` | `npm install` |
| **Run dev mode** | `dotnet run` | `npm run start:dev` |
| **Build** | `dotnet build` | `npm run build` |
| **Run production** | `dotnet run -c Release` | `npm run start:prod` |
| **Clean** | `dotnet clean` | `rm -rf dist/` |

---

## Next Steps

1. ✅ **Run the server** - `npm run start:dev`
2. ✅ **Test with grpcurl** - Verify it works
3. 📖 **Read UNDERSTANDING_NESTJS.md** - Learn how it works
4. 📖 **Read COMPARISON.md** - See .NET vs NestJS differences
5. 🔧 **Modify services** - Make your own changes
6. 🚀 **Connect your MAUI client** - Point it to port 5195

---

## Getting Help

- **NestJS Docs**: https://docs.nestjs.com/
- **gRPC Docs**: https://grpc.io/docs/
- **Node.js Docs**: https://nodejs.org/docs/

---

## Summary

```bash
# 1. Install
npm install

# 2. Run
npm run start:dev

# 3. Test
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello

# ✅ Done!
```

Your NestJS gRPC service is now running alongside your .NET service!
- .NET: `localhost:5194`
- NestJS: `localhost:5195`
