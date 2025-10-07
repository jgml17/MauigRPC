# .NET vs NestJS: Side-by-Side Comparison

This document shows the **exact same functionality** implemented in both frameworks.

---

## 1. Project Entry Point

### .NET: `Program.cs`
```csharp
using GrpcServiceApi.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5194, o => o.Protocols = HttpProtocols.Http2);
});

// Add gRPC service
builder.Services.AddGrpc();

var app = builder.Build();

// Map gRPC services
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ContactServiceImpl>();

app.Run();
```

### NestJS: `main.ts`
```typescript
import { NestFactory } from '@nestjs/core';
import { MicroserviceOptions, Transport } from '@nestjs/microservices';
import { AppModule } from './app.module';
import { join } from 'path';

async function bootstrap() {
  const app = await NestFactory.createMicroservice<MicroserviceOptions>(
    AppModule,
    {
      transport: Transport.GRPC,
      options: {
        url: '0.0.0.0:5195',  // Port 5195 (different from .NET)
        package: ['greet', 'contact'],
        protoPath: [
          join(__dirname, '../src/protos/greet.proto'),
          join(__dirname, '../src/protos/contact.proto'),
        ],
      },
    },
  );

  await app.listen();
}

bootstrap();
```

**Key Differences:**
- .NET: Explicit Kestrel configuration
- NestJS: Configuration in options object
- .NET: Port 5194
- NestJS: Port 5195 (to avoid conflict)

---

## 2. Service Registration

### .NET: `Program.cs`
```csharp
// Services registered directly
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ContactServiceImpl>();
```

### NestJS: `app.module.ts`
```typescript
@Module({
  controllers: [GreeterController, ContactController],
})
export class AppModule {}
```

**Key Differences:**
- .NET: Services registered in Program.cs
- NestJS: Controllers registered in Module

---

## 3. Greeter Service Implementation

### .NET: `GreeterService.cs`
```csharp
public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(
        HelloRequest request, 
        ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
```

### NestJS: `greeter.controller.ts`
```typescript
@Controller()
export class GreeterController {
  
  @GrpcMethod('Greeter', 'SayHello')
  sayHello(request: HelloRequest): HelloReply {
    return {
      message: `Hello ${request.name}`,
    };
  }
}
```

**Key Differences:**
- .NET: Inherits from auto-generated base class
- NestJS: Uses `@GrpcMethod` decorator
- .NET: Returns `Task<T>`
- NestJS: Returns `T` directly (async is optional)
- .NET: Has `ServerCallContext` parameter
- NestJS: Context is optional

---

## 4. Contact Service - Create Method

### .NET: `ContactService.cs`
```csharp
public override Task<ContactReply> CreateContact(
    CreateContactRequest request, 
    ServerCallContext context)
{
    // Validate
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        throw new RpcException(new Status(
            StatusCode.InvalidArgument, 
            "Name is required"));
    }

    lock (Lock)
    {
        var contact = new Contact
        {
            Id = _nextId++,
            Name = request.Name,
            Address = request.Address ?? new Address(),
            PhoneNumbers = { request.PhoneNumbers }
        };

        Contacts[contact.Id] = contact;

        return Task.FromResult(new ContactReply
        {
            Contact = contact,
            Success = true,
            Message = $"Contact '{contact.Name}' created with ID {contact.Id}"
        });
    }
}
```

### NestJS: `contact.controller.ts`
```typescript
@GrpcMethod('ContactService', 'CreateContact')
createContact(request: CreateContactRequest): ContactReply {
  // Validate
  if (!request.name || request.name.trim() === '') {
    throw new RpcException({
      code: status.INVALID_ARGUMENT,
      message: 'Name is required',
    });
  }

  const contact: Contact = {
    id: ContactController.nextId++,
    name: request.name,
    address: request.address || {
      street: '', city: '', state: '', zip_code: '', country: ''
    },
    phone_numbers: request.phone_numbers || [],
  };

  ContactController.contacts.set(contact.id, contact);

  return {
    contact,
    success: true,
    message: `Contact '${contact.name}' created with ID ${contact.id}`,
  };
}
```

