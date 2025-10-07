# 📂 Project Guide - File by File

A complete guide to every file in the GrpcJSServiceApi project.

---

## 🎯 Start Here

If you're completely new to NestJS, read the files in this order:

1. **OVERVIEW.md** ← You are here! Start with this
2. **QUICKSTART.md** ← Get it running in 5 minutes
3. **UNDERSTANDING_NESTJS.md** ← Learn how NestJS works
4. **COMPARISON.md** ← See .NET vs NestJS side-by-side

---

## 📋 Complete File Structure

```
GrpcJSServiceApi/
│
├── 📘 Documentation Files (START HERE!)
│   ├── OVERVIEW.md                    ← High-level project overview
│   ├── QUICKSTART.md                  ← Get started in 5 minutes
│   ├── UNDERSTANDING_NESTJS.md        ← Learn NestJS concepts
│   ├── COMPARISON.md                  ← .NET vs NestJS comparison
│   ├── README.md                      ← Technical reference
│   └── PROJECT_GUIDE.md               ← This file!
│
├── ⚙️ Configuration Files
│   ├── package.json                   ← Dependencies & scripts (like .csproj)
│   ├── tsconfig.json                  ← TypeScript compiler settings
│   ├── nest-cli.json                  ← NestJS CLI configuration
│   └── .gitignore                     ← Git ignore rules
│
├── 📁 src/ (Your Source Code)
│   │
│   ├── 📄 main.ts                     ← Entry point (starts the server)
│   ├── 📄 app.module.ts               ← App configuration (registers services)
│   │
│   ├── 📁 protos/ (Protocol Buffers)
│   │   ├── greet.proto                ← Greeter service definition
│   │   └── contact.proto              ← Contact CRUD service definition
│   │
│   └── 📁 services/ (gRPC Implementations)
│       ├── greeter.controller.ts      ← Greeter service implementation
│       └── contact.controller.ts      ← Contact CRUD implementation
│
├── 📁 dist/ (Generated after build)
│   └── Compiled JavaScript files
│
└── 📁 node_modules/ (Generated after npm install)
    └── Downloaded npm packages
```

---

## 📘 Documentation Files

### OVERVIEW.md (This File!)
**Purpose:** High-level introduction to the entire project
**Read when:** You first open the project
**Contains:**
- What the project is
- Quick start commands
- Documentation roadmap
- Architecture overview

### QUICKSTART.md ⭐ START HERE FIRST!
**Purpose:** Get the project running immediately
**Read when:** You want to run it right now
**Contains:**
- Installation steps
- How to run the server
- Testing commands
- Troubleshooting

### UNDERSTANDING_NESTJS.md
**Purpose:** Learn NestJS from a .NET perspective
**Read when:** You want to understand how it works
**Contains:**
- What is NestJS?
- Project structure explanation
- How decorators work
- Key concepts for .NET developers
- Step-by-step code walkthrough

### COMPARISON.md
**Purpose:** Side-by-side .NET vs NestJS comparison
**Read when:** You want to translate C# to TypeScript
**Contains:**
- Line-by-line code comparisons
- Syntax differences
- Feature equivalents
- Best practices from both worlds

### README.md
**Purpose:** Technical reference and quick commands
**Read when:** You need quick info about features or commands
**Contains:**
- Feature list
- Installation instructions
- Testing examples with grpcurl
- Project structure

### PROJECT_GUIDE.md (You're Reading It!)
**Purpose:** Explain every file in the project
**Read when:** You want to understand what each file does

---

## ⚙️ Configuration Files

### package.json
```json
{
  "name": "GrpcJSServiceApi",
  "dependencies": { ... },
  "scripts": { ... }
}
```
**Equivalent to:** `.csproj` file in .NET
**Purpose:** Project metadata and dependencies
**What it does:**
- Lists all npm packages needed (like NuGet packages)
- Defines scripts (`npm run start:dev`, etc.)
- Specifies project name and version

**Key scripts:**
- `npm run start:dev` → Run with hot-reload (like dotnet watch)
- `npm run build` → Compile TypeScript to JavaScript
- `npm run start:prod` → Run production build

