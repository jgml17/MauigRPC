import { Module } from '@nestjs/common';
import { GreeterController } from './services/greeter.controller';
import { ContactController } from './services/contact.controller';

/**
 * Main application module for the gRPC service.
 * 
 * This module:
 * - Registers all gRPC service controllers
 * - Configures the application dependencies
 * 
 * The actual gRPC server configuration (port, proto files, etc.)
 * is handled in main.ts using NestJS microservices.
 */
@Module({
  imports: [],
  controllers: [GreeterController, ContactController],
  providers: [],
})
export class AppModule {}