**Key Differences:**
- .NET: Uses `lock` for thread safety
- NestJS: JavaScript is single-threaded (no lock needed)
- .NET: Uses `Dictionary<int, Contact>`
- NestJS: Uses `Map<number, Contact>`
- .NET: `PhoneNumbers = { request.PhoneNumbers }` (collection initializer)
- NestJS: `phone_numbers: request.phone_numbers || []` (spread or default)

---

## 5. Error Handling

### .NET
```csharp
throw new RpcException(new Status(
    StatusCode.NotFound, 
    $"Contact with ID {request.Id} not found"
));
```

### NestJS
```typescript
throw new RpcException({
  code: status.NOT_FOUND,
  message: `Contact with ID ${request.id} not found`,
});
```

**Similar but different syntax:**
- Both use `RpcException`
- .NET: `StatusCode` enum
- NestJS: `status` object from `@grpc/grpc-js`

---

## 6. Data Storage

### .NET
```csharp
private static readonly Dictionary<int, Contact> Contacts = new();
private static readonly Lock Lock = new();
private static int _nextId = 1;

static ContactServiceImpl()
{
    // Seed data
    Contacts[1] = new Contact { ... };
}
```

### NestJS
```typescript
private static contacts: Map<number, Contact> = new Map();
private static nextId = 1;

static {
  // Seed data
  ContactController.contacts.set(1, { ... });
}
```

**Key Differences:**
- .NET: Uses `Dictionary` + `Lock` for thread safety
- NestJS: Uses `Map` (no lock needed)
- .NET: Static constructor
- NestJS: Static initialization block

---

## 7. Proto Files

### IDENTICAL IN BOTH!

```protobuf
syntax = "proto3";

package greet;

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

The proto files are the **same** - this is the beauty of gRPC!

---

## 8. Type Definitions

### .NET
```csharp
// Auto-generated from proto files
public class HelloRequest
{
    public string Name { get; set; }
}

public class HelloReply
{
    public string Message { get; set; }
}
```

### NestJS
```typescript
// Manually defined interfaces
interface HelloRequest {
  name: string;
}

interface HelloReply {
  message: string;
}
```

**Key Differences:**
- .NET: Auto-generated classes with properties
- NestJS: Manual interfaces (or you can use code generation tools)
- .NET: PascalCase (Name)
- NestJS: snake_case (name) to match proto field names

---

## 9. Running the Application

### .NET
```bash
dotnet run --project GrpcServiceApi
# Runs on http://localhost:5194
```

### NestJS
```bash
cd GrpcJSServiceApi
npm install
npm run start:dev
# Runs on http://localhost:5195
```

---

## 10. Testing (Identical!)

```bash
# .NET (port 5194)
grpcurl -plaintext -d '{"name": "World"}' localhost:5194 greet.Greeter/SayHello

# NestJS (port 5195)
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
```

Both return the same result:
```json
{
  "message": "Hello World"
}
```

---

## Summary Table

| Feature | .NET | NestJS |
|---------|------|--------|
| **Language** | C# | TypeScript |
| **Port** | 5194 | 5195 |
| **Base Class** | Inherit from generated base | Use decorators |
| **Return Type** | `Task<T>` | `T` or `Promise<T>` |
| **Threading** | Multi-threaded (needs locks) | Single-threaded (event loop) |
| **Data Structure** | `Dictionary<K,V>` | `Map<K,V>` |
| **Type Generation** | Auto-generated from proto | Manual interfaces |
| **Null Handling** | `?? new()` | `|| {}` |
| **String Interpolation** | `$"text {var}"` | `` `text ${var}` `` |
| **Error Status** | `StatusCode.NotFound` | `status.NOT_FOUND` |

---

## Which is Better?

**Neither!** They're **functionally identical**:
- Same proto definitions
- Same gRPC protocol
- Same wire format
- Compatible with same clients

**Choose based on:**
- **.NET**: If you prefer C#, have .NET ecosystem, need strong typing
- **NestJS**: If you prefer JavaScript/TypeScript, have Node.js ecosystem, need npm packages

Both implementations work exactly the same from a client's perspective!
