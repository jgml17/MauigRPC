/**
 * API Route for Greeter Service
 * 
 * In Next.js, gRPC clients run on the SERVER, not in the browser.
 * This is similar to how your MAUI app calls gRPC services.
 * 
 * C# MAUI:           Browser makes HTTP request →  Next.js API Route
 * var client = new   (fetch('/api/greet'))        gRPC Client calls → gRPC Server
 * GreeterClient()                                  
 * await client.SayHelloAsync()
 */

import { NextRequest, NextResponse } from 'next/server';
import { GreeterClient } from '@/lib/grpc-clients';

export async function POST(request: NextRequest) {
  try {
    const { name, server } = await request.json();
    
    // Choose which server to call (similar to changing channel address in C#)
    const serverAddress = server === 'dotnet' ? 'localhost:5194' : 'localhost:5195';
    
    // Create client (like: var client = new Greeter.GreeterClient(channel))
    const client = new GreeterClient(serverAddress);
    
    // Call gRPC method (like: await client.SayHelloAsync(new HelloRequest { Name = name }))
    const response = await client.sayHello(name);
    
    return NextResponse.json({
      success: true,
      message: response.message,
      server: server,
    });
  } catch (error: any) {
    return NextResponse.json({
      success: false,
      error: error.message,
    }, { status: 500 });
  }
}
