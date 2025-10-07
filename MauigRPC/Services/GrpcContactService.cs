using Grpc.Net.Client;
using GrpcServiceApi;
using System.Net;

namespace MauigRPC.Services;

/// <summary>
/// Client-side service wrapper for communicating with the ContactService gRPC API.
/// Handles gRPC channel creation and provides methods for CRUD operations on contacts.
/// </summary>
/// <remarks>
/// This service manages contact operations including:
/// - Creating new contacts with nested Address and PhoneNumber data
/// - Retrieving single or multiple contacts
/// - Updating existing contacts
/// - Deleting contacts
/// 
/// Uses the same gRPC channel configuration as GrpcGreeterService for optimal performance.
/// </remarks>
public class GrpcContactService : IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly ContactService.ContactServiceClient _client;

    /// <summary>
    /// Initializes a new instance of the GrpcContactService.
    /// Creates and configures the gRPC channel with platform-specific settings.
    /// </summary>
    public GrpcContactService()
    {
        var address = GetServerAddress();
        
        _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = CreateHttpHandler()
        });
        
        _client = new ContactService.ContactServiceClient(_channel);
    }

    /// <summary>
    /// Gets the appropriate gRPC server address based on the current platform.
    /// </summary>
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
    private static HttpMessageHandler CreateHttpHandler()
    {
#if ANDROID
        var handler = new Xamarin.Android.Net.AndroidMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
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
        handler.SslOptions.RemoteCertificateValidationCallback = 
            (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        
        return handler;
#endif
    }

    /// <summary>
    /// Creates a new contact on the server.
    /// </summary>
    /// <param name="name">Contact name (required).</param>
    /// <param name="address">Contact address (optional).</param>
    /// <param name="phoneNumbers">List of phone numbers (optional).</param>
    /// <returns>ContactReply with the created contact including server-generated ID.</returns>
    public async Task<ContactReply> CreateContactAsync(
        string name, 
        Address? address = null, 
        IEnumerable<PhoneNumber>? phoneNumbers = null)
    {
        var request = new CreateContactRequest
        {
            Name = name,
            Address = address ?? new Address()
        };

        if (phoneNumbers != null)
        {
            request.PhoneNumbers.AddRange(phoneNumbers);
        }

        return await _client.CreateContactAsync(request);
    }

    /// <summary>
    /// Gets a single contact by ID.
    /// </summary>
    /// <param name="id">The contact ID.</param>
    /// <returns>ContactReply with the contact data.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown if contact not found (StatusCode.NotFound).</exception>
    public async Task<ContactReply> GetContactAsync(int id)
    {
        var request = new GetContactRequest { Id = id };
        return await _client.GetContactAsync(request);
    }

    /// <summary>
    /// Gets all contacts from the server.
    /// </summary>
    /// <returns>ContactListReply with all contacts and total count.</returns>
    public async Task<ContactListReply> GetAllContactsAsync()
    {
        var request = new GetAllContactsRequest();
        return await _client.GetAllContactsAsync(request);
    }

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="id">The contact ID to update.</param>
    /// <param name="name">New contact name.</param>
    /// <param name="address">New address.</param>
    /// <param name="phoneNumbers">New list of phone numbers.</param>
    /// <returns>ContactReply with the updated contact.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown if contact not found (StatusCode.NotFound).</exception>
    public async Task<ContactReply> UpdateContactAsync(
        int id,
        string name,
        Address? address = null,
        IEnumerable<PhoneNumber>? phoneNumbers = null)
    {
        var request = new UpdateContactRequest
        {
            Id = id,
            Name = name,
            Address = address ?? new Address()
        };

        if (phoneNumbers != null)
        {
            request.PhoneNumbers.AddRange(phoneNumbers);
        }

        return await _client.UpdateContactAsync(request);
    }

    /// <summary>
    /// Deletes a contact by ID.
    /// </summary>
    /// <param name="id">The contact ID to delete.</param>
    /// <returns>DeleteContactReply with success status.</returns>
    /// <exception cref="Grpc.Core.RpcException">Thrown if contact not found (StatusCode.NotFound).</exception>
    public async Task<DeleteContactReply> DeleteContactAsync(int id)
    {
        var request = new DeleteContactRequest { Id = id };
        return await _client.DeleteContactAsync(request);
    }

    /// <summary>
    /// Disposes the gRPC channel and releases all associated resources.
    /// </summary>
    public void Dispose()
    {
        _channel?.Dispose();
    }
}
