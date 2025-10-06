using System.Diagnostics;
using MauigRPC.Services;

namespace MauigRPC;

/// <summary>
/// Main page of the MAUI application demonstrating gRPC communication.
/// Provides a user interface for testing the gRPC Greeter service.
/// </summary>
/// <remarks>
/// This page demonstrates:
/// - Integration of gRPC client in a MAUI application
/// - Dependency injection of gRPC services
/// - Async/await patterns for network calls
/// - User feedback during asynchronous operations (loading indicators)
/// - Error handling and user-friendly error messages
/// - Accessibility features (semantic screen reader announcements)
/// 
/// The page receives the GrpcGreeterService through constructor injection,
/// which is configured as a Singleton in MauiProgram.cs to reuse the gRPC channel.
/// </remarks>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// The gRPC service used to communicate with the remote Greeter service.
    /// Injected as a singleton to reuse the HTTP/2 connection.
    /// </summary>
    private readonly GrpcGreeterService _grpcService;

    /// <summary>
    /// Initializes a new instance of the MainPage.
    /// </summary>
    /// <param name="grpcService">The gRPC greeter service injected by the DI container.</param>
    /// <remarks>
    /// The grpcService parameter is automatically resolved by .NET MAUI's dependency injection.
    /// The service is registered as a Singleton in MauiProgram.CreateMauiApp().
    /// </remarks>
    public MainPage(GrpcGreeterService grpcService)
    {
        InitializeComponent();
        _grpcService = grpcService;
    }

    /// <summary>
    /// Event handler for the "Call gRPC Service" button click.
    /// Demonstrates the complete flow of calling a gRPC service from a MAUI application.
    /// </summary>
    /// <param name="sender">The button that triggered the event.</param>
    /// <param name="e">Event arguments.</param>
    /// <remarks>
    /// This method demonstrates best practices for calling remote services in a mobile app:
    /// 
    /// <strong>1. User Feedback:</strong>
    /// - Shows a loading indicator immediately when the button is clicked
    /// - Disables the button to prevent duplicate calls
    /// - Displays status message ("Calling gRPC service...")
    /// 
    /// <strong>2. Network Communication:</strong>
    /// - Retrieves user input from the text entry field
    /// - Uses a default name ("MAUI Client") if input is empty
    /// - Calls the gRPC service asynchronously using await
    /// - The call uses binary Protocol Buffers over HTTP/2 for efficiency
    /// 
    /// <strong>3. Response Handling:</strong>
    /// - Updates the UI label with the server's greeting response
    /// - Logs the response to Debug output for development/debugging
    /// - Announces the response to screen readers for accessibility
    /// 
    /// <strong>4. Error Handling:</strong>
    /// - Catches all exceptions from the gRPC call
    /// - Displays error in the response label
    /// - Shows a user-friendly alert dialog
    /// - Logs errors to Debug output for troubleshooting
    /// 
    /// <strong>5. Cleanup:</strong>
    /// - Always hides the loading indicator (in finally block)
    /// - Re-enables the button for subsequent calls
    /// - Ensures UI is responsive even if an error occurs
    /// 
    /// <strong>Performance Characteristics:</strong>
    /// - Binary protobuf is ~60% smaller than equivalent JSON
    /// - HTTP/2 multiplexing allows concurrent calls on same connection
    /// - Keep-alive connections reduce latency by ~40%
    /// 
    /// <strong>Common Error Scenarios:</strong>
    /// - "Connection refused": Server not running or firewall blocking
    /// - "Status(StatusCode=\"Internal\", Detail=\"Bad gRPC response...\")" : HTTP/2 misconfiguration
    /// - "Deadline exceeded": Server took too long to respond
    /// - "Unavailable": Network connectivity issues
    /// 
    /// For troubleshooting, check:
    /// - Server is running on correct port (5194 for development)
    /// - Correct address for platform (10.0.2.2 for Android, localhost for iOS)
    /// - Firewall allows connections on port 5194
    /// - Server configured for HTTP/2 protocol
    /// </remarks>
    /// <example>
    /// User flow:
    /// <code>
    /// 1. User enters "World" in the text entry
    /// 2. User clicks "Call gRPC Service" button
    /// 3. Button disabled, loading indicator shows
    /// 4. gRPC call: HelloRequest { Name = "World" } → Server
    /// 5. Server responds: HelloReply { Message = "Hello World" }
    /// 6. UI displays: "Hello World"
    /// 7. Loading indicator hides, button re-enabled
    /// </code>
    /// </example>
    private async void OnCallGrpcClicked(object? sender, EventArgs e)
    {
        try
        {
            // Show loading indicator
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            CallGrpcBtn.IsEnabled = false;
            ResponseLabel.Text = "Calling gRPC service...";

            // Get the name from the entry
            string name = string.IsNullOrWhiteSpace(NameEntry.Text) 
                ? "MAUI Client" 
                : NameEntry.Text;

            // Call the gRPC service
            string response = await _grpcService.SayHelloAsync(name);

            // Update the UI with the response
            Debug.WriteLine($"MESSAGE: {response}");
            ResponseLabel.Text = response;
            SemanticScreenReader.Announce(response);
        }
        catch (Exception ex)
        {
            ResponseLabel.Text = $"Error: {ex.Message}";
            Debug.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            // Hide loading indicator
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            CallGrpcBtn.IsEnabled = true;
        }
    }
}
