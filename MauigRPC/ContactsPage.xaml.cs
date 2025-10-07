using MauigRPC.ViewModels;

namespace MauigRPC;

public partial class ContactsPage : ContentPage
{
    public ContactsPage(ContactsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