### tsconfig.json
```json
{
  "compilerOptions": {
    "target": "ES2021",
    "module": "commonjs",
    ...
  }
}
```
**Equivalent to:** Compiler settings in `.csproj`
**Purpose:** TypeScript compiler configuration
**What it does:**
- Tells TypeScript how to compile to JavaScript
- Sets target JavaScript version
- Configures module system

**You probably won't need to edit this.**

### nest-cli.json
```json
{
  "collection": "@nestjs/schematics",
  "sourceRoot": "src"
}
```
**Equivalent to:** .NET CLI settings
**Purpose:** NestJS CLI configuration
**What it does:**
- Configures NestJS build tools
- Sets source code location

**You probably won't need to edit this.**

### .gitignore
**Purpose:** Tells Git which files to ignore
**What it ignores:**
- `node_modules/` (too large)
- `dist/` (generated files)
- `.env` (secrets)
- Log files

---

## 📁 Source Code Files

### src/main.ts ⚡ ENTRY POINT
```typescript
async function bootstrap() {
  const app = await NestFactory.createMicroservice(...);
  await app.listen();
}
bootstrap();
```
**Equivalent to:** `Program.cs` in .NET
**Purpose:** Starts the gRPC server
**What it does:**
1. Creates a NestJS microservice
2. Configures gRPC transport
3. Loads proto files
4. Starts server on port 5195

**This is where the app starts!**

**When to edit:**
- Change port number
- Add new proto files
- Modify server configuration

---

### src/app.module.ts 📦 MODULE
```typescript
@Module({
  controllers: [GreeterController, ContactController],
})
export class AppModule {}
```
**Equivalent to:** Service registration in `Program.cs`
**Purpose:** Registers all services (controllers)
**What it does:**
- Lists all gRPC controllers
- Wires up dependency injection

**When to edit:**
- Add new controller classes
- Add providers (services, repositories, etc.)

---

### src/protos/greet.proto 📄 PROTO
```protobuf
service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}
```
**Equivalent to:** Same file in .NET project
**Purpose:** Defines the Greeter gRPC service contract
**What it defines:**
- Service: `Greeter`
- Method: `SayHello`
- Request: `HelloRequest { name: string }`
- Response: `HelloReply { message: string }`

**This is IDENTICAL to the .NET version!**

**When to edit:**
- Add new RPC methods
- Change message structure

---

### src/protos/contact.proto 📄 PROTO
```protobuf
service ContactService {
  rpc CreateContact (CreateContactRequest) returns (ContactReply);
  rpc GetContact (GetContactRequest) returns (ContactReply);
  ...
}
```
**Equivalent to:** Same file in .NET project
**Purpose:** Defines the Contact CRUD service contract
**What it defines:**
- Service: `ContactService`
- Methods: CreateContact, GetContact, GetAllContacts, UpdateContact, DeleteContact
- Messages: Contact, Address, PhoneNumber
- Enum: PhoneType

**This is IDENTICAL to the .NET version!**

---

### src/services/greeter.controller.ts 🎯 SERVICE
```typescript
@Controller()
export class GreeterController {
  @GrpcMethod('Greeter', 'SayHello')
  sayHello(request: HelloRequest): HelloReply {
    return { message: `Hello ${request.name}` };
  }
}
```
**Equivalent to:** `GreeterService.cs` in .NET
**Purpose:** Implements the Greeter service
**What it does:**
- Receives `HelloRequest` from client
- Returns `HelloReply` with greeting message

**Key differences from .NET:**
- Uses `@GrpcMethod` decorator instead of inheriting from base class
- Returns plain object instead of `Task<T>`
- No `ServerCallContext` parameter (optional)

**When to edit:**
- Change greeting logic
- Add logging
- Add new RPC methods

---

### src/services/contact.controller.ts 🎯 SERVICE
```typescript
@Controller()
export class ContactController {
  private static contacts = new Map<number, Contact>();
  
  @GrpcMethod('ContactService', 'CreateContact')
  createContact(request: CreateContactRequest): ContactReply { ... }
  
  @GrpcMethod('ContactService', 'GetContact')
  getContact(request: GetContactRequest): ContactReply { ... }
  
  // ... more methods
}
```
**Equivalent to:** `ContactServiceImpl.cs` in .NET
**Purpose:** Implements full CRUD operations for contacts
**What it does:**
- Stores contacts in memory (Map)
- Implements Create, Read, Update, Delete operations
- Validates requests
- Handles errors with proper gRPC status codes

