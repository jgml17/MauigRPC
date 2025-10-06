using MauigRPC.ViewModels;

namespace MauigRPC;

/// <summary>
/// Main page of the MAUI application demonstrating gRPC communication with MVVM pattern.
/// </summary>
/// <remarks>
/// This page now follows the MVVM (Model-View-ViewModel) architectural pattern:
/// 
/// <strong>Architecture Overview:</strong>
/// - View (MainPage.xaml): Pure UI definition with data bindings
/// - ViewModel (MainViewModel): Business logic, state management, and gRPC communication
/// - Model: gRPC proto models (HelloRequest, HelloReply) used as DTOs
/// - Service: GrpcGreeterService handles low-level gRPC communication
/// 
/// <strong>Benefits of MVVM:</strong>
/// - Separation of Concerns: UI is separate from business logic
/// - Testability: ViewModel can be unit tested without UI
/// - Maintainability: Changes to UI or logic are isolated
/// - Reusability: ViewModel logic can be shared across platforms
/// - Data Binding: Automatic UI updates via INotifyPropertyChanged
/// 
/// <strong>Code-Behind Simplification:</strong>
/// The code-behind now only:
/// 1. Initializes the component
/// 2. Sets the BindingContext to the injected ViewModel
/// 
/// All business logic, state management, and gRPC calls are now in MainViewModel.
/// This follows best practices for MAUI MVVM applications.
/// 
/// <strong>Dependency Injection Flow:</strong>
/// <code>
/// MauiProgram.cs:
///   ├── Registers GrpcGreeterService (Singleton)
///   ├── Registers MainViewModel (Transient)
///   └── Registers MainPage (Transient)
/// 
/// MainPage Constructor:
///   ├── DI Container creates MainViewModel
///   │   └── Injects GrpcGreeterService
///   └── Sets ViewModel as BindingContext
/// 
/// View (XAML):
///   └── Binds to ViewModel properties/commands
/// </code>
/// 
/// <strong>Proto Model Integration:</strong>
/// Proto models are used as DTOs (Data Transfer Objects) only:
/// - HelloRequest: Created in GrpcGreeterService.SayHelloAsync()
/// - HelloReply: Received in GrpcGreeterService.SayHelloAsync()
/// - ViewModel: Uses simple properties (string, bool) for UI binding
/// 
/// This separation keeps proto models as pure data contracts while allowing
/// rich ViewModel properties with validation, computed values, and UI state.
/// </remarks>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the MainPage.
    /// </summary>
    /// <param name="viewModel">The MainViewModel injected by the DI container.</param>
    /// <remarks>
    /// The ViewModel is automatically resolved by .NET MAUI's dependency injection system.
    /// It is registered as Transient in MauiProgram.CreateMauiApp(), meaning a new instance
    /// is created each time the page is navigated to.
    /// 
    /// The ViewModel is set as the BindingContext, which enables all XAML bindings to
    /// resolve properties and commands from the ViewModel.
    /// 
    /// <strong>Why Transient for ViewModels?</strong>
    /// - Fresh state for each page navigation
    /// - Prevents state leakage between navigation cycles
    /// - Aligns with page lifecycle in MAUI
    /// 
    /// If you need shared state across navigations, consider:
    /// - Singleton ViewModel (not recommended for pages)
    /// - Shared service for state (recommended)
    /// - Navigation parameters to pass state
    /// </remarks>
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
