# GrpcJSServiceApi

NestJS-based gRPC service API equivalent to the .NET GrpcServiceApi.

## Features

- **Greeter Service**: Simple greeting service with SayHello RPC
- **Contact Service**: Full CRUD operations for contact management
  - CreateContact
  - GetContact
  - GetAllContacts
  - UpdateContact
  - DeleteContact

## Prerequisites

- Node.js 18+ and npm

## Installation

```bash
npm install
```

## Running the Application

### Development Mode

```bash
npm run start:dev
```

### Production Mode

```bash
npm run build
npm run start:prod
```

The gRPC server will be available at `localhost:5195` (HTTP/2).

## Project Structure

```
GrpcJSServiceApi/
├── src/
│   ├── protos/           # Protocol Buffer definitions
│   │   ├── greet.proto
│   │   └── contact.proto
│   ├── services/         # gRPC service implementations
│   │   ├── greeter.controller.ts
│   │   └── contact.controller.ts
│   ├── interfaces/       # TypeScript interfaces (generated from protos)
│   ├── app.module.ts     # Application module
│   └── main.ts          # Application entry point
├── package.json
├── tsconfig.json
└── nest-cli.json
```

## Testing with grpcurl

```bash
# List services
grpcurl -plaintext localhost:5195 list

# Call SayHello
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello

# Get all contacts
grpcurl -plaintext -d '{}' localhost:5195 contact.ContactService/GetAllContacts

# Get a contact by ID
grpcurl -plaintext -d '{"id": 1}' localhost:5195 contact.ContactService/GetContact

# Create a new contact
grpcurl -plaintext -d '{
  "name": "John Doe",
  "address": {
    "street": "123 Main St",
    "city": "San Francisco",
    "state": "CA",
    "zip_code": "94102",
    "country": "USA"
  },
  "phone_numbers": [
    {"number": "+1-555-1234", "type": "MOBILE"}
  ]
}' localhost:5195 contact.ContactService/CreateContact
```

## Differences from .NET Version

- Port: 5195 instead of 5194 (to avoid conflicts)
- Framework: NestJS instead of ASP.NET Core
- Language: TypeScript instead of C#
- Same proto definitions and service functionality
