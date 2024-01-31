using Capstone.Models;
using Capstone.Services;
using Plugin.LocalNotification;

namespace Capstone;

[QueryProperty("ReceivedId", "id")]

public partial class ViewProductPage : ContentPage
{
    public static IList<Sprint> sprintList;
    public static Product selectedProduct;
    
    public static string ReceivedId
    {
        set
        {
            // retrieve selected Product
            Guid ProductId = new Guid(value);           
            selectedProduct = DataAccessService.GetProduct(ProductId);            
        }
    }

    public ViewProductPage()
	{
		InitializeComponent();       
	}

    protected override void OnAppearing()
    {
        // reset button text
        AddSprintButton.Text = "Add Task";
        EditProductButton.Text = "Edit Product";
        DeleteProductButton.Text = "Delete Product";

        LoadDetails(selectedProduct.Id);
    }
   
    public void LoadDetails(Guid productId)
    {               
        // format product dates
        var startDate = selectedProduct.Start.ToLocalTime().ToString("MMMM d, yyyy");
        var endDate = selectedProduct.End.ToLocalTime().ToString("MMMM d, yyyy");

        // set product details in xaml
        ProductName.Text = selectedProduct.Name;
        ProductDates.Text = $"{startDate} to {endDate}";
        ProductOwnerNameLabel.Text = selectedProduct.OwnerName;
        ProductOwnerPhoneLabel.Text = selectedProduct.OwnerPhone;
        ProductOwnerEmailLabel.Text = selectedProduct.OwnerEmail;
        switch (selectedProduct.Status)
        {
            case 1:
                ProductStatusLabel.Text = "Not started";
                break;
            case 2:
                ProductStatusLabel.Text = "In progress";
                break;
            case 3:
                ProductStatusLabel.Text = "Completed";
                break;
        }
        switch (selectedProduct.Priority)
        {
            case 1:
                ProductPriorityLabel.Text = "High";
                break;
            case 2:
                ProductPriorityLabel.Text = "Medium";
                break;
            case 3:
                ProductPriorityLabel.Text = "Low";
                break;
        }
        ProductAlertsLabel.Text = selectedProduct.Alerts;

        // refresh sprint list
        RefreshSprintList(productId);        
    }
    
    public void RefreshSprintList(Guid productId)
    {
        // populate sprint list
        sprintList = DataAccessService.GetSprintsByProduct(productId);

        // order list        
        var orderedList = sprintList.OrderBy(s => s.Start).ThenBy(s => s.End).ThenBy(s => s.Status).ThenBy(s => s.Priority);

        // populate xaml collection view with ordered list items       
        SprintCollectionView.ItemsSource = orderedList;

        // determine if label is needed
        if (!sprintList.Any())
        {
            ProductTasksLabel.Text = String.Empty;
        }
        else
        {
            ProductTasksLabel.Text = "Tasks";
        }
    }   
       
    private async void ViewSprintButton_Clicked(object sender, EventArgs e)
    {     
        // gets the id of the selected sprint          
        var tempId = (Button)sender;

        // loading indicator
        tempId.Text = "Loading...";

        // passes the selected Sprint's ID to the View Sprint Page
        await Shell.Current.GoToAsync($"{nameof(ViewSprintPage)}?id={tempId.CommandParameter}");       
    }
       
    private async void AddSprintButton_Clicked(object sender, EventArgs e)
    {
        // check sprint limit
        if (sprintList.Count == 24)
        {            
            await DisplayAlert("Alert", "Task limit [24] reached.", "OK");
            return;
        }
        else
        {
            // loading indicator
            AddSprintButton.Text = "Loading...";

            // go to add sprint page
            await Shell.Current.GoToAsync(nameof(AddSprintPage));
        }
    }
  
    private async void EditProductButton_Clicked(object sender, EventArgs e)
    {
        // loading indicator
        EditProductButton.Text = "Loading...";

        // go to edit product page
        await Shell.Current.GoToAsync(nameof(EditProductPage));
    }

    private async void DeleteProductButton_Clicked(object sender, EventArgs e)
    {
        // confirm delete
        bool result = await DisplayAlert("Confirm", "Are you sure you want to delete this product?", "YES", "NO");
        if (result == false)
        {
            return;
        }
        else
        {
            // loading indicator
            DeleteProductButton.Text = "Loading...";

            // delete product
            DataAccessService.DeleteProduct(selectedProduct.Id);

            // reset search results
            SearchPage.instance?.ResetPage();

            // return to home page
            await Shell.Current.GoToAsync("..");
        }      
    }    
}