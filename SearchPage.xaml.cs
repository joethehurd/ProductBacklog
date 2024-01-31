using Capstone.Models;
using Capstone.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Capstone;

public partial class SearchPage : ContentPage
{
    public static SearchPage instance;

    public static IList<Product> productList;
    public static IList<Sprint> sprintList;
    public static IList<Requirement> requirementList;

    public static List<Product> productResults = new List<Product>();
    public static List<Sprint> sprintResults = new List<Sprint>();
    public static List<Requirement> requirementResults = new List<Requirement>();

    public static List<Button> buttonList = new List<Button>();

    public static int resultsCount = 0;

    public SearchPage()
    {
        instance = this;
        InitializeComponent();
        SearchEntry.ReturnCommand = SearchCommand;     
    }

    protected override void OnAppearing()
    {
        // reset button text
        SearchButton.Text = "Search";
        ResetViewButtons();

        PopulateLists();

        // reset report results       
        ReportsPage.instance?.ResetPage();
    }

    public static void ResetViewButtons()
    {
        if (buttonList.Any())
        {
            foreach (Button button in buttonList)
            {
                button.Text = "View";
            }

            // reset list that remembers clicked buttons
            buttonList = new List<Button>();
        }       
    }

    public static void PopulateLists()
    {
        // populate the lists that will be searched
        productList = DataAccessService.GetProductsByUser(MainPage.activeUser.Id);
        sprintList = DataAccessService.GetSprintsByUser(MainPage.activeUser.Id);
        requirementList = DataAccessService.GetRequirementsByUser(MainPage.activeUser.Id);       
    }

    public void ResetPage()
    {
        // clear search box
        SearchEntry.Text = String.Empty;

        // reset result lists
        productResults = new List<Product>();
        ProductCollectionView.ItemsSource = productResults;

        sprintResults = new List<Sprint>();
        SprintCollectionView.ItemsSource = sprintResults;

        requirementResults = new List<Requirement>();
        RequirementCollectionView.ItemsSource = requirementResults;

        // clear labels
        ResultsLabel.Text = String.Empty;
        ProductsLabel.Text = String.Empty;
        SprintsLabel.Text = String.Empty;
        RequirementsLabel.Text = String.Empty;

        // reset results counter
        resultsCount = 0;
    }

    // allows search to be performed from keyboard
    public ICommand SearchCommand => new Command(SearchAll);

    // search function
    public void SearchAll()
    {
        SearchEntry.IsEnabled = false;
        SearchEntry.IsEnabled = true;

        if (!String.IsNullOrWhiteSpace(SearchEntry.Text))
        {
            // prepare search
            var searchTerm = SearchEntry.Text.Trim();
            ResetPage();
            SearchEntry.Text = searchTerm;

            // perform search
            SearchProducts(searchTerm);
            SearchSprints(searchTerm);
            SearchRequirements(searchTerm);

            // populate xaml lists based on results
            if (productResults.Any())
            {
                ProductsLabel.Text = "Products";
                ProductCollectionView.ItemsSource = productResults;
            }
            else
            {
                ProductsLabel.Text = String.Empty;
            }

            if (sprintResults.Any())
            {
                SprintsLabel.Text = "Tasks";
                SprintCollectionView.ItemsSource = sprintResults;
            }
            else
            {
                SprintsLabel.Text = String.Empty;
            }

            if (requirementResults.Any())
            {
                RequirementsLabel.Text = "Requirements";
                RequirementCollectionView.ItemsSource = requirementResults;
            }
            else
            {
                RequirementsLabel.Text = String.Empty;
            }

            if (resultsCount == 1)
            {
                ResultsLabel.Text = $"Results: {resultsCount} match found.";
            }
            else
            {
                ResultsLabel.Text = $"Results: {resultsCount} matches found.";
            }            
        }

        // reset button text
        SearchButton.Text = "Search";
    }

