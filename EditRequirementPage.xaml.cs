using Capstone.Models;
using Capstone.Services;
using Microsoft.Maui.Controls;

namespace Capstone;

public partial class EditRequirementPage : ContentPage
{
    public static List<string> typeOptions = new List<string>()
    {
        "Functional",
        "Non-functional",
        "User",
        "System",
        "Design",
        "Business",
        "Other"
    };

    public EditRequirementPage()
	{
		InitializeComponent();
	}
   
    protected override void OnAppearing()
    {
        // reset button text
        SaveRequirementButton.Text = "Save";
        CancelRequirementButton.Text = "Cancel";

        // populate xaml type picker with type options
        RequirementTypePicker.ItemsSource = typeOptions;

        PopulateFields();
    }

    public void PopulateFields()
    {
        RequirementNameEntry.Text = ViewRequirementPage.selectedRequirement.Name;       
        RequirementTypePicker.SelectedItem = ViewRequirementPage.selectedRequirement.Type;       
        RequirementDescriptionEntry.Text = ViewRequirementPage.selectedRequirement.Description;
    }
 
    private async void SaveRequirementButton_Clicked(object sender, EventArgs e)
    {
        // validate save
        if (string.IsNullOrWhiteSpace(RequirementNameEntry.Text))
        {
            // Requirement Name Entry = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (RequirementTypePicker.SelectedItem is null)
        {
            // Requirement Type = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(RequirementDescriptionEntry.Text))
        {
            // format empty description
            RequirementDescriptionEntry.Text = " ";
        }

        // loading indicator
        SaveRequirementButton.Text = "Loading...";

        // update requirement
        DataAccessService.UpdateRequirement(ViewRequirementPage.selectedRequirement.Id, RequirementNameEntry.Text, RequirementTypePicker.SelectedItem.ToString(), RequirementDescriptionEntry.Text, ViewRequirementPage.selectedRequirement.SprintId, ViewRequirementPage.selectedRequirement.UserId);

        // reset search results
        SearchPage.instance?.ResetPage();

        // return to view requirement page
        await Shell.Current.GoToAsync("..");
    }
 
    private async void CancelRequirementButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        CancelRequirementButton.Text = "Loading...";

        // return to view requirement page
        await Shell.Current.GoToAsync("..");
    }
}