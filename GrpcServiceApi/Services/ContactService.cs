using Grpc.Core;

namespace GrpcServiceApi.Services;

/// <summary>
/// gRPC service implementation for Contact management with CRUD operations.
/// Uses in-memory storage for demonstration purposes.
/// </summary>
/// <remarks>
/// This service demonstrates:
/// - CRUD operations (Create, Read, Update, Delete)
/// - Working with nested proto messages (Address, PhoneNumber)
/// - Working with repeated fields (List of phone numbers)
/// - Working with enums (PhoneType)
/// - Error handling with gRPC status codes
/// - Thread-safe operations with lock
/// 
/// For production, replace the in-memory dictionary with a real database (Entity Framework, Dapper, etc.)
/// </remarks>
public class ContactServiceImpl : ContactService.ContactServiceBase
{
    /// <summary>
    /// In-memory storage for contacts. Key: Contact ID, Value: Contact object.
    /// In production, replace this with a database.
    /// </summary>
    private static readonly Dictionary<int, Contact> Contacts = new();
    
    /// <summary>
    /// Thread-safe lock for accessing the contacts' dictionary.
    /// </summary>
    private static readonly Lock Lock = new();
    
    /// <summary>
    /// Auto-incrementing ID for new contacts.
    /// </summary>
    private static int _nextId = 1;

    /// <summary>
    /// Static constructor to seed some initial data for testing.
    /// </summary>
    static ContactServiceImpl()
    {
        // Seed initial data
        Contacts[1] = new Contact
        {
            Id = 1,
            Name = "John Doe",
            Address = new Address
            {
                Street = "123 Main St",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94102",
                Country = "USA"
            },
            PhoneNumbers =
            {
                new PhoneNumber { Number = "+1-555-1234", Type = PhoneType.Mobile },
                new PhoneNumber { Number = "+1-555-5678", Type = PhoneType.Home }
            }
        };

        Contacts[2] = new Contact
        {
            Id = 2,
            Name = "Jane Smith",
            Address = new Address
            {
                Street = "456 Oak Ave",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "90001",
                Country = "USA"
            },
            PhoneNumbers =
            {
                new PhoneNumber { Number = "+1-555-9876", Type = PhoneType.Work }
            }
        };

        _nextId = 3;
    }

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="request">The create contact request containing name, address, and phone numbers.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A ContactReply with the created contact and success status.</returns>
    /// <remarks>
    /// This method:
    /// 1. Validates the request (name is required)
    /// 2. Generates a new unique ID
    /// 3. Creates a new Contact proto message with nested Address and PhoneNumber objects
    /// 4. Stores the contact in memory
    /// 5. Returns the created contact
    /// 
    /// Example usage:
    /// <code>
    /// var request = new CreateContactRequest
    /// {
    ///     Name = "John Doe",
    ///     Address = new Address { Street = "123 Main St", City = "SF", State = "CA", ZipCode = "94102", Country = "USA" },
    ///     PhoneNumbers = { new PhoneNumber { Number = "+1-555-1234", Type = PhoneType.Mobile } }
    /// };
    /// </code>
    /// </remarks>
    public override Task<ContactReply> CreateContact(CreateContactRequest request, ServerCallContext context)
    {
        // Validate request
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required"));
        }

        lock (Lock)
        {
            // Create new contact with auto-generated ID
            var contact = new Contact
            {
                Id = _nextId++,
                Name = request.Name,
                Address = request.Address ?? new Address(), // Handle null address
                PhoneNumbers = { request.PhoneNumbers } // Copy all phone numbers
            };

            // Store in memory
            Contacts[contact.Id] = contact;

            // Return success response
            return Task.FromResult(new ContactReply
            {
                Contact = contact,
                Success = true,
                Message = $"Contact '{contact.Name}' created successfully with ID {contact.Id}"
            });
        }
    }

    /// <summary>
    /// Gets a contact by ID.
    /// </summary>
    /// <param name="request">The get contact request containing the contact ID.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A ContactReply with the requested contact.</returns>
    /// <remarks>
    /// Returns gRPC NotFound status if the contact doesn't exist.
    /// </remarks>
    public override Task<ContactReply> GetContact(GetContactRequest request, ServerCallContext context)
    {
        lock (Lock)
        {
            // Check if contact exists
            if (!Contacts.TryGetValue(request.Id, out var contact))
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Contact with ID {request.Id} not found"));
            }

            // Return the contact
            return Task.FromResult(new ContactReply
            {
                Contact = contact,
                Success = true,
                Message = "Contact retrieved successfully"
            });
        }
    }

    /// <summary>
    /// Gets all contacts.
    /// </summary>
    /// <param name="request">Empty request (could be extended with pagination/filters).</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A ContactListReply containing all contacts and total count.</returns>
    /// <remarks>
    /// In production, implement pagination for large datasets:
    /// - Add page_size and page_number to GetAllContactsRequest
    /// - Use LINQ Skip() and Take() for pagination
    /// - Consider adding sorting and filtering options
    /// </remarks>
    public override Task<ContactListReply> GetAllContacts(GetAllContactsRequest request, ServerCallContext context)
    {
        lock (Lock)
        {
            var reply = new ContactListReply
            {
                TotalCount = Contacts.Count
            };

            // Add all contacts to the reply
            reply.Contacts.AddRange(Contacts.Values);

            return Task.FromResult(reply);
        }
    }

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="request">The update request with contact ID and new data.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A ContactReply with the updated contact.</returns>
    /// <remarks>
    /// This performs a full update (replaces all fields).
    /// For partial updates, consider adding a separate UpdateContactPartial RPC with field mask support.
    /// </remarks>
    public override Task<ContactReply> UpdateContact(UpdateContactRequest request, ServerCallContext context)
    {
        // Validate request
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required"));
        }

        lock (Lock)
        {
            // Check if contact exists
            if (!Contacts.ContainsKey(request.Id))
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Contact with ID {request.Id} not found"));
            }

            // Update the contact
            var contact = new Contact
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address ?? new Address(),
                PhoneNumbers = { request.PhoneNumbers }
            };

            Contacts[request.Id] = contact;

            return Task.FromResult(new ContactReply
            {
                Contact = contact,
                Success = true,
                Message = $"Contact '{contact.Name}' updated successfully"
            });
        }
    }

    /// <summary>
    /// Deletes a contact by ID.
    /// </summary>
    /// <param name="request">The delete request containing the contact ID.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A DeleteContactReply with success status.</returns>
    public override Task<DeleteContactReply> DeleteContact(DeleteContactRequest request, ServerCallContext context)
    {
        lock (Lock)
        {
            // Check if contact exists
            if (!Contacts.ContainsKey(request.Id))
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Contact with ID {request.Id} not found"));
            }

            // Remove the contact
            var contact = Contacts[request.Id];
            Contacts.Remove(request.Id);

            return Task.FromResult(new DeleteContactReply
            {
                Success = true,
                Message = $"Contact '{contact.Name}' (ID: {request.Id}) deleted successfully"
            });
        }
    }
}