**Methods:**
1. `createContact` - Create new contact with auto-generated ID
2. `getContact` - Retrieve contact by ID
3. `getAllContacts` - List all contacts
4. `updateContact` - Update existing contact
5. `deleteContact` - Remove contact

**Data storage:**
- In-memory `Map<number, Contact>`
- Seeded with 2 sample contacts (John Doe, Jane Smith)

**Key differences from .NET:**
- Uses `Map` instead of `Dictionary`
- No `lock` needed (JavaScript is single-threaded)
- Uses `@GrpcMethod` decorator
- Throws `RpcException` with different syntax

**When to edit:**
- Change business logic
- Add validation rules
- Connect to real database (replace Map)

---

## 🔄 How It All Works Together

```
1. User runs: npm run start:dev
         ↓
2. main.ts starts
         ↓
3. Creates NestJS microservice
         ↓
4. Loads app.module.ts
         ↓
5. Registers GreeterController & ContactController
         ↓
6. Loads greet.proto & contact.proto
         ↓
7. Starts gRPC server on port 5195
         ↓
8. Waits for client requests
         ↓
9. Client calls: greet.Greeter/SayHello
         ↓
10. NestJS routes to: GreeterController.sayHello()
         ↓
11. Method executes and returns response
         ↓
12. Response sent back to client
```

---

## 🎨 Visual Flow: SayHello Request

```
Client (grpcurl)
    │
    │ SayHello({ name: "World" })
    │
    ↓
main.ts (gRPC Server)
    │
    │ Routes request based on proto definition
    │
    ↓
app.module.ts
    │
    │ Finds GreeterController
    │
    ↓
greeter.controller.ts
    │
    │ @GrpcMethod('Greeter', 'SayHello')
    │ sayHello(request: HelloRequest)
    │
    │ return { message: `Hello ${request.name}` }
    │
    ↓
Client receives: { message: "Hello World" }
```

---

## 📝 Key Concepts

### Decorators
```typescript
@Controller()        // Marks class as a controller
@GrpcMethod()       // Maps method to gRPC service
```
Think of decorators as C# attributes (`[Controller]`, `[HttpGet]`, etc.)

### Interfaces vs Classes
- **NestJS**: Uses interfaces (lightweight, TypeScript only)
- **.NET**: Uses classes (generated from proto)

Both work the same at runtime!

### Async vs Sync
- **NestJS**: Can return `T` directly or `Promise<T>`
- **.NET**: Must return `Task<T>`

```typescript
// Both valid in NestJS:
sayHello(request): HelloReply { ... }           // Sync
async sayHello(request): Promise<HelloReply> { ... }  // Async
```

---

## 🚀 Next Steps

### 1. Run the Project
```bash
cd GrpcJSServiceApi
npm install
npm run start:dev
```

### 2. Test It
```bash
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
```

### 3. Make a Change
Edit `src/services/greeter.controller.ts`:
```typescript
return { message: `¡Hola ${request.name}!` };  // Spanish!
```
Save and it auto-reloads!

### 4. Add a New Method
1. Add to `src/protos/greet.proto`
2. Add method to `src/services/greeter.controller.ts`
3. Test with grpcurl

---

## ❓ Common Questions

**Q: Which file do I edit to change the port?**
A: `src/main.ts` - change `url: '0.0.0.0:5195'`

**Q: Which file implements the actual gRPC logic?**
A: `src/services/*.controller.ts` files

**Q: Which files define the API contract?**
A: `src/protos/*.proto` files

**Q: Which file starts the server?**
A: `src/main.ts`

**Q: How do I add a new gRPC service?**
A: 
1. Create proto file in `src/protos/`
2. Create controller in `src/services/`
3. Register in `src/app.module.ts`
4. Add proto to `src/main.ts`

---

## 🎉 Summary

You now have a complete understanding of every file in the project!

**Core Files:**
- `main.ts` → Starts server
- `app.module.ts` → Registers services
- `*.proto` → Defines API
- `*.controller.ts` → Implements logic

**Config Files:**
- `package.json` → Dependencies
- `tsconfig.json` → Compiler settings

**Documentation:**
- Read QUICKSTART.md next to get it running!

Happy coding! 🚀
