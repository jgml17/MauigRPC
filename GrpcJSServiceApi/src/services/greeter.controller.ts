import { Controller } from '@nestjs/common';
import { GrpcMethod } from '@nestjs/microservices';

/**
 * Interface for HelloRequest message
 */
interface HelloRequest {
  name: string;
}

/**
 * Interface for HelloReply message
 */
interface HelloReply {
  message: string;
}

/**
 * Implementation of the Greeter gRPC service.
 * Provides greeting functionality through gRPC calls using Protocol Buffers (protobuf) serialization.
 * 
 * This service is automatically registered in the NestJS microservices pipeline.
 * It uses decorators to map gRPC methods to controller methods.
 * All gRPC services use HTTP/2 protocol for efficient binary communication.
 */
@Controller()
export class GreeterController {
  /**
   * Handles the SayHello RPC call by creating a personalized greeting message.
   * 
   * @param request - The HelloRequest message containing the name to greet.
   * @returns A HelloReply message with the greeting.
   * 
   * @remarks
   * This method is called automatically when a client invokes the SayHello RPC.
   * The request/response are serialized using Protocol Buffers for efficient binary transmission.
   * 
   * Example gRPC call:
   * ```
   * const request = { name: 'World' };
   * const reply = await client.sayHello(request);
   * // reply.message = "Hello World"
   * ```
   * 
   * Performance: Protobuf serialization is ~5x faster than JSON and produces ~60% smaller payloads.
   */
  @GrpcMethod('Greeter', 'SayHello')
  sayHello(request: HelloRequest): HelloReply {
    return {
      message: `Hello ${request.name}`,
    };
  }
}