    public static void SearchProducts(string searchTerm)
    {
        foreach (Product product in productList)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(product))
            {
                string name = descriptor.Name;
                string value = descriptor.GetValue(product).ToString();                

                if (name != "Id")
                {
                    if (name == "Start")
                    {
                        value = product.Start.ToLocalTime().ToString("MMMM");
                    }
                    else if (name == "End")
                    {
                        value = product.End.ToLocalTime().ToString("MMMM");
                    }
                    else if (name == "Status")
                    {
                        switch (product.Status)
                        {
                            case 1:
                                value = "Not started";
                                break;
                            case 2:
                                value = "In progress";
                                break;
                            case 3:
                                value = "Completed";
                                break;
                        }
                    }
                    else if (name == "Priority")
                    {
                        switch (product.Priority)
                        {
                            case 1:
                                value = "High";
                                break;
                            case 2:
                                value = "Medium";
                                break;
                            case 3:
                                value = "Low";
                                break;
                        }
                    }                    

                    if (value.ToLower().Contains(searchTerm.ToLower()))
                    {
                        productResults.Add(product);
                        resultsCount++;
                        break;
                    }
                }              
            }
        }
    }

    public static void SearchSprints(string searchTerm)
    {
        foreach (Sprint sprint in sprintList)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(sprint))
            {
                string name = descriptor.Name;
                string value = descriptor.GetValue(sprint).ToString();

                if (name != "Id" && name != "ProductId")
                {
                    if (name == "Start")
                    {
                        value = sprint.Start.ToLocalTime().ToString("MMMM");
                    }
                    else if (name == "End")
                    {
                        value = sprint.End.ToLocalTime().ToString("MMMM");
                    }
                    else if (name == "Status")
                    {
                        switch (sprint.Status)
                        {
                            case 1:
                                value = "Not started";
                                break;
                            case 2:
                                value = "In progress";
                                break;
                            case 3:
                                value = "Completed";
                                break;
                        }
                    }
                    else if (name == "Priority")
                    {
                        switch (sprint.Priority)
                        {
                            case 1:
                                value = "High";
                                break;
                            case 2:
                                value = "Medium";
                                break;
                            case 3:
                                value = "Low";
                                break;
                        }
                    }                  

                    if (value.ToLower().Contains(searchTerm.ToLower()))
                    {
                        sprintResults.Add(sprint);
                        resultsCount++;
                        break;
                    }
                }
            }
        }
    }

    public static void SearchRequirements(string searchTerm)
    {
        foreach (Requirement requirement in requirementList)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(requirement))
            {
                string name = descriptor.Name;
                string value = descriptor.GetValue(requirement).ToString();  
                
                if (name != "Id" && name != "SprintId")
                {
                    if (value.ToLower().Contains(searchTerm.ToLower()))
                    {
                        requirementResults.Add(requirement);
                        resultsCount++;
                        break;
                    }
                }                
            }
        }
    }

    private void SearchButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        SearchButton.Text = "Loading...";

        SearchAll();     
    }

    private async void ViewProductButton_Clicked(object sender, EventArgs e)
    {
        // gets the id of the selected product          
        var tempId = (Button)sender;
      
        // loading indicator
        tempId.Text = "Loading...";

        // remember clicked button
        buttonList.Add(tempId);

        // passes the selected Product's ID to the View Product Page
        await Shell.Current.GoToAsync($"{nameof(ViewProductPage)}?id={tempId.CommandParameter}");
    }

    private async void ViewSprintButton_Clicked(object sender, EventArgs e)
    {
        // gets the id of the selected sprint          
        var tempId = (Button)sender;
       
        // loading indicator
        tempId.Text = "Loading...";

        // remember clicked button
        buttonList.Add(tempId);

        // passes the selected Sprint's ID to the View Sprint Page
        await Shell.Current.GoToAsync($"{nameof(ViewSprintPage)}?id={tempId.CommandParameter}");
    }

    private async void ViewRequirementButton_Clicked(object sender, EventArgs e)
    {
        // gets the id of the selected requirement       
        var tempId = (Button)sender;
       
        // loading indicator
        tempId.Text = "Loading...";

        // remember clicked button
        buttonList.Add(tempId);

        // passes the selected Requirement's ID to the View Requirement Page
        await Shell.Current.GoToAsync($"{nameof(ViewRequirementPage)}?id={tempId.CommandParameter}");
    }   
}