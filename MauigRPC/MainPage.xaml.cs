using System.Diagnostics;
using MauigRPC.Services;

namespace MauigRPC;

public partial class MainPage : ContentPage
{
    private readonly GrpcGreeterService _grpcService;

    public MainPage(GrpcGreeterService grpcService)
    {
        InitializeComponent();
        _grpcService = grpcService;
    }

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
