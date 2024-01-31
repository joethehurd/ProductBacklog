using Capstone.Models;
using Capstone.Services;
using Microsoft.Maui.Controls;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Capstone;

public partial class AddProductPage : ContentPage
{
    public static List<string> statusOptions = new List<string>()
    {
        "Not started",
        "In progress",
        "Completed"
    };

    public static List<string> priorityOptions = new List<string>()
    {
        "High",
        "Medium",
        "Low"
    };

    public static List<string> alertOptions = new List<string>()
    {
        "On",
        "Off"
    };

    public AddProductPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        // reset button text
        SaveProductButton.Text = "Save";
        CancelProductButton.Text = "Cancel";

        // populate xaml status picker with status options
        ProductStatusPicker.ItemsSource = statusOptions;

        // populate xaml priority picker with priority options
        ProductPriorityPicker.ItemsSource = priorityOptions;

        // populate xaml alerts picker with alert options
        ProductAlertsPicker.ItemsSource = alertOptions;        
    }

    private async void SaveProductButton_Clicked(object sender, EventArgs e)
    {
        // validation
        if (string.IsNullOrWhiteSpace(ProductNameEntry.Text))
        {
            // Product Name Entry = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(ProductStartDatePicker.Date.ToString()))
        {
            // Product Start Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(ProductEndDatePicker.Date.ToString()))
        {
            // Product End Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(ProductOwnerNameEntry.Text))
        {
            // Product Owner Name = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(ProductOwnerPhoneEntry.Text))
        {
            // Product Owner Phone = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (!new PhoneAttribute().IsValid(ProductOwnerPhoneEntry.Text))
        {
            // Product Owner Phone = Invalid
            await DisplayAlert("Alert", "Invalid phone number.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(ProductOwnerEmailEntry.Text))
        {
            // Product Owner Email = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (!new EmailAddressAttribute().IsValid(ProductOwnerEmailEntry.Text))
        {
            // Product Owner Email = Invalild
            await DisplayAlert("Alert", "Invalid email address.", "OK");
            return;
        }
        if (ProductStatusPicker.SelectedItem is null)
        {
            // Product Status = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (ProductPriorityPicker.SelectedItem is null)
        {
            // Product Priority = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (ProductAlertsPicker.SelectedItem is null)
        {
            //Product Alerts = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (ProductStartDatePicker.Date >= ProductEndDatePicker.Date)
        {
            // Product Start Date is after or on End Date
            await DisplayAlert("Alert", "Start Date must be before End Date.", "OK");
            return;
        }

        // format text
        var tempName = ProductNameEntry.Text.ToLower();
        ProductNameEntry.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempName);

        var tempOwnerName = ProductOwnerNameEntry.Text.ToLower();
        ProductOwnerNameEntry.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempOwnerName);

        // sync status
        var status = 2;
        if (ProductStatusPicker.SelectedItem.ToString() == "Not started")
        {
            status = 1;
        }
        else if (ProductStatusPicker.SelectedItem.ToString() == "In progress")
        {
            status = 2;
        }
        else if (ProductStatusPicker.SelectedItem.ToString() == "Completed")
        {
            status = 3;
        }

        // sync priority
        var priority = 2;
        if (ProductPriorityPicker.SelectedItem.ToString() == "High")
        {
            priority = 1;
        }
        else if (ProductPriorityPicker.SelectedItem.ToString() == "Medium")
        {
            priority = 2;
        }
        else if (ProductPriorityPicker.SelectedItem.ToString() == "Low")
        {
            priority = 3;
        }

        // loading indicator
        SaveProductButton.Text = "Loading...";

        // add Product
        DataAccessService.AddProduct(new Guid(), ProductNameEntry.Text, ProductStartDatePicker.Date, ProductEndDatePicker.Date, ProductOwnerNameEntry.Text, ProductOwnerPhoneEntry.Text, ProductOwnerEmailEntry.Text, status, priority, ProductAlertsPicker.SelectedItem.ToString(), MainPage.activeUser.Id);

        // reset search results
        SearchPage.instance?.ResetPage();

        // return to home page
        await Shell.Current.GoToAsync("..");
    }

    private async void CancelProductButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        CancelProductButton.Text = "Loading...";

        // return to home page
        await Shell.Current.GoToAsync("..");
    }  
}