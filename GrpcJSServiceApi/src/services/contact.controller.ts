import { Controller } from '@nestjs/common';
import { GrpcMethod, RpcException } from '@nestjs/microservices';
import { status } from '@grpc/grpc-js';

/**
 * Enum for phone types
 */
enum PhoneType {
  MOBILE = 0,
  HOME = 1,
  WORK = 2,
}

/**
 * Interface for PhoneNumber
 */
interface PhoneNumber {
  number: string;
  type: PhoneType;
}

/**
 * Interface for Address
 */
interface Address {
  street: string;
  city: string;
  state: string;
  zip_code: string;
  country: string;
}

/**
 * Interface for Contact
 */
interface Contact {
  id: number;
  name: string;
  address?: Address;
  phone_numbers: PhoneNumber[];
}

/**
 * Interface for CreateContactRequest
 */
interface CreateContactRequest {
  name: string;
  address?: Address;
  phone_numbers: PhoneNumber[];
}

/**
 * Interface for GetContactRequest
 */
interface GetContactRequest {
  id: number;
}

/**
 * Interface for GetAllContactsRequest
 */
interface GetAllContactsRequest {
  // Empty for now, but could add filters
}

/**
 * Interface for UpdateContactRequest
 */
interface UpdateContactRequest {
  id: number;
  name: string;
  address?: Address;
  phone_numbers: PhoneNumber[];
}

/**
 * Interface for DeleteContactRequest
 */
interface DeleteContactRequest {
  id: number;
}

/**
 * Interface for ContactReply
 */
interface ContactReply {
  contact?: Contact;
  success: boolean;
  message: string;
}

/**
 * Interface for ContactListReply
 */
interface ContactListReply {
  contacts: Contact[];
  total_count: number;
}

/**
 * Interface for DeleteContactReply
 */
interface DeleteContactReply {
  success: boolean;
  message: string;
}

/**
 * gRPC service implementation for Contact management with CRUD operations.
 * Uses in-memory storage for demonstration purposes.
 * 
 * This service demonstrates:
 * - CRUD operations (Create, Read, Update, Delete)
 * - Working with nested proto messages (Address, PhoneNumber)
 * - Working with repeated fields (Array of phone numbers)
 * - Working with enums (PhoneType)
 * - Error handling with gRPC status codes
 * 
 * For production, replace the in-memory Map with a real database (TypeORM, Prisma, etc.)
 */
@Controller()
export class ContactController {
  /**
   * In-memory storage for contacts. Key: Contact ID, Value: Contact object.
   * In production, replace this with a database.
   */
  private static contacts: Map<number, Contact> = new Map();

  /**
   * Auto-incrementing ID for new contacts.
   */
  private static nextId = 1;

  /**
   * Static initializer to seed some initial data for testing.
   */
  static {
    // Seed initial data
    ContactController.contacts.set(1, {
      id: 1,
      name: 'John Doe',
      address: {
        street: '123 Main St',
        city: 'San Francisco',
        state: 'CA',
        zip_code: '94102',
        country: 'USA',
      },
      phone_numbers: [
        { number: '+1-555-1234', type: PhoneType.MOBILE },
        { number: '+1-555-5678', type: PhoneType.HOME },
      ],
    });

    ContactController.contacts.set(2, {
      id: 2,
      name: 'Jane Smith',
      address: {
        street: '456 Oak Ave',
        city: 'Los Angeles',
        state: 'CA',
        zip_code: '90001',
        country: 'USA',
      },
      phone_numbers: [{ number: '+1-555-9876', type: PhoneType.WORK }],
    });

    ContactController.nextId = 3;
  }

