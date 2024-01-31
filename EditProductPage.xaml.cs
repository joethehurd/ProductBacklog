using Capstone.Models;
using Capstone.Services;
using Microsoft.Maui.Controls;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Capstone;

public partial class EditProductPage : ContentPage
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

    public EditProductPage()
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

        PopulateFields();
    }
       
    public void PopulateFields()
    {
        // populate fields with selected product's details
        ProductNameEntry.Text = ViewProductPage.selectedProduct.Name;
        ProductStartDatePicker.Date = ViewProductPage.selectedProduct.Start.ToLocalTime();
        ProductEndDatePicker.Date = ViewProductPage.selectedProduct.End.ToLocalTime();
        ProductOwnerNameEntry.Text = ViewProductPage.selectedProduct.OwnerName;
        ProductOwnerPhoneEntry.Text = ViewProductPage.selectedProduct.OwnerPhone;
        ProductOwnerEmailEntry.Text = ViewProductPage.selectedProduct.OwnerEmail;
        switch (ViewProductPage.selectedProduct.Status)
        {
            case 1:
                ProductStatusPicker.SelectedItem = "Not started";
                break;
            case 2:
                ProductStatusPicker.SelectedItem = "In progress";
                break;
            case 3:
                ProductStatusPicker.SelectedItem = "Completed";
                break;
        }
        switch (ViewProductPage.selectedProduct.Priority)
        {
            case 1:
                ProductPriorityPicker.SelectedItem = "High";
                break;
            case 2:
                ProductPriorityPicker.SelectedItem = "Medium";
                break;
            case 3:
                ProductPriorityPicker.SelectedItem = "Low";
                break;
        }
        ProductAlertsPicker.SelectedItem = ViewProductPage.selectedProduct.Alerts;
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
            // Product Alerts = Empty
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

        //sync priority
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
      
        // ensure existing sprint dates are within product dates
        foreach (Sprint sprint in ViewProductPage.sprintList)
        {
            if (sprint.Start.ToLocalTime() < ProductStartDatePicker.Date || sprint.Start.ToLocalTime() >= ProductEndDatePicker.Date)
            {
                if (sprint.End.ToLocalTime() > ProductEndDatePicker.Date || sprint.End.ToLocalTime() <= ProductStartDatePicker.Date)
                {
                    // both Sprint Dates are outside of Product Dates
                    // update both Sprint Dates to Product Dates                    
                    DataAccessService.UpdateSprint(sprint.Id, sprint.Name, ProductStartDatePicker.Date, ProductEndDatePicker.Date, sprint.AssigneeName, sprint.AssigneePhone, sprint.AssigneeEmail, sprint.Status, sprint.Priority, sprint.Alerts, sprint.ProductId, sprint.UserId);
                }
                else
                {
                    // Sprint Start Date is outside of Product Dates
                    // set Sprint Start Date to Product Start Date                    
                    DataAccessService.UpdateSprint(sprint.Id, sprint.Name, ProductStartDatePicker.Date, sprint.End, sprint.AssigneeName, sprint.AssigneePhone, sprint.AssigneeEmail, sprint.Status, sprint.Priority, sprint.Alerts, sprint.ProductId, sprint.UserId);
                }
              
            }          
            else if (sprint.End.ToLocalTime() > ProductEndDatePicker.Date || sprint.End.ToLocalTime() <= ProductStartDatePicker.Date)
            {
                // Sprint End Date is outside of Product Dates
                // update Sprint End Date to Product End Date                
                DataAccessService.UpdateSprint(sprint.Id, sprint.Name, sprint.Start, ProductEndDatePicker.Date, sprint.AssigneeName, sprint.AssigneePhone, sprint.AssigneeEmail, sprint.Status, sprint.Priority, sprint.Alerts, sprint.ProductId, sprint.UserId);
            }            
        }

        // loading indicator
        SaveProductButton.Text = "Loading...";

        // update product
        DataAccessService.UpdateProduct(ViewProductPage.selectedProduct.Id, ProductNameEntry.Text, ProductStartDatePicker.Date, ProductEndDatePicker.Date, ProductOwnerNameEntry.Text, ProductOwnerPhoneEntry.Text, ProductOwnerEmailEntry.Text, status, priority, ProductAlertsPicker.SelectedItem.ToString(), ViewProductPage.selectedProduct.UserId);

        // reset search results
        SearchPage.instance?.ResetPage();

        // return to view product page
        await Shell.Current.GoToAsync("..");
    }
 
    private async void CancelProductButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        CancelProductButton.Text = "Loading...";

        // return to view product page
        await Shell.Current.GoToAsync("..");
    }
}