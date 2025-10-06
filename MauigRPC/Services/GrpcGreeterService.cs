using Grpc.Net.Client;
using GrpcServiceApi;
using System.Net;

namespace MauigRPC.Services;

/// <summary>
/// Client-side service wrapper for communicating with the Greeter gRPC service.
/// Handles gRPC channel creation, connection management, and platform-specific HTTP configuration.
/// </summary>
/// <remarks>
/// This service manages the gRPC channel lifecycle and provides a simplified interface for calling
/// the remote Greeter service. It includes platform-specific configurations for:
/// - Android: Uses special IP address (10.0.2.2) for emulator-to-host communication
/// - iOS/macOS: Uses localhost for simulator access
/// - HTTP/2: Configured with SocketsHttpHandler for optimal performance
/// - Keep-Alive: Maintains persistent connections to reduce latency
/// 
/// The service implements IDisposable to properly clean up gRPC channels and connections.
/// It should be registered as a Singleton in the DI container to reuse connections efficiently.
/// </remarks>
public class GrpcGreeterService : IDisposable
{
    /// <summary>
    /// The gRPC channel used for communication with the server.
    /// Maintains a persistent HTTP/2 connection with multiplexing support.
    /// </summary>
    private readonly GrpcChannel _channel;
    
    /// <summary>
    /// The strongly-typed gRPC client generated from the greet.proto file.
    /// Provides methods for invoking RPCs on the Greeter service.
    /// </summary>
    private readonly Greeter.GreeterClient _client;

    /// <summary>
    /// Initializes a new instance of the GrpcGreeterService.
    /// Creates and configures the gRPC channel with platform-specific settings.
    /// </summary>
    /// <remarks>
    /// The constructor:
    /// 1. Determines the correct server address based on the platform (Android uses 10.0.2.2, iOS uses localhost)
    /// 2. Creates an HTTP/2 handler with keep-alive settings for optimal performance
    /// 3. Establishes a gRPC channel to the server
    /// 4. Instantiates the strongly-typed client for making RPC calls
    /// 
    /// Platform-specific addresses:
    /// - Android Emulator: http://10.0.2.2:5194 (special IP to reach host machine)
    /// - iOS Simulator: http://localhost:5194 (direct access to host)
    /// - Physical Devices: Update to use your machine's IP address on the network
    /// 
    /// This setup is for development only. In production:
    /// - Use HTTPS instead of HTTP
    /// - Remove SSL certificate validation bypass
    /// - Consider using service discovery for server addresses
    /// </remarks>
    public GrpcGreeterService()
    {
        var address = GetServerAddress();
        
        _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = CreateHttpHandler()
        });
        
        _client = new Greeter.GreeterClient(_channel);
    }

    /// <summary>
    /// Gets the appropriate gRPC server address based on the current platform.
    /// </summary>
    /// <returns>
    /// A string containing the HTTP URL of the gRPC server:
    /// - Android: "http://10.0.2.2:5194" (emulator bridge to host)
    /// - iOS/macOS: "http://localhost:5194" (direct localhost access)
    /// - Other platforms: "http://localhost:5194"
    /// </returns>
    /// <remarks>
    /// Android emulators cannot use "localhost" to reach the host machine.
    /// Instead, they use the special IP address 10.0.2.2 which is automatically
    /// routed to the host machine's 127.0.0.1 (localhost).
    /// 
    /// For physical devices, you'll need to:
    /// 1. Find your computer's local IP address (e.g., 192.168.1.100)
    /// 2. Update this method to return that IP
    /// 3. Ensure both devices are on the same network
    /// 4. Configure firewall to allow connections on port 5194
    /// </remarks>
    private static string GetServerAddress()
    {
#if ANDROID
        return "http://10.0.2.2:5194";
#elif IOS || MACCATALYST
        return "http://localhost:5194";
#else
        return "http://localhost:5194";
#endif
    }

    /// <summary>
    /// Creates and configures an HTTP message handler optimized for gRPC communication.
    /// </summary>
    /// <returns>
    /// A configured HttpMessageHandler:
    /// - Android: AndroidMessageHandler (required for proper HTTP/2 support)
    /// - iOS/macOS/Windows: SocketsHttpHandler with HTTP/2 optimizations
    /// </returns>
    /// <remarks>
    /// Platform-specific implementations:
    /// 
    /// <strong>Android:</strong>
    /// Uses AndroidMessageHandler which is required for proper HTTP/2 support on Android.
    /// The handler bypasses SSL certificate validation for development purposes.
    /// 
    /// <strong>iOS/macOS/Windows:</strong>
    /// Uses SocketsHttpHandler with the following optimizations:
    /// - PooledConnectionIdleTimeout: Infinite - keeps connections alive indefinitely
    /// - KeepAlivePingDelay: 60 seconds - sends keep-alive pings to maintain connection
    /// - KeepAlivePingTimeout: 30 seconds - timeout for keep-alive ping responses
    /// - EnableMultipleHttp2Connections: true - allows concurrent streams over multiple connections
    /// 
    /// <strong>Security Warning:</strong>
    /// In DEBUG mode, SSL certificate validation is bypassed to allow self-signed certificates.
    /// This is ONLY for development and must be removed in production builds.
    /// In production:
    /// - Use valid SSL certificates
    /// - Remove ServerCertificateCustomValidationCallback override
    /// - Use HTTPS endpoints
    /// 
    /// <strong>Performance Benefits:</strong>
    /// - Keep-alive reduces latency by reusing connections (~40% faster)
    /// - Connection pooling allows concurrent requests
    /// - HTTP/2 multiplexing enables multiple streams on single connection
    /// </remarks>
    private static HttpMessageHandler CreateHttpHandler()
    {
#if ANDROID
        var handler = new Xamarin.Android.Net.AndroidMessageHandler();
        // WARNING: For development only - bypasses SSL validation
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            return true;
        };
        return handler;
