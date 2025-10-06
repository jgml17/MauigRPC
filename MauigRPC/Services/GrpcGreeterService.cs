using Grpc.Net.Client;
using GrpcServiceApi;
using System.Net;

namespace MauigRPC.Services;

public class GrpcGreeterService : IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly Greeter.GreeterClient _client;

    public GrpcGreeterService()
    {
        // For development on Android emulator, use 10.0.2.2 instead of localhost
        // For iOS simulator, use localhost
        // For physical devices, use your machine's IP address
        
        var address = GetServerAddress();
        
        _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            // For development with self-signed certificates
            HttpHandler = CreateHttpHandler()
        });
        
        _client = new Greeter.GreeterClient(_channel);
    }

    private static string GetServerAddress()
    {
#if ANDROID
        // Android emulator uses 10.0.2.2 to reach host machine's localhost
        return "http://10.0.2.2:5194";
#elif IOS || MACCATALYST
        // iOS simulator can use localhost
        return "http://localhost:5194";
#else
        return "http://localhost:5194";
#endif
    }

    private static HttpMessageHandler CreateHttpHandler()
    {
#if ANDROID
        // Android requires special handler configuration for HTTP/2
        var handler = new Xamarin.Android.Net.AndroidMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            return true; // For development only
        };
        return handler;
#else
        // Use SocketsHttpHandler for better HTTP/2 support
        var handler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true
        };
        
        // For development only - bypass SSL certificate validation
        // Remove this in production!
#if DEBUG
        handler.SslOptions.RemoteCertificateValidationCallback = 
            (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        
        return handler;
#endif
    }

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

    public void Dispose()
    {
        _channel?.Dispose();
    }
}
