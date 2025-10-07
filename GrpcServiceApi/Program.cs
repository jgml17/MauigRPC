using GrpcServiceApi.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to support HTTP/2 without TLS (for development)
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on HTTP port with HTTP/2 support
    options.ListenLocalhost(5194, o => o.Protocols = HttpProtocols.Http2);
});

// Add services to the container.
builder.Services.AddGrpc();

#if DEBUG
// Enable gRPC reflection for development (Postman, gRPC UI, grpcurl)
builder.Services.AddGrpcReflection();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ContactServiceImpl>();

#if DEBUG
// Map gRPC reflection service for development tools (Postman, gRPC UI, grpcurl)
app.MapGrpcReflectionService();
#endif

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();