/**
 * gRPC Client Library for Next.js
 * 
 * This is similar to the C# gRPC client in your MAUI app.
 * In C#, you use: var client = new Greeter.GreeterClient(channel);
 * In Next.js, we load proto files dynamically and create clients.
 */

import * as grpc from '@grpc/grpc-js';
import * as protoLoader from '@grpc/proto-loader';
import path from 'path';

// ========================================
// TYPES (Similar to C# generated classes)
// ========================================

export interface HelloRequest {
  name: string;
}

export interface HelloReply {
  message: string;
}

export interface Address {
  street: string;
  city: string;
  state: string;
  zip_code: string;
  country: string;
}

export interface PhoneNumber {
  number: string;
  type: 'MOBILE' | 'HOME' | 'WORK';
}

export interface Contact {
  id: number;
  name: string;
  address?: Address;
  phone_numbers: PhoneNumber[];
}

export interface CreateContactRequest {
  name: string;
  address?: Address;
  phone_numbers: PhoneNumber[];
}

export interface ContactReply {
  contact?: Contact;
  success: boolean;
  message: string;
}

export interface ContactListReply {
  contacts: Contact[];
  total_count: number;
}

// ========================================
// PROTO LOADING (Similar to AddGrpcClient in C#)
// ========================================

const PROTO_PATH = {
  greet: path.join(process.cwd(), 'proto', 'greet.proto'),
  contact: path.join(process.cwd(), 'proto', 'contact.proto'),
};

const packageDefinition = {
  greet: protoLoader.loadSync(PROTO_PATH.greet, {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
  }),
  contact: protoLoader.loadSync(PROTO_PATH.contact, {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
  }),
};

const greetProto = grpc.loadPackageDefinition(packageDefinition.greet) as any;
const contactProto = grpc.loadPackageDefinition(packageDefinition.contact) as any;

// ========================================
// CLIENT CREATION (Similar to C# channels)
// ========================================

/**
 * Creates a gRPC channel
 * C# equivalent: GrpcChannel.ForAddress("http://localhost:5195")
 */
function createChannel(address: string): grpc.ChannelCredentials {
  return grpc.credentials.createInsecure();
}

/**
 * Greeter Client
 * C# equivalent: var client = new Greeter.GreeterClient(channel);
 */
export class GreeterClient {
  private client: any;

  constructor(serverAddress: string = 'localhost:5195') {
    this.client = new greetProto.greet.Greeter(
      serverAddress,
      createChannel(serverAddress)
    );
  }

  /**
   * Call SayHello method
   * C# equivalent: await client.SayHelloAsync(new HelloRequest { Name = "World" });
   */
  async sayHello(name: string): Promise<HelloReply> {
    return new Promise((resolve, reject) => {
      this.client.SayHello({ name }, (error: any, response: HelloReply) => {
        if (error) {
          reject(error);
        } else {
          resolve(response);
        }
      });
    });
  }
}

/**
 * Contact Client
 * C# equivalent: var client = new ContactService.ContactServiceClient(channel);
 */
export class ContactClient {
  private client: any;

  constructor(serverAddress: string = 'localhost:5195') {
    this.client = new contactProto.contact.ContactService(
      serverAddress,
      createChannel(serverAddress)
    );
  }

  /**
   * Get all contacts
   * C# equivalent: await client.GetAllContactsAsync(new GetAllContactsRequest());
   */
  async getAllContacts(): Promise<ContactListReply> {
    return new Promise((resolve, reject) => {
      this.client.GetAllContacts({}, (error: any, response: ContactListReply) => {
        if (error) {
          reject(error);
        } else {
          resolve(response);
        }
      });
    });
  }

  /**
   * Get contact by ID
   * C# equivalent: await client.GetContactAsync(new GetContactRequest { Id = 1 });
   */
  async getContact(id: number): Promise<ContactReply> {
    return new Promise((resolve, reject) => {
      this.client.GetContact({ id }, (error: any, response: ContactReply) => {
        if (error) {
          reject(error);
        } else {
          resolve(response);
        }
      });
    });
  }

  /**
   * Create a new contact
   * C# equivalent: await client.CreateContactAsync(new CreateContactRequest { ... });
   */
  async createContact(request: CreateContactRequest): Promise<ContactReply> {
    return new Promise((resolve, reject) => {
      this.client.CreateContact(request, (error: any, response: ContactReply) => {
        if (error) {
          reject(error);
        } else {
          resolve(response);
        }
      });
    });
  }

  /**
   * Update a contact
   */
  async updateContact(id: number, request: CreateContactRequest): Promise<ContactReply> {
    return new Promise((resolve, reject) => {
      this.client.UpdateContact(
        { id, ...request },
        (error: any, response: ContactReply) => {
          if (error) {
            reject(error);
          } else {
            resolve(response);
          }
        }
      );
    });
  }

  /**
   * Delete a contact
   */
  async deleteContact(id: number): Promise<{success: boolean; message: string}> {
    return new Promise((resolve, reject) => {
      this.client.DeleteContact({ id }, (error: any, response: any) => {
        if (error) {
          reject(error);
        } else {
          resolve(response);
        }
      });
    });
  }
}
