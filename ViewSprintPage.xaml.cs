using Capstone.Models;
using Capstone.Services;

namespace Capstone;

[QueryProperty("ReceivedId", "id")]

public partial class ViewSprintPage : ContentPage
{
    public static IList<Requirement> requirementList;
    public static Sprint selectedSprint;

    public static string ReceivedId
    {
        set
        {
            // retrieve selected Sprint
            Guid sprintId = new Guid(value);            
            selectedSprint = DataAccessService.GetSprint(sprintId);
        }
    }

    public ViewSprintPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        // reset button text
        AddRequirementButton.Text = "Add Requirement";
        EditSprintButton.Text = "Edit Task";
        DeleteSprintButton.Text = "Delete Task";

        LoadDetails(selectedSprint.Id);
    }
   
    public void LoadDetails(Guid sprintId)
    {
        // format sprint dates
        var startDate = selectedSprint.Start.ToLocalTime().ToString("MMMM d, yyyy");
        var endDate = selectedSprint.End.ToLocalTime().ToString("MMMM d, yyyy");

        // set sprint details in xaml
        SprintName.Text = selectedSprint.Name;
        SprintDates.Text = $"{startDate} to {endDate}";
        SprintAssigneeNameLabel.Text = selectedSprint.AssigneeName;
        SprintAssigneePhoneLabel.Text = selectedSprint.AssigneePhone;
        SprintAssigneeEmailLabel.Text = selectedSprint.AssigneeEmail;
        switch (selectedSprint.Status)
        {
            case 1:
                SprintStatusLabel.Text = "Not started";
                break;
            case 2:
                SprintStatusLabel.Text = "In progress";
                break;
            case 3:
                SprintStatusLabel.Text = "Completed";
                break;
        }
        switch (selectedSprint.Priority)
        {
            case 1:
                SprintPriorityLabel.Text = "High";
                break;
            case 2:
                SprintPriorityLabel.Text = "Medium";
                break;
            case 3:
                SprintPriorityLabel.Text = "Low";
                break;
        }       
        SprintAlertsLabel.Text = selectedSprint.Alerts;
      
        // refresh requirement list
        RefreshRequirementList(sprintId);       
    }  
   
    public void RefreshRequirementList(Guid sprintId)
    {
        // populate requirement list
        requirementList = DataAccessService.GetRequirementsBySprint(sprintId);

        // populate xaml collection view with list items
        RequirementCollectionView.ItemsSource = requirementList;

        // determine if label is needed
        if (!requirementList.Any())
        {
            SprintRequirementsLabel.Text = String.Empty;
        }
        else
        {
            SprintRequirementsLabel.Text = "Requirements";
        }
    }
      
    private async void ViewRequirementButton_Clicked(object sender, EventArgs e)
    {
        // gets the id of the selected requirement       
        var tempId = (Button)sender;

        // loading indicator
        tempId.Text = "Loading...";

        // passes the selected Requirement's ID to the View Requirement Page
        await Shell.Current.GoToAsync($"{nameof(ViewRequirementPage)}?id={tempId.CommandParameter}");
    }
      
    private async void AddRequirementButton_Clicked(object sender, EventArgs e)
    {
        // check requirement limit      
        if (requirementList.Count == 12)
        {            
            await DisplayAlert("Alert", "Requirement limit [12] reached.", "OK");
            return;
        }
        else
        {
            // loading indicator
            AddRequirementButton.Text = "Loading...";

            // go to add requirement page
            await Shell.Current.GoToAsync(nameof(AddRequirementPage));
        }      
    }
       
    private async void EditSprintButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        EditSprintButton.Text = "Loading...";

        // go to edit sprint page
        await Shell.Current.GoToAsync(nameof(EditSprintPage));
    }
       
    private async void DeleteSprintButton_Clicked(object sender, EventArgs e)
    {
        // confirm delete
        bool result = await DisplayAlert("Confirm", "Are you sure you want to delete this task?", "YES", "NO");
        if (result == false)
        {
            return;
        }
        else
        {
            // loading indicator
            DeleteSprintButton.Text = "Loading...";

            // delete sprint
            DataAccessService.DeleteSprint(selectedSprint.Id);

            // reset search results
            SearchPage.instance?.ResetPage();

            // return to product page
            await Shell.Current.GoToAsync("..");
        }
    }   
}