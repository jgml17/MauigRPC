# Entity Framework + gRPC Integration Example

This example shows how to integrate Entity Framework Core (traditional ORM) with gRPC services.

## Overview

```
Database (SQL/PostgreSQL/etc)
    ↓
Entity Framework Core (ORM - Object Relational Mapping)
    ↓
C# Entity Classes
    ↓
gRPC Service (maps entities to protobuf messages)
    ↓
Protocol Buffers (wire format)
    ↓
gRPC Client
```

## Step 1: Define Proto Messages

**user.proto**:
```protobuf
syntax = "proto3";

option csharp_namespace = "GrpcServiceApi";

service UserService {
  rpc GetUser (GetUserRequest) returns (User);
  rpc CreateUser (CreateUserRequest) returns (User);
  rpc ListUsers (ListUsersRequest) returns (ListUsersResponse);
  rpc UpdateUser (UpdateUserRequest) returns (User);
  rpc DeleteUser (DeleteUserRequest) returns (DeleteUserResponse);
}

message User {
  int32 id = 1;
  string name = 2;
  string email = 3;
  string phone = 4;
  int64 created_at = 5;
}

message GetUserRequest {
  int32 id = 1;
}

message CreateUserRequest {
  string name = 1;
  string email = 2;
  string phone = 3;
}

message UpdateUserRequest {
  int32 id = 1;
  string name = 2;
  string email = 3;
  string phone = 4;
}

message DeleteUserRequest {
  int32 id = 1;
}

message DeleteUserResponse {
  bool success = 1;
  string message = 2;
}

message ListUsersRequest {
  int32 page = 1;
  int32 page_size = 2;
}

message ListUsersResponse {
  repeated User users = 1;
  int32 total = 2;
}
```

## Step 2: Install NuGet Packages

```bash
# Add Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# Or for PostgreSQL:
# dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Add AutoMapper for object mapping
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

## Step 3: Create Entity Framework Models

**Models/UserEntity.cs**:
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcServiceApi.Models;

/// <summary>
/// Entity Framework model representing a User in the database.
/// This is separate from the protobuf User message for flexibility.
/// </summary>
[Table("Users")]
public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
```

## Step 4: Create DbContext

**Data/ApplicationDbContext.cs**:
```csharp
using Microsoft.EntityFrameworkCore;
using GrpcServiceApi.Models;

namespace GrpcServiceApi.Data;

/// <summary>
/// Entity Framework DbContext for database operations.
/// Acts as the ORM layer between objects and database.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure entity
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
```

## Step 5: Configure AutoMapper

**Mappings/MappingProfile.cs**:
```csharp
using AutoMapper;
using GrpcServiceApi.Models;

namespace GrpcServiceApi.Mappings;

/// <summary>
/// AutoMapper profile for mapping between EF entities and protobuf messages.
/// Similar to how an ORM maps between objects and database rows.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map from EF entity to protobuf message
        CreateMap<UserEntity, User>()
            .ForMember(dest => dest.CreatedAt, 
                opt => opt.MapFrom(src => new DateTimeOffset(src.CreatedAt).ToUnixTimeSeconds()));
        
        // Map from protobuf message to EF entity
        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.CreatedAt, 
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.CreatedAt).DateTime));
        
        // Map create request to entity
        CreateMap<CreateUserRequest, UserEntity>();
        
        // Map update request to entity
        CreateMap<UpdateUserRequest, UserEntity>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
```

## Step 6: Implement gRPC Service with EF Core

**Services/UserServiceImpl.cs**:
```csharp
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using GrpcServiceApi.Data;
using GrpcServiceApi.Models;

namespace GrpcServiceApi.Services;

/// <summary>
/// gRPC service implementation using Entity Framework Core as the ORM.
/// Demonstrates the pattern: gRPC ↔ EF Core ↔ Database
/// </summary>
public class UserServiceImpl : UserService.UserServiceBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserServiceImpl> _logger;
    
    public UserServiceImpl(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<UserServiceImpl> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Get a user by ID using Entity Framework.
    /// </summary>
    public override async Task<User> GetUser(GetUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", request.Id);
        
        // Use EF Core to query database (ORM layer)
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id);
        
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, 
                $"User with ID {request.Id} not found"));
        }
        
        // Use AutoMapper to convert EF entity to protobuf message
        return _mapper.Map<User>(userEntity);
    }
    
    /// <summary>
    /// Create a new user using Entity Framework.
    /// </summary>
    public override async Task<User> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Creating user: {UserName}", request.Name);
        
        // Check if email already exists
        var existingUser = await _context.Users
            .AnyAsync(u => u.Email == request.Email);
        
        if (existingUser)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, 
                $"User with email {request.Email} already exists"));
        }
        
        // Map request to EF entity
        var userEntity = _mapper.Map<UserEntity>(request);
        
        // Use EF Core to save to database (ORM layer)
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User created with ID: {UserId}", userEntity.Id);
        
        // Map entity back to protobuf message
        return _mapper.Map<User>(userEntity);
    }
    
    /// <summary>
    /// List users with pagination using Entity Framework.
    /// </summary>
    public override async Task<ListUsersResponse> ListUsers(
        ListUsersRequest request, 
        ServerCallContext context)
    {
        var pageSize = request.PageSize > 0 ? request.PageSize : 10;
        var page = request.Page > 0 ? request.Page : 1;
        var skip = (page - 1) * pageSize;
        
        _logger.LogInformation("Listing users - Page: {Page}, PageSize: {PageSize}", page, pageSize);
        
        // Use EF Core with pagination (ORM layer)
        var total = await _context.Users.CountAsync();
        
        var userEntities = await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        // Map entities to protobuf messages
        var users = _mapper.Map<List<User>>(userEntities);
        
        return new ListUsersResponse
        {
            Users = { users },
            Total = total
        };
    }
    
    /// <summary>
    /// Update a user using Entity Framework.
    /// </summary>
    public override async Task<User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", request.Id);
        
        // Find existing user
        var userEntity = await _context.Users.FindAsync(request.Id);
        
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, 
                $"User with ID {request.Id} not found"));
        }
        
        // Update entity properties
        userEntity.Name = request.Name;
        userEntity.Email = request.Email;
        userEntity.Phone = request.Phone;
        userEntity.UpdatedAt = DateTime.UtcNow;
        
        // Use EF Core to update database (ORM layer)
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User updated: {UserId}", userEntity.Id);
        
        // Map updated entity to protobuf message
        return _mapper.Map<User>(userEntity);
    }
    
    /// <summary>
    /// Delete a user using Entity Framework.
    /// </summary>
    public override async Task<DeleteUserResponse> DeleteUser(
        DeleteUserRequest request, 
        ServerCallContext context)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", request.Id);
        
        // Find user
        var userEntity = await _context.Users.FindAsync(request.Id);
        
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, 
                $"User with ID {request.Id} not found"));
        }
        
        // Use EF Core to delete from database (ORM layer)
        _context.Users.Remove(userEntity);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User deleted: {UserId}", request.Id);
        
        return new DeleteUserResponse
        {
            Success = true,
            Message = $"User {request.Id} deleted successfully"
        };
    }
}
```

## Step 7: Configure Services in Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
using GrpcServiceApi.Data;
using GrpcServiceApi.Services;
using GrpcServiceApi.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework (ORM)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

// Or for PostgreSQL:
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
// );

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure Kestrel for HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5194, o => o.Protocols = HttpProtocols.Http2);
});

// Add gRPC services
builder.Services.AddGrpc();

var app = builder.Build();

// Apply migrations on startup (optional)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Map gRPC services
app.MapGrpcService<UserServiceImpl>();

app.Run();
```

## Step 8: Add Connection String

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MauigRPC;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

## Step 9: Create Migration

```bash
# Install EF Core tools
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

## Architecture Benefits

### Traditional REST + ORM:
```
HTTP Request (JSON) → Controller → EF Core → Database
                                    ↓
HTTP Response (JSON) ← Controller ← EF Core ← Database
```

### gRPC + ORM (This Approach):
```
gRPC Request (Protobuf) → Service → EF Core → Database
                                      ↓
gRPC Response (Protobuf) ← Service ← EF Core ← Database
```

### Advantages:
1. **Best of Both Worlds**:
   - gRPC: Fast, binary, type-safe communication
   - EF Core: Powerful ORM for database operations

2. **Separation of Concerns**:
   - Protobuf messages: Wire format (client/server contract)
   - EF entities: Database schema
   - AutoMapper: Bridges the two

3. **Flexibility**:
   - Change database schema without changing gRPC API
   - Change gRPC API without changing database
   - Different field names/types on each layer

4. **Performance**:
   - EF Core: Optimized database queries
   - gRPC: Efficient binary serialization
   - AutoMapper: Fast object-to-object mapping

## Client Usage Example

```csharp
// Create user
var createRequest = new CreateUserRequest 
{ 
    Name = "John Doe",
    Email = "john@example.com",
    Phone = "+1234567890"
};
var user = await userClient.CreateUserAsync(createRequest);
Console.WriteLine($"Created user: {user.Id}");

// Get user
var getRequest = new GetUserRequest { Id = user.Id };
var fetchedUser = await userClient.GetUserAsync(getRequest);
Console.WriteLine($"User: {fetchedUser.Name} ({fetchedUser.Email})");

// List users with pagination
var listRequest = new ListUsersRequest { Page = 1, PageSize = 10 };
var response = await userClient.ListUsersAsync(listRequest);
Console.WriteLine($"Total users: {response.Total}");
foreach (var u in response.Users)
{
    Console.WriteLine($"- {u.Name} ({u.Email})");
}

// Update user
var updateRequest = new UpdateUserRequest
{
    Id = user.Id,
    Name = "John Updated",
    Email = "john.updated@example.com",
    Phone = "+9876543210"
};
var updatedUser = await userClient.UpdateUserAsync(updateRequest);

// Delete user
var deleteRequest = new DeleteUserRequest { Id = user.Id };
var deleteResponse = await userClient.DeleteUserAsync(deleteRequest);
Console.WriteLine(deleteResponse.Message);
```

## Summary

**"ORM for gRPC"** means:
- Use traditional ORM (Entity Framework) for database operations
- Use Protocol Buffers for serialization (built-in to gRPC)
- Use AutoMapper to bridge EF entities and protobuf messages
- Keep concerns separated: wire format vs database schema

This pattern gives you:
- ✅ Type-safe database access (EF Core)
- ✅ Efficient binary serialization (Protocol Buffers)
- ✅ Automatic code generation (both EF and gRPC)
- ✅ Separation of concerns (API contract vs data model)
- ✅ Best performance (gRPC + EF Core optimizations)
