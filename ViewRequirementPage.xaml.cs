using Capstone.Models;
using Capstone.Services;

namespace Capstone;

[QueryProperty("ReceivedId", "id")]

public partial class ViewRequirementPage : ContentPage
{    
    public static Requirement selectedRequirement;

    public static string ReceivedId
    {
        set
        {
            // retrieve selected Requirement
            Guid requirementId = new Guid(value);            
            selectedRequirement = DataAccessService.GetRequirement(requirementId);
        }
    }

    public ViewRequirementPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        // reset button text
        EditRequirementButton.Text = "Edit Requirement";
        DeleteRequirementButton.Text = "Delete Requirement";

        LoadDetails(selectedRequirement.Id);
    }

    // rename Requirement to requirement in xaml
    public void LoadDetails(Guid sprintId)
    {              
        // set requirement details in xaml        
        RequirementName.Text = selectedRequirement.Name;      
        RequirementTypeLabel.Text = selectedRequirement.Type;       
        RequirementDescriptionLabel.Text = selectedRequirement.Description;
    }

    // rename Requirement to requirement in xaml
    private async void EditRequirementButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        EditRequirementButton.Text = "Loading...";

        // go to edit requirement page
        await Shell.Current.GoToAsync(nameof(EditRequirementPage));
    }

    // rename Requirement to requirement in xaml
    private async void DeleteRequirementButton_Clicked(object sender, EventArgs e)
    {      
        // confirm delete
        bool result = await DisplayAlert("Confirm", "Are you sure you want to delete this requirement?", "YES", "NO");
        if (result == false)
        {
            return;
        }
        else
        {
            // loading indicator
            DeleteRequirementButton.Text = "Loading...";

            // delete requirement
            DataAccessService.DeleteRequirement(selectedRequirement.Id);

            // reset search results
            SearchPage.instance?.ResetPage();

            // return to sprint page
            await Shell.Current.GoToAsync("..");
        }
    }
}