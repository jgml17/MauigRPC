/**
 * API Route for Contact Service
 * Similar to ContactService calls in your MAUI app
 */

import { NextRequest, NextResponse } from 'next/server';
import { ContactClient } from '@/lib/grpc-clients';

// Helper function to convert snake_case to camelCase
function convertContactToCamelCase(contact: any) {
  return {
    id: contact.id,
    name: contact.name,
    address: contact.address ? {
      street: contact.address.street,
      city: contact.address.city,
      state: contact.address.state,
      zipCode: contact.address.zip_code,
      country: contact.address.country,
    } : undefined,
    phoneNumbers: contact.phone_numbers || [],
  };
}

// GET all contacts or single contact by ID
export async function GET(request: NextRequest) {
  try {
    const { searchParams } = new URL(request.url);
    const id = searchParams.get('id');
    const server = searchParams.get('server') || 'nestjs';
    const serverAddress = server === 'dotnet' ? 'localhost:5194' : 'localhost:5195';
    
    const client = new ContactClient(serverAddress);
    
    // Get single contact by ID
    if (id) {
      const response = await client.getContact(parseInt(id));
      return NextResponse.json({
        success: response.success,
        contact: response.contact ? convertContactToCamelCase(response.contact) : null,
        message: response.message,
        server,
      });
    }
    
    // Get all contacts
    const response = await client.getAllContacts();
    const contacts = response.contacts.map(convertContactToCamelCase);
    
    return NextResponse.json({
      success: true,
      contacts,
      total_count: response.total_count,
      server,
    });
  } catch (error: any) {
    return NextResponse.json({
      success: false,
      error: error.message,
    }, { status: 500 });
  }
}

// POST - Create new contact
export async function POST(request: NextRequest) {
  try {
    const { name, address, phoneNumbers, server } = await request.json();
    const serverAddress = server === 'dotnet' ? 'localhost:5194' : 'localhost:5195';
    
    // Convert camelCase to snake_case for gRPC
    const contactRequest = {
      name,
      address: address ? {
        street: address.street,
        city: address.city,
        state: address.state,
        zip_code: address.zipCode,
        country: address.country,
      } : undefined,
      phone_numbers: phoneNumbers || [],
    };
    
    const client = new ContactClient(serverAddress);
    const response = await client.createContact(contactRequest);
    
    return NextResponse.json({
      success: response.success,
      contact: response.contact ? convertContactToCamelCase(response.contact) : null,
      message: response.message,
      server,
    });
  } catch (error: any) {
    return NextResponse.json({
      success: false,
      error: error.message,
    }, { status: 500 });
  }
}

// PUT - Update contact
export async function PUT(request: NextRequest) {
  try {
    const { id, name, address, phoneNumbers, server } = await request.json();
    const serverAddress = server === 'dotnet' ? 'localhost:5194' : 'localhost:5195';
    
    // Convert camelCase to snake_case for gRPC
    const contactRequest = {
      name,
      address: address ? {
        street: address.street,
        city: address.city,
        state: address.state,
        zip_code: address.zipCode,
        country: address.country,
      } : undefined,
      phone_numbers: phoneNumbers || [],
    };
    
    const client = new ContactClient(serverAddress);
    const response = await client.updateContact(id, contactRequest);
    
    return NextResponse.json({
      success: response.success,
      contact: response.contact ? convertContactToCamelCase(response.contact) : null,
      message: response.message,
      server,
    });
  } catch (error: any) {
    return NextResponse.json({
      success: false,
      error: error.message,
    }, { status: 500 });
  }
}

// DELETE - Delete contact
export async function DELETE(request: NextRequest) {
  try {
    const { id, server } = await request.json();
    const serverAddress = server === 'dotnet' ? 'localhost:5194' : 'localhost:5195';
    
    const client = new ContactClient(serverAddress);
    const response = await client.deleteContact(id);
    
    return NextResponse.json({
      success: response.success,
      message: response.message,
      server,
    });
  } catch (error: any) {
    return NextResponse.json({
      success: false,
      error: error.message,
    }, { status: 500 });
  }
}