  /**
   * Creates a new contact.
   * 
   * @param request - The create contact request containing name, address, and phone numbers.
   * @returns A ContactReply with the created contact and success status.
   * 
   * @remarks
   * This method:
   * 1. Validates the request (name is required)
   * 2. Generates a new unique ID
   * 3. Creates a new Contact object with nested Address and PhoneNumber objects
   * 4. Stores the contact in memory
   * 5. Returns the created contact
   * 
   * Example usage:
   * ```
   * const request = {
   *   name: 'John Doe',
   *   address: { street: '123 Main St', city: 'SF', state: 'CA', zip_code: '94102', country: 'USA' },
   *   phone_numbers: [{ number: '+1-555-1234', type: PhoneType.MOBILE }]
   * };
   * ```
   */
  @GrpcMethod('ContactService', 'CreateContact')
  createContact(request: CreateContactRequest): ContactReply {
    // Validate request
    if (!request.name || request.name.trim() === '') {
      throw new RpcException({
        code: status.INVALID_ARGUMENT,
        message: 'Name is required',
      });
    }

    // Create new contact with auto-generated ID
    const contact: Contact = {
      id: ContactController.nextId++,
      name: request.name,
      address: request.address || {
        street: '',
        city: '',
        state: '',
        zip_code: '',
        country: '',
      },
      phone_numbers: request.phone_numbers || [],
    };

    // Store in memory
    ContactController.contacts.set(contact.id, contact);

    // Return success response
    return {
      contact,
      success: true,
      message: `Contact '${contact.name}' created successfully with ID ${contact.id}`,
    };
  }

  /**
   * Gets a contact by ID.
   * 
   * @param request - The get contact request containing the contact ID.
   * @returns A ContactReply with the requested contact.
   * 
   * @remarks
   * Returns gRPC NOT_FOUND status if the contact doesn't exist.
   */
  @GrpcMethod('ContactService', 'GetContact')
  getContact(request: GetContactRequest): ContactReply {
    // Check if contact exists
    const contact = ContactController.contacts.get(request.id);
    if (!contact) {
      throw new RpcException({
        code: status.NOT_FOUND,
        message: `Contact with ID ${request.id} not found`,
      });
    }

    // Return the contact
    return {
      contact,
      success: true,
      message: 'Contact retrieved successfully',
    };
  }

  /**
   * Gets all contacts.
   * 
   * @param request - Empty request (could be extended with pagination/filters).
   * @returns A ContactListReply containing all contacts and total count.
   * 
   * @remarks
   * In production, implement pagination for large datasets:
   * - Add page_size and page_number to GetAllContactsRequest
   * - Use array slicing for pagination
   * - Consider adding sorting and filtering options
   */
  @GrpcMethod('ContactService', 'GetAllContacts')
  getAllContacts(request: GetAllContactsRequest): ContactListReply {
    const contacts = Array.from(ContactController.contacts.values());

    return {
      contacts,
      total_count: contacts.length,
    };
  }

  /**
   * Updates an existing contact.
   * 
   * @param request - The update request with contact ID and new data.
   * @returns A ContactReply with the updated contact.
   * 
   * @remarks
   * This performs a full update (replaces all fields).
   * For partial updates, consider adding a separate UpdateContactPartial RPC with field mask support.
   */
  @GrpcMethod('ContactService', 'UpdateContact')
  updateContact(request: UpdateContactRequest): ContactReply {
    // Validate request
    if (!request.name || request.name.trim() === '') {
      throw new RpcException({
        code: status.INVALID_ARGUMENT,
        message: 'Name is required',
      });
    }

    // Check if contact exists
    if (!ContactController.contacts.has(request.id)) {
      throw new RpcException({
        code: status.NOT_FOUND,
        message: `Contact with ID ${request.id} not found`,
      });
    }

    // Update the contact
    const contact: Contact = {
      id: request.id,
      name: request.name,
      address: request.address || {
        street: '',
        city: '',
        state: '',
        zip_code: '',
        country: '',
      },
      phone_numbers: request.phone_numbers || [],
    };

    ContactController.contacts.set(request.id, contact);

    return {
      contact,
      success: true,
      message: `Contact '${contact.name}' updated successfully`,
    };
  }

  /**
   * Deletes a contact by ID.
   * 
   * @param request - The delete request containing the contact ID.
   * @returns A DeleteContactReply with success status.
   */
  @GrpcMethod('ContactService', 'DeleteContact')
  deleteContact(request: DeleteContactRequest): DeleteContactReply {
    // Check if contact exists
    const contact = ContactController.contacts.get(request.id);
    if (!contact) {
      throw new RpcException({
        code: status.NOT_FOUND,
        message: `Contact with ID ${request.id} not found`,
      });
    }

    // Remove the contact
    ContactController.contacts.delete(request.id);

    return {
      success: true,
      message: `Contact '${contact.name}' (ID: ${request.id}) deleted successfully`,
    };
  }
}
