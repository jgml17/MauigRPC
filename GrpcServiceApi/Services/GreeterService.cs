using Grpc.Core;
using GrpcServiceApi;

namespace GrpcServiceApi.Services;

/// <summary>
/// Implementation of the Greeter gRPC service.
/// Provides greeting functionality through gRPC calls using Protocol Buffers (protobuf) serialization.
/// </summary>
/// <remarks>
/// This service is automatically registered in the ASP.NET Core pipeline via MapGrpcService.
/// It inherits from the auto-generated Greeter.GreeterBase class created from the greet.proto file.
/// All gRPC services use HTTP/2 protocol for efficient binary communication.
/// </remarks>
public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    /// <summary>
    /// Logger instance for recording service operations and errors.
    /// </summary>
    private readonly ILogger<GreeterService> _logger = logger;

    /// <summary>
    /// Handles the SayHello RPC call by creating a personalized greeting message.
    /// </summary>
    /// <param name="request">The HelloRequest message containing the name to greet.</param>
    /// <param name="context">Server-side call context containing metadata, deadlines, and cancellation tokens.</param>
    /// <returns>A Task containing the HelloReply message with the greeting.</returns>
    /// <remarks>
    /// This method is called automatically when a client invokes the SayHello RPC.
    /// The request/response are serialized using Protocol Buffers for efficient binary transmission.
    /// 
    /// Example gRPC call:
    /// <code>
    /// var request = new HelloRequest { Name = "World" };
    /// var reply = await client.SayHelloAsync(request);
    /// // reply.Message = "Hello World"
    /// </code>
    /// 
    /// Performance: Protobuf serialization is ~5x faster than JSON and produces ~60% smaller payloads.
    /// </remarks>
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
