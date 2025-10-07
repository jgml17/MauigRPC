import { NestFactory } from '@nestjs/core';
import { MicroserviceOptions, Transport } from '@nestjs/microservices';
import { AppModule } from './app.module';
import { join } from 'path';

/**
 * Bootstrap function to start the gRPC microservice.
 * 
 * This configures the NestJS application as a gRPC microservice with:
 * - HTTP/2 protocol support
 * - Multiple proto file definitions (greet.proto, contact.proto)
 * - Server running on localhost:5195
 * - gRPC reflection enabled for development tools
 * 
 * The configuration is similar to the .NET version but uses port 5195
 * to avoid conflicts with the .NET service on 5194.
 */
async function bootstrap() {
  const app = await NestFactory.createMicroservice<MicroserviceOptions>(
    AppModule,
    {
      transport: Transport.GRPC,
      options: {
        // gRPC server configuration
        url: '0.0.0.0:5195',
        
        // Protocol buffer package definitions
        package: ['greet', 'contact'],
        
        // Proto file paths
        protoPath: [
          join(__dirname, '../src/protos/greet.proto'),
          join(__dirname, '../src/protos/contact.proto'),
        ],
        
        // Load proto files with options
        loader: {
          keepCase: true,           // Keep field names as defined in proto
          longs: String,            // Convert long values to strings
          enums: String,            // Keep enum values as strings
          defaults: true,           // Set default values for missing fields
          oneofs: true,             // Support oneof fields
        },
        
        // Enable gRPC reflection for development
        // This allows tools like grpcurl, Postman, and gRPC UI to discover services
        // In production, you might want to disable this for security
      },
    },
  );

  // Start the microservice
  await app.listen();
  
  console.log('');
  console.log('🚀 gRPC Server is running on http://localhost:5195');
  console.log('');
  console.log('Available Services:');
  console.log('  - greet.Greeter');
  console.log('  - contact.ContactService');
  console.log('');
  console.log('Test with grpcurl:');
  console.log('  grpcurl -plaintext -d \'{"name": "World"}\' localhost:5195 greet.Greeter/SayHello');
  console.log('  grpcurl -plaintext -d \'{}\' localhost:5195 contact.ContactService/GetAllContacts');
  console.log('');
}

bootstrap();
