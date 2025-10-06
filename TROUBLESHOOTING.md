# Troubleshooting Guide

## Common Issues

### 1. "Bad gRPC response. Response protocol downgraded to HTTP/1.1"

**Cause**: The gRPC client requires HTTP/2, but the server is responding with HTTP/1.1.

**Solutions**:

#### Server Configuration (Already Fixed)
The server is now explicitly configured to use HTTP/2:
```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5194, o => o.Protocols = HttpProtocols.Http2);
});
```

#### Client Configuration (Already Fixed)
- macOS/iOS: Uses `SocketsHttpHandler` with proper HTTP/2 support
- Android: Uses `AndroidMessageHandler` for platform-specific HTTP/2 handling

**Verification Steps**:
1. Make sure you're running the server with the updated code
2. Restart the server completely (stop and start again)
3. Rebuild the MAUI client
4. Test the connection

### 2. Connection Refused / Cannot Connect

**Platform-Specific Solutions**:

#### macOS Catalyst / iOS Simulator
- Server address: `http://localhost:5194`
- Ensure the server is running on the host machine
- Check firewall settings

#### Android Emulator
- Server address: `http://10.0.2.2:5194`
- `10.0.2.2` is the special IP that Android emulator uses to reach the host's localhost
- Make sure the server is running on HTTP (not HTTPS only)

#### Physical Devices
- Find your computer's local IP address:
  ```bash
  # macOS
  ipconfig getifaddr en0
  # or
  ifconfig | grep "inet "
  ```
- Update `GrpcGreeterService.cs` to use your IP:
  ```csharp
  return "http://YOUR_IP_ADDRESS:5194";
  ```
- Ensure both devices are on the same network
- Check firewall settings to allow incoming connections on port 5194

### 3. SSL/TLS Certificate Errors

**Current Setup (Development Only)**:
- The client bypasses SSL certificate validation in DEBUG mode
- This is for development only and should be removed in production

**For Production**:
- Use proper SSL certificates
- Remove certificate validation bypass
- Use HTTPS endpoint

### 4. Timeout Errors

**Solutions**:
- Increase timeout in `GrpcGreeterService.cs`:
  ```csharp
  var handler = new SocketsHttpHandler
  {
      PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
      KeepAlivePingDelay = TimeSpan.FromSeconds(60),
      KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
  };
  ```
- Check network connectivity
- Ensure server is running and accessible

## Testing Connectivity

### Test Server is Running
```bash
# Check if server is listening
curl http://localhost:5194
# Should return a message about gRPC endpoints

# Test from another terminal
netstat -an | grep 5194
# Should show the port is listening
```

### Test from MAUI App
The app displays detailed error messages in the response label. Check these for specific issues.

## Debug Mode Logging

To enable more detailed logging:

### Server Side
Update `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug",
      "Grpc": "Debug"
    }
  }
}
```

### Client Side (MAUI)
Add logging to `MauiProgram.cs`:
```csharp
#if DEBUG
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif
```

## Quick Fix Checklist

- [ ] Server is running on `http://localhost:5194`
- [ ] Server is using HTTP/2 protocol (check Program.cs)
- [ ] Client is using correct address for platform
- [ ] Firewall allows connections on port 5194
- [ ] Both projects are rebuilt with latest changes
- [ ] Server restarted after configuration changes
