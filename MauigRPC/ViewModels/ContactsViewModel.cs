using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GrpcServiceApi;
using MauigRPC.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauigRPC.ViewModels;

/// <summary>
/// ViewModel for managing contacts using gRPC ContactService.
/// Demonstrates CRUD operations with proto models in MVVM pattern.
/// </summary>
public partial class ContactsViewModel(GrpcContactService contactService) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<GrpcServiceApi.Contact> _contacts = new();

    [ObservableProperty]
    private GrpcServiceApi.Contact? _selectedContact;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private int _totalCount;

    /// <summary>
    /// Loads all contacts from the server.
    /// </summary>
    [RelayCommand]
    private async Task LoadContactsAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Loading contacts...";

            var response = await contactService.GetAllContactsAsync();
            
            Contacts.Clear();
            foreach (var contact in response.Contacts)
            {
                Contacts.Add(contact);
            }

            TotalCount = response.TotalCount;
            StatusMessage = $"Loaded {TotalCount} contact(s)";
            
            Debug.WriteLine($"Loaded {TotalCount} contacts");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            Debug.WriteLine($"Error loading contacts: {ex.Message}");
            await ShowAlertAsync("Error", $"Failed to load contacts: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Refreshes the contacts list (for pull-to-refresh).
    /// </summary>
    [RelayCommand]
    private async Task RefreshContactsAsync()
    {
        IsRefreshing = true;
        await LoadContactsAsync();
    }

    /// <summary>
    /// Creates a new contact with sample data.
    /// In a real app, this would show a form to collect data.
    /// </summary>
    [RelayCommand]
    private async Task AddContactAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Creating contact...";

            // Create sample address
            var address = new Address
            {
                Street = "123 New Street",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94102",
                Country = "USA"
            };

            // Create sample phone numbers
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Number = "+1-555-0000", Type = PhoneType.Mobile }
            };

            var response = await contactService.CreateContactAsync(
                $"New Contact {DateTime.Now:HHmmss}",
                address,
                phoneNumbers
            );

            if (response.Success)
            {
                Contacts.Add(response.Contact);
                TotalCount++;
                StatusMessage = response.Message;
                await ShowAlertAsync("Success", response.Message);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            await ShowAlertAsync("Error", $"Failed to create contact: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Deletes the selected contact.
    /// </summary>
    [RelayCommand]
    private async Task DeleteContactAsync(GrpcServiceApi.Contact? contact)
    {
        if (contact == null) return;

        try
        {
            bool confirm = await Application.Current!.MainPage!.DisplayAlert(
                "Delete Contact",
                $"Are you sure you want to delete '{contact.Name}'?",
                "Yes",
                "No"
            );

            if (!confirm) return;

            IsLoading = true;
            StatusMessage = $"Deleting {contact.Name}...";

            var response = await contactService.DeleteContactAsync(contact.Id);

            if (response.Success)
            {
                Contacts.Remove(contact);
                TotalCount--;
                StatusMessage = response.Message;
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            await ShowAlertAsync("Error", $"Failed to delete contact: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Shows details of the selected contact.
    /// </summary>
    [RelayCommand]
    private async Task ViewContactDetailsAsync(GrpcServiceApi.Contact? contact)
    {
        if (contact == null) return;

        var details = $"Name: {contact.Name}\n\n" +
                     $"Address:\n" +
                     $"  {contact.Address?.Street}\n" +
                     $"  {contact.Address?.City}, {contact.Address?.State} {contact.Address?.ZipCode}\n" +
                     $"  {contact.Address?.Country}\n\n" +
                     $"Phone Numbers:\n";

        foreach (var phone in contact.PhoneNumbers)
        {
            details += $"  {phone.Type}: {phone.Number}\n";
        }

        await ShowAlertAsync($"Contact Details (ID: {contact.Id})", details);
    }

    /// <summary>
    /// Helper method to show alerts.
    /// </summary>
    private async Task ShowAlertAsync(string title, string message)
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
