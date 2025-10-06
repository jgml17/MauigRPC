# gRPC Quick Reference Cheat Sheet

## Proto Syntax Basics

### Service Definition
```protobuf
syntax = "proto3";

option csharp_namespace = "YourNamespace";

service YourService {
  rpc MethodName (RequestType) returns (ResponseType);
  rpc StreamMethod (RequestType) returns (stream ResponseType);  // Server streaming
  rpc UploadMethod (stream RequestType) returns (ResponseType);  // Client streaming
  rpc ChatMethod (stream RequestType) returns (stream ResponseType);  // Bidirectional
}
```

### Message Types
```protobuf
message MyMessage {
  string field_name = 1;      // Field number (NOT the value!)
  int32 count = 2;
  bool is_active = 3;
  repeated string tags = 4;   // Array/List
  map<string, int32> scores = 5;  // Dictionary/Map
  optional string nickname = 6;   // Optional field (can be null)
  
  // Nested message
  Address address = 7;
  
  // Enum
  Status status = 8;
}

message Address {
  string street = 1;
  string city = 2;
}

enum Status {
  UNKNOWN = 0;  // Must start at 0
  ACTIVE = 1;
  INACTIVE = 2;
}
```

## Proto Data Types

| Proto Type | C# Type | Use For |
|------------|---------|---------|
| `string` | `string` | Text |
| `int32` | `int` | 32-bit integers |
| `int64` | `long` | 64-bit integers |
| `float` | `float` | Floating point |
| `double` | `double` | Double precision |
| `bool` | `bool` | True/false |
| `bytes` | `ByteString` | Binary data |
| `repeated Type` | `List<Type>` | Arrays |
| `map<K,V>` | `Dictionary<K,V>` | Maps |

## Server Implementation

### 1. Add Proto File
```xml
<!-- In .csproj -->
<ItemGroup>
  <Protobuf Include="Protos\myservice.proto" GrpcServices="Server" />
</ItemGroup>
```

### 2. Implement Service
```csharp
public class MyServiceImpl : MyService.MyServiceBase
{
    public override Task<ResponseType> MethodName(
        RequestType request,
        ServerCallContext context)
    {
        // Your logic here
        return Task.FromResult(new ResponseType
        {
            Field = "value"
        });
    }
    
    // Server streaming
    public override async Task StreamMethod(
        RequestType request,
        IServerStreamWriter<ResponseType> responseStream,
        ServerCallContext context)
    {
        for (int i = 0; i < 10; i++)
        {
            await responseStream.WriteAsync(new ResponseType { Data = i });
        }
    }
}
```

### 3. Register Service
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<MyServiceImpl>();
app.Run();
```

## Client Implementation

### 1. Add Proto File
```xml
<!-- In .csproj -->
<ItemGroup>
  <Protobuf Include="Protos\myservice.proto" GrpcServices="Client" />
</ItemGroup>

<ItemGroup>
  <PackageReference Include="Grpc.Net.Client" Version="2.64.0" />
  <PackageReference Include="Google.Protobuf" Version="3.27.0" />
  <PackageReference Include="Grpc.Tools" Version="2.64.0">
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
</ItemGroup>
```

### 2. Create Client Service
```csharp
public class MyGrpcService
{
    private readonly GrpcChannel _channel;
    private readonly MyService.MyServiceClient _client;
    
    public MyGrpcService()
    {
        var handler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true
        };
        
        _channel = GrpcChannel.ForAddress("http://localhost:5000", 
            new GrpcChannelOptions { HttpHandler = handler });
        _client = new MyService.MyServiceClient(_channel);
    }
    
    public async Task<ResponseType> CallMethodAsync(RequestType request)
    {
        return await _client.MethodNameAsync(request);
    }
}
```

### 3. Use in Code
```csharp
// Simple call
var request = new RequestType { Field = "value" };
var response = await client.MethodNameAsync(request);

// With deadline
var response = await client.MethodNameAsync(request, 
    deadline: DateTime.UtcNow.AddSeconds(5));

// With metadata (headers)
var metadata = new Metadata
{
    { "authorization", "Bearer token" }
};
var response = await client.MethodNameAsync(request, headers: metadata);

// Streaming
var call = client.StreamMethod(new RequestType());
await foreach (var response in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(response.Data);
}
```

## Error Handling

### Server Side
```csharp
// Throw specific error
throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

// Common status codes
StatusCode.OK            // Success
StatusCode.Cancelled     // Cancelled
StatusCode.NotFound      // Resource not found
StatusCode.AlreadyExists // Already exists
StatusCode.InvalidArgument // Invalid argument
StatusCode.Unauthenticated // Not authenticated
StatusCode.PermissionDenied // No permission
StatusCode.Internal      // Internal error
```

### Client Side
```csharp
try
{
    var response = await client.MethodAsync(request);
}
catch (RpcException ex)
{
    switch (ex.StatusCode)
    {
        case StatusCode.NotFound:
            Console.WriteLine("Resource not found");
            break;
        case StatusCode.Unauthenticated:
            Console.WriteLine("Authentication required");
            break;
        default:
            Console.WriteLine($"Error: {ex.Status.Detail}");
            break;
    }
}
```

## Common Patterns

### Request with Pagination
```protobuf
message ListRequest {
  int32 page = 1;
  int32 page_size = 2;
  string sort_by = 3;
}

