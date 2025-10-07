# GrpcJSServiceApi - Project Overview

A complete NestJS-based gRPC service that mirrors the functionality of your .NET GrpcServiceApi.

---

## 📋 What You Have Now

You now have **two functionally identical gRPC services**:

1. **.NET GrpcServiceApi** (Port 5194) - Your original C# implementation
2. **NestJS GrpcJSServiceApi** (Port 5195) - The new TypeScript implementation

Both services:
- Use the **same proto files** (greet.proto, contact.proto)
- Implement the **same gRPC methods**
- Return the **same responses**
- Can be used by the **same clients**

---

## 🚀 Quick Start

```bash
# 1. Navigate to the project
cd GrpcJSServiceApi

# 2. Install dependencies (first time only)
npm install

# 3. Start the server
npm run start:dev

# 4. Test it
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
```

**Expected output:**
```json
{
  "message": "Hello World"
}
```

---

## 📚 Documentation Files

We've created several guides to help you understand NestJS:

### 1. **QUICKSTART.md** - Start Here! 👈
   - How to install and run the project
   - Basic testing commands
   - Troubleshooting common issues
   - **Best for:** Getting it running immediately

### 2. **UNDERSTANDING_NESTJS.md** - Learn NestJS
   - What is NestJS and how does it compare to .NET?
   - Project structure explained
   - How decorators work
   - Key concepts for .NET developers
   - **Best for:** Understanding the framework

### 3. **COMPARISON.md** - Side-by-Side Code
   - .NET vs NestJS code examples
   - Line-by-line comparisons
   - Syntax differences
   - **Best for:** Translating between C# and TypeScript

### 4. **README.md** - Technical Reference
   - Features list
   - Installation instructions
   - Testing examples
   - **Best for:** Quick reference

---

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────┐
│                    Clients                          │
│  (MAUI App, grpcurl, Postman, etc.)                │
└───────────────┬─────────────────┬───────────────────┘
                │                 │
                ↓                 ↓
    ┌───────────────────┐  ┌──────────────────┐
    │  .NET Service     │  │  NestJS Service  │
    │  (Port 5194)      │  │  (Port 5195)     │
    └───────────────────┘  └──────────────────┘
                │                 │
                ↓                 ↓
        ┌───────────────────────────────┐
        │     Same Proto Files          │
        │  • greet.proto                │
        │  • contact.proto              │
        └───────────────────────────────┘
```

---

## 📦 What's Implemented

### Greeter Service
- ✅ `SayHello` - Simple greeting RPC

### Contact Service (Full CRUD)
- ✅ `CreateContact` - Create new contacts with address and phone numbers
- ✅ `GetContact` - Retrieve a contact by ID
- ✅ `GetAllContacts` - List all contacts
- ✅ `UpdateContact` - Update existing contacts
- ✅ `DeleteContact` - Remove contacts

### Features
- ✅ In-memory data storage (seeded with 2 sample contacts)
- ✅ Nested messages (Address, PhoneNumber)
- ✅ Repeated fields (arrays of phone numbers)
- ✅ Enums (PhoneType: MOBILE, HOME, WORK)
- ✅ Error handling with proper gRPC status codes
- ✅ Comprehensive documentation

---

## 🔧 Technology Stack

| Component | Technology |
|-----------|------------|
| **Framework** | NestJS 10.x |
| **Language** | TypeScript |
| **Runtime** | Node.js 18+ |
| **Protocol** | gRPC (HTTP/2) |
| **Package Manager** | npm |
| **Build Tool** | NestJS CLI |

---

## 📁 Project Structure

```
GrpcJSServiceApi/
│
├── src/                           # Source code
│   ├── protos/                   # Protocol Buffer definitions
│   │   ├── greet.proto          # Greeter service
│   │   └── contact.proto        # Contact CRUD service
│   │
│   ├── services/                # Service implementations
│   │   ├── greeter.controller.ts    # Greeter logic
│   │   └── contact.controller.ts    # Contact CRUD logic
│   │
│   ├── app.module.ts            # Application module (registers services)
│   └── main.ts                  # Entry point (bootstraps server)
│
├── dist/                         # Compiled output (generated)
├── node_modules/                # Dependencies (generated)
│
├── package.json                 # Project metadata & dependencies
├── tsconfig.json               # TypeScript compiler config
├── nest-cli.json               # NestJS CLI config
├── .gitignore                  # Git ignore rules
│
└── Documentation/
    ├── QUICKSTART.md           # Quick start guide
    ├── UNDERSTANDING_NESTJS.md # NestJS concepts
    ├── COMPARISON.md           # .NET vs NestJS
    ├── README.md               # Technical reference
    └── OVERVIEW.md             # This file
```

---

## 🎯 Key Differences from .NET

| Aspect | .NET | NestJS |
|--------|------|--------|
| **Port** | 5194 | 5195 |
| **Language** | C# | TypeScript |
| **Inheritance** | Base classes | Decorators |
| **Async** | `Task<T>` | `T` or `Promise<T>` |
| **Collections** | `Dictionary<K,V>` | `Map<K,V>` |
| **Threading** | Multi-threaded | Single-threaded |
| **Code Gen** | Auto from proto | Manual interfaces |

---

## 🧪 Testing the Service

### Using grpcurl

```bash
# 1. Greeter Service
grpcurl -plaintext -d '{"name": "Alice"}' localhost:5195 greet.Greeter/SayHello