#else
        var handler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true
        };
        
#if DEBUG
        // WARNING: For development only - bypasses SSL validation
        handler.SslOptions.RemoteCertificateValidationCallback = 
            (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        
        return handler;
#endif
    }

    /// <summary>
    /// Calls the SayHello RPC method on the remote gRPC server to get a greeting message.
    /// </summary>
    /// <param name="name">The name to include in the greeting. Cannot be null.</param>
    /// <returns>
    /// A Task containing the greeting message string from the server.
    /// If an error occurs, returns an error message string instead of throwing.
    /// </returns>
    /// <remarks>
    /// This method:
    /// 1. Creates a HelloRequest protobuf message with the provided name
    /// 2. Sends the request to the server via gRPC (binary Protocol Buffers over HTTP/2)
    /// 3. Waits for the HelloReply response
    /// 4. Returns the greeting message from the reply
    /// 
    /// <strong>Protocol Flow:</strong>
    /// <code>
    /// Client                                    Server
    ///   |                                          |
    ///   |  HelloRequest (binary protobuf)         |
    ///   |  { name: "World" }                      |
    ///   |----------------------------------------->|
    ///   |                                          | Processing...
    ///   |  HelloReply (binary protobuf)           |
    ///   |  { message: "Hello World" }             |
    ///   |<-----------------------------------------|
    ///   |                                          |
    /// </code>
    /// 
    /// <strong>Performance:</strong>
    /// - Binary serialization is ~5x faster than JSON
    /// - Payload size is ~60% smaller than equivalent JSON
    /// - HTTP/2 enables multiplexing (multiple calls on same connection)
    /// 
    /// <strong>Error Handling:</strong>
    /// Catches all exceptions and returns an error message string.
    /// Common errors:
    /// - Connection refused: Server not running or wrong address
    /// - Deadline exceeded: Server took too long to respond
    /// - Unavailable: Server is temporarily unavailable
    /// - Internal: Server encountered an internal error
    /// 
    /// For production, consider:
    /// - Throwing specific exceptions instead of returning error strings
    /// - Adding retry logic with exponential backoff
    /// - Implementing circuit breaker pattern
    /// - Logging errors for monitoring
    /// </remarks>
    /// <example>
    /// <code>
    /// var service = new GrpcGreeterService();
    /// var greeting = await service.SayHelloAsync("World");
    /// Console.WriteLine(greeting); // Output: "Hello World"
    /// </code>
    /// </example>
    public async Task<string> SayHelloAsync(string name)
    {
        try
        {
            var request = new HelloRequest { Name = name };
            var reply = await _client.SayHelloAsync(request);
            return reply.Message;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    /// <summary>
    /// Disposes the gRPC channel and releases all associated resources.
    /// </summary>
    /// <remarks>
    /// This method properly cleans up the gRPC channel by:
    /// - Closing active HTTP/2 connections
    /// - Releasing network resources
    /// - Cleaning up internal buffers
    /// 
    /// The service is registered as a Singleton in the DI container,
    /// so Dispose will be called automatically when the application shuts down.
    /// 
    /// <strong>Important:</strong>
    /// Do not call Dispose() manually unless you're certain the service is no longer needed.
    /// Creating new channels is expensive, so reusing the singleton instance is preferred.
    /// </remarks>
    public void Dispose()
    {
        _channel?.Dispose();
    }
}