message ListResponse {
  repeated Item items = 1;
  int32 total_count = 2;
  int32 page = 3;
  int32 page_size = 4;
}
```

### Standard CRUD
```protobuf
service ItemService {
  rpc CreateItem (CreateItemRequest) returns (Item);
  rpc GetItem (GetItemRequest) returns (Item);
  rpc UpdateItem (UpdateItemRequest) returns (Item);
  rpc DeleteItem (DeleteItemRequest) returns (DeleteItemResponse);
  rpc ListItems (ListItemsRequest) returns (ListItemsResponse);
}

message Item {
  int32 id = 1;
  string name = 2;
  int64 created_at = 3;
}

message CreateItemRequest {
  string name = 1;
}

message GetItemRequest {
  int32 id = 1;
}

message UpdateItemRequest {
  int32 id = 1;
  string name = 2;
}

message DeleteItemRequest {
  int32 id = 1;
}

message DeleteItemResponse {
  bool success = 1;
}

message ListItemsRequest {
  int32 page = 1;
  int32 page_size = 2;
}

message ListItemsResponse {
  repeated Item items = 1;
  int32 total = 2;
}
```

### Error Response Pattern
```protobuf
message ErrorResponse {
  string code = 1;
  string message = 2;
  repeated string details = 3;
}

message StandardResponse {
  oneof result {
    SuccessData data = 1;
    ErrorResponse error = 2;
  }
}
```

## Platform-Specific Configuration

### Android
```csharp
#if ANDROID
var handler = new Xamarin.Android.Net.AndroidMessageHandler();
handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
var address = "http://10.0.2.2:5000";  // Emulator to host
#endif
```

### iOS/macOS
```csharp
#if IOS || MACCATALYST
var handler = new SocketsHttpHandler();
var address = "http://localhost:5000";  // Direct localhost
#endif
```

### Windows
```csharp
#if WINDOWS
var handler = new SocketsHttpHandler();
var address = "http://localhost:5000";
#endif
```

## Versioning Best Practices

### ✅ DO
```protobuf
message User {
  string name = 1;
  string email = 2;
  // Add new fields with new numbers
  string phone = 3;  // New field - safe
  
  // Mark unused fields as reserved
  reserved 4, 5;
  reserved "old_field_name";
}
```

### ❌ DON'T
```protobuf
message User {
  string email = 1;  // DON'T change field numbers!
  string name = 2;   // DON'T reorder fields!
  // DON'T change field types
}
```

## Testing Commands

### grpcurl
```bash
# Install
brew install grpcurl  # macOS

# List services
grpcurl -plaintext localhost:5000 list

# List methods
grpcurl -plaintext localhost:5000 list mypackage.MyService

# Call method
grpcurl -plaintext -d '{"field": "value"}' \
  localhost:5000 mypackage.MyService/MethodName

# With metadata
grpcurl -plaintext \
  -H "authorization: Bearer token" \
  -d '{"field": "value"}' \
  localhost:5000 mypackage.MyService/MethodName
```

## Common Issues & Solutions

### Issue: "Response protocol downgraded to HTTP/1.1"
**Solution**: Configure server for HTTP/2
```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
});
```

### Issue: "Deadline Exceeded"
**Solution**: Increase timeout or optimize server
```csharp
var call = client.MethodAsync(request, 
    deadline: DateTime.UtcNow.AddSeconds(30));
```

### Issue: Android can't connect to localhost
**Solution**: Use 10.0.2.2 for emulator
```csharp
var address = "http://10.0.2.2:5000";
```

## Performance Tips

1. **Reuse Channels**: Create once, reuse for all calls
2. **Connection Pooling**: Enable multiple HTTP/2 connections
   ```csharp
   EnableMultipleHttp2Connections = true
   ```
3. **Keep-Alive**: Maintain persistent connections
   ```csharp
   KeepAlivePingDelay = TimeSpan.FromSeconds(60)
   ```
4. **Use Streaming**: For large datasets or real-time data
5. **Binary Data**: Use `bytes` type instead of base64 strings

## Security Checklist

- [ ] Use HTTPS in production (not HTTP)
- [ ] Remove SSL certificate bypass for production
- [ ] Implement authentication (JWT, OAuth, etc.)
- [ ] Use metadata for authorization tokens
- [ ] Validate all inputs
- [ ] Implement rate limiting
- [ ] Use deadlines to prevent hanging requests
- [ ] Log security events

## Quick Command Reference

```bash
# Build proto files
dotnet build

# Run server
dotnet run --project Server/Server.csproj

# Run client
dotnet run --project Client/Client.csproj

# Test with grpcurl
grpcurl -plaintext localhost:5000 list

# Clean and rebuild
dotnet clean && dotnet build
```

## Useful Resources

- **Proto3 Language Guide**: https://protobuf.dev/programming-guides/proto3/
- **gRPC Core Concepts**: https://grpc.io/docs/what-is-grpc/core-concepts/
- **C# gRPC Tutorial**: https://learn.microsoft.com/aspnet/core/grpc/
- **Status Codes**: https://grpc.github.io/grpc/core/md_doc_statuscodes.html