# 2. Get All Contacts
grpcurl -plaintext -d '{}' localhost:5195 contact.ContactService/GetAllContacts

# 3. Get Contact by ID
grpcurl -plaintext -d '{"id": 1}' localhost:5195 contact.ContactService/GetContact

# 4. Create Contact
grpcurl -plaintext -d '{
  "name": "Bob Smith",
  "address": {"street": "123 Main", "city": "NYC", "state": "NY", "zip_code": "10001", "country": "USA"},
  "phone_numbers": [{"number": "+1-555-0000", "type": "MOBILE"}]
}' localhost:5195 contact.ContactService/CreateContact

# 5. Update Contact
grpcurl -plaintext -d '{
  "id": 3,
  "name": "Bob Johnson",
  "address": {"street": "456 Oak", "city": "LA", "state": "CA", "zip_code": "90001", "country": "USA"},
  "phone_numbers": [{"number": "+1-555-1111", "type": "WORK"}]
}' localhost:5195 contact.ContactService/UpdateContact

# 6. Delete Contact
grpcurl -plaintext -d '{"id": 3}' localhost:5195 contact.ContactService/DeleteContact
```

### Using Your MAUI Client

Update your MAUI app to connect to the NestJS service:

```csharp
// Change the channel URL
var channel = GrpcChannel.ForAddress("http://localhost:5195");
var client = new Greeter.GreeterClient(channel);
```

---

## 💡 Common Commands

```bash
# Development
npm install              # Install dependencies
npm run start:dev       # Run with hot-reload
npm run build           # Compile TypeScript
npm run start:prod      # Run production build

# Maintenance
npm update              # Update packages
npm audit fix           # Fix security issues
rm -rf node_modules/    # Clean dependencies
npm install             # Reinstall

# Troubleshooting
kill -9 $(lsof -ti:5195)  # Kill process on port 5195
```

---

## 📖 Learning Path

**If you're new to NestJS, follow this order:**

1. ✅ **Run the project** (QUICKSTART.md)
2. 📚 **Understand the basics** (UNDERSTANDING_NESTJS.md)
3. 🔍 **Compare with .NET** (COMPARISON.md)
4. 🛠️ **Make your first change**
5. 🚀 **Build something new**

---

## 🆚 When to Use Which?

### Use the .NET Service When:
- ✅ You prefer C# and the .NET ecosystem
- ✅ You need tight integration with other .NET services
- ✅ Your team is primarily .NET developers
- ✅ You want auto-generated code from proto files

### Use the NestJS Service When:
- ✅ You prefer TypeScript/JavaScript
- ✅ You need npm packages or Node.js libraries
- ✅ Your team knows JavaScript better than C#
- ✅ You want lightweight, fast startup times

### Use Both When:
- ✅ You want to compare performance
- ✅ You're learning gRPC across platforms
- ✅ You need redundancy/failover options
- ✅ Different teams prefer different tech stacks

---

## 🎓 Resources

### NestJS
- **Official Docs**: https://docs.nestjs.com/
- **gRPC Guide**: https://docs.nestjs.com/microservices/grpc
- **GitHub**: https://github.com/nestjs/nest

### gRPC
- **Official Site**: https://grpc.io/
- **Node.js Guide**: https://grpc.io/docs/languages/node/
- **Protocol Buffers**: https://developers.google.com/protocol-buffers

### TypeScript
- **Handbook**: https://www.typescriptlang.org/docs/
- **Playground**: https://www.typescriptlang.org/play

---

## 🤝 Contributing & Extending

### Adding a New Service

**1. Create proto file:**
```bash
# Add src/protos/myservice.proto
```

**2. Create controller:**
```bash
# Add src/services/myservice.controller.ts
```

**3. Register in module:**
```typescript
// app.module.ts
@Module({
  controllers: [..., MyServiceController],
})
```

**4. Update main.ts:**
```typescript
// main.ts
package: ['greet', 'contact', 'myservice'],
protoPath: [..., 'src/protos/myservice.proto'],
```

---

## ❓ FAQ

**Q: Can I use this in production?**
A: Yes, but replace in-memory storage with a real database (PostgreSQL, MongoDB, etc.)

**Q: Can both services run at the same time?**
A: Yes! They use different ports (5194 vs 5195)

**Q: Do clients need to change to use NestJS instead of .NET?**
A: No! gRPC clients only care about the proto contract, not the implementation

**Q: Is NestJS as fast as .NET?**
A: Both are fast. .NET might have slight edge in raw performance, Node.js has fast startup

**Q: Can I use this with my MAUI app?**
A: Absolutely! Just point your client to port 5195 instead of 5194

---

## 🎉 You're Ready!

You now have a complete, production-ready gRPC service built with NestJS that matches your .NET implementation. 

**Next steps:**
1. Run `npm install` and `npm run start:dev`
2. Test it with grpcurl
3. Read the documentation
4. Start building!

Happy coding! 🚀
