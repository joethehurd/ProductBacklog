using Capstone.Models;
using Capstone.Services;
using Microsoft.Maui.Controls;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Capstone;

public partial class EditSprintPage : ContentPage
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

    public EditSprintPage()
	{
		InitializeComponent();
	}
  
    protected override void OnAppearing()
    {
        // reset button text
        SaveSprintButton.Text = "Save";
        CancelSprintButton.Text = "Cancel";

        // populate xaml status picker with status options
        SprintStatusPicker.ItemsSource = statusOptions;

        // populate xaml priority picker with priority options
        SprintPriorityPicker.ItemsSource = priorityOptions;

        // populate xaml alerts picker with alert options
        SprintAlertsPicker.ItemsSource = alertOptions;

        PopulateFields();
    }   
  
    public void PopulateFields()
    {
        // populate fields with selected Sprint's details
        SprintNameEntry.Text = ViewSprintPage.selectedSprint.Name;
        SprintStartDatePicker.Date = ViewSprintPage.selectedSprint.Start.ToLocalTime();
        SprintEndDatePicker.Date = ViewSprintPage.selectedSprint.End.ToLocalTime();
        SprintAssigneeNameEntry.Text = ViewSprintPage.selectedSprint.AssigneeName;
        SprintAssigneePhoneEntry.Text = ViewSprintPage.selectedSprint.AssigneePhone;
        SprintAssigneeEmailEntry.Text = ViewSprintPage.selectedSprint.AssigneeEmail;     
        switch (ViewSprintPage.selectedSprint.Status)
        {
            case 1:
                SprintStatusPicker.SelectedItem = "Not started";
                break;
            case 2:
                SprintStatusPicker.SelectedItem = "In progress";
                break;
            case 3:
                SprintStatusPicker.SelectedItem = "Completed";
                break;
        }
        switch (ViewSprintPage.selectedSprint.Priority)
        {
            case 1:
                SprintPriorityPicker.SelectedItem = "High";
                break;
            case 2:
                SprintPriorityPicker.SelectedItem = "Medium";
                break;
            case 3:
                SprintPriorityPicker.SelectedItem = "Low";
                break;
        }
        SprintAlertsPicker.SelectedItem = ViewSprintPage.selectedSprint.Alerts;       
    }
   
    private async void SaveSprintButton_Clicked(object sender, EventArgs e)
    {
        // validation
        if (string.IsNullOrWhiteSpace(SprintNameEntry.Text))
        {
            // Sprint Name Entry = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SprintStartDatePicker.Date.ToString()))
        {
            // Sprint Start Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SprintEndDatePicker.Date.ToString()))
        {
            // Sprint End Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SprintAssigneeNameEntry.Text))
        {
            // Sprint Assignee Name = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SprintAssigneePhoneEntry.Text))
        {
            // Sprint Assignee Phone = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (!new PhoneAttribute().IsValid(SprintAssigneePhoneEntry.Text))
        {
            // Sprint Assignee Phone = Invalid
            await DisplayAlert("Alert", "Invalid phone number.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SprintAssigneeEmailEntry.Text))
        {
            // Sprint Assignee Email = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (!new EmailAddressAttribute().IsValid(SprintAssigneeEmailEntry.Text))
        {
            // Sprint Assignee Email = Invalild
            await DisplayAlert("Alert", "Invalid email address.", "OK");
            return;
        }
        if (SprintStatusPicker.SelectedItem is null)
        {
            // Sprint Status = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (SprintPriorityPicker.SelectedItem is null)
        {
            // Sprint Priority = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (SprintAlertsPicker.SelectedItem is null)
        {
            // Sprint Alerts = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (SprintStartDatePicker.Date >= SprintEndDatePicker.Date)
        {
            // Sprint Start Date is after or on End Date
            await DisplayAlert("Alert", "Start Date must be before End Date.", "OK");
            return;
        }
        if (SprintStartDatePicker.Date >= ViewProductPage.selectedProduct.End.ToLocalTime())
        {
            // Sprint Start Date is after or on Product End Date
            await DisplayAlert("Alert", $"Start Date must be within Product Dates: {ViewProductPage.selectedProduct.Start.ToLocalTime().ToString("MMMM d, yyyy")} to {ViewProductPage.selectedProduct.End.ToLocalTime().ToString("MMMM d, yyyy")}.", "OK");
            return;
        }
        if (SprintStartDatePicker.Date < ViewProductPage.selectedProduct.Start.ToLocalTime())
        {
            // Sprint Start Date is before Product Start Date
            await DisplayAlert("Alert", $"Start Date must be within Product Dates: {ViewProductPage.selectedProduct.Start.ToLocalTime().ToString("MMMM d, yyyy")} to {ViewProductPage.selectedProduct.End.ToLocalTime().ToString("MMMM d, yyyy")}.", "OK");
            return;
        }
        if (SprintEndDatePicker.Date > ViewProductPage.selectedProduct.End.ToLocalTime())
        {
            // Sprint End Date is after Product End Date
            await DisplayAlert("Alert", $"End Date must be within Product Dates: {ViewProductPage.selectedProduct.Start.ToLocalTime().ToString("MMMM d, yyyy")} to {ViewProductPage.selectedProduct.End.ToLocalTime().ToString("MMMM d, yyyy")}.", "OK");
            return;
        }
        if (SprintEndDatePicker.Date <= ViewProductPage.selectedProduct.Start.ToLocalTime())
        {
            // Sprint End Date is before or on Product Start Date
            await DisplayAlert("Alert", $"End Date must be within Product Dates: {ViewProductPage.selectedProduct.Start.ToLocalTime().ToString("MMMM d, yyyy")} to {ViewProductPage.selectedProduct.End.ToLocalTime().ToString("MMMM d, yyyy")}.", "OK");
            return;
        }

        // format text
        var tempName = SprintNameEntry.Text.ToLower();
        SprintNameEntry.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempName);

        var tempAssigneeName = SprintAssigneeNameEntry.Text.ToLower();
        SprintAssigneeNameEntry.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempAssigneeName);

        // sync status
        var status = 2;       
        if (SprintStatusPicker.SelectedItem.ToString() == "Not started")
        {
            status = 1;
        }
        else if (SprintStatusPicker.SelectedItem.ToString() == "In progress")
        {
            status = 2;
        }
        else if (SprintStatusPicker.SelectedItem.ToString() == "Completed")
        {
            status = 3;
        }     

        // sync priority
        var priority = 2;
        if (SprintPriorityPicker.SelectedItem.ToString() == "High")
        {
            priority = 1;
        }
        else if (SprintPriorityPicker.SelectedItem.ToString() == "Medium")
        {
            priority = 2;
        }
        else if (SprintPriorityPicker.SelectedItem.ToString() == "Low")
        {
            priority = 3;
        }

        // loading indicator
        SaveSprintButton.Text = "Loading...";

        // update sprint
        DataAccessService.UpdateSprint(ViewSprintPage.selectedSprint.Id, SprintNameEntry.Text, SprintStartDatePicker.Date, SprintEndDatePicker.Date, SprintAssigneeNameEntry.Text, SprintAssigneePhoneEntry.Text, SprintAssigneeEmailEntry.Text, status, priority, SprintAlertsPicker.SelectedItem.ToString(), ViewSprintPage.selectedSprint.ProductId, ViewSprintPage.selectedSprint.UserId);

        // reset search results
        SearchPage.instance?.ResetPage();

        // return to view sprint page
        await Shell.Current.GoToAsync("..");
    }

    private async void CancelSprintButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        CancelSprintButton.Text = "Loading...";

        // return to view sprint page
        await Shell.Current.GoToAsync("..");
    }
}