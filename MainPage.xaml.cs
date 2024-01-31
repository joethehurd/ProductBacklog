using CommunityToolkit.Maui.Markup;
using Capstone.Models;
using Capstone.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Plugin.LocalNotification;

namespace Capstone
{
    public partial class MainPage : ContentPage
    {
        public static User activeUser;

        public static IList<User> userList;
        public static IList<Product> productList; 
        public static IList<Sprint> sprintList;       

        public static bool loggedIn = false;
        public static bool firstLoad = true;

        public MainPage()
        {           
            InitializeComponent();          
        }
        
        protected override void OnAppearing()
        {           
            // check if logged in         
            if (loggedIn == false)
            {
                // populate user list
                userList = DataAccessService.GetAllUsers();

                // check log-in status
                foreach (User user in userList)
                {                    
                    if (user.LoggedIn == "true")
                    {
                        // grant access
                        activeUser = user;
                        loggedIn = true;
                        InitializeDisplay();
                        return;
                    }
                }

                // redirect to login page
                Shell.Current.GoToAsync(nameof(LoginPage));                
            }
            else
            {
                // display main page
                InitializeDisplay();            
            }           
        }

        public void InitializeDisplay()
        {            
            // show navigation bar
            Shell.SetNavBarIsVisible(this, true);
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);

            // reset button text
            AddProductButton.Text = "Add Product";

            // check if database needs to be populated
            if (firstLoad == true)
            {
                InitializeProductList();
            }
            else
            {
                RefreshProductList();
            }

            // reset search and report results
            SearchPage.instance?.ResetPage();
            ReportsPage.instance?.ResetPage();
        }

        public void InitializeProductList()
        {
            RefreshProductList();

            if (!productList.Any())
            {
                // database is empty, populate with default data                
                DataAccessService.PopulateTables();
                RefreshProductList();
            }

            firstLoad = false;
        }
       
        public void RefreshProductList()
        {
            // populate product list
            productList = DataAccessService.GetProductsByUser(activeUser.Id);

            // order list            
            var orderedList = productList.OrderBy(p => p.Start).ThenBy(p => p.End).ThenBy(p => p.Status).ThenBy(p => p.Priority);

            // populate sprint list (for alerts only)
            sprintList = DataAccessService.GetSprintsByUser(activeUser.Id);

            // populate xaml collection view with ordered list items
            ProductCollectionView.ItemsSource = orderedList;
           
            // check for alerts
            if (productList.Count > 0 && firstLoad == true)
            {
                CheckForAlerts();
            }

            // determine if label is needed
            if (!productList.Any())
            {
                ProductsLabel.Text = String.Empty;
            }
            else
            {
                ProductsLabel.Text = "Products";
            }
        }
     
        public void CheckForAlerts()
        {
            foreach (Sprint sprint in sprintList)
            {
                if (sprint.Alerts == "On")
                {
                    if (sprint.Start.ToLocalTime().Month == DateTime.Now.Month)
                    {
                        if (sprint.Start.ToLocalTime() == DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Task Notification",
                                Subtitle = "Kickoff",
                                Description = $"The following task kicks off today: {sprint.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following task kicks off today: {sprint.Name}.", "OK"); };
                        }
                        else if (sprint.Start.ToLocalTime() > DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Task Notification",
                                Subtitle = "Kickoff",
                                Description = $"The following task kicks off this month: {sprint.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following task kicks off this month: {sprint.Name}.", "OK"); };
                        }
                    }

                    if (sprint.End.ToLocalTime().Month == DateTime.Now.Month)
                    {
                        if (sprint.End.ToLocalTime() == DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Task Notification",
                                Subtitle = "Deadline",
                                Description = $"The following task is due today: {sprint.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following task is due today: {sprint.Name}.", "OK"); };
                        }
                        else if (sprint.End.ToLocalTime() > DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Task Notification",
                                Subtitle = "Deadline",
                                Description = $"The following task is due this month: {sprint.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following task is due this month: {sprint.Name}.", "OK"); };
                        }
                    }
                }
            }

            foreach (Product product in productList)
            {                
                if (product.Alerts == "On")
                {             
                    if (product.Start.ToLocalTime().Month == DateTime.Now.Month)
                    {
                        if (product.Start.ToLocalTime() == DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Product Notification",
                                Subtitle = "Kickoff",
                                Description = $"The following product kicks off today: {product.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following product kicks off today: {product.Name}.", "OK"); };                                                                                
                        }
                        else if (product.Start.ToLocalTime() > DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Product Notification",
                                Subtitle = "Kickoff",
                                Description = $"The following product kicks off this month: {product.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following product kicks off this month: {product.Name}.", "OK"); };                                                   
                        }
                    }
                    if (product.End.ToLocalTime().Month == DateTime.Now.Month)
                    {
                        if (product.End.ToLocalTime() == DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Product Notification",
                                Subtitle = "Deadline",
                                Description = $"The following product is due today: {product.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following product is due today: {product.Name}.", "OK"); };                                                     
                        }
                        else if (product.End.ToLocalTime() > DateTime.Today)
                        {
                            var notification = new NotificationRequest
                            {
                                Title = "Product Notification",
                                Subtitle = "Deadline",
                                Description = $"The following product is due this month: {product.Name}."
                            };

                            LocalNotificationCenter.Current.Show(notification);
                            this.Loaded += (s, e) => { DisplayAlert("Notification", $"The following product is due this month: {product.Name}.", "OK"); };                                                       
                        }
                    }                 
                }
            }            
        }
       
        private async void AddProductButton_Clicked(object sender, EventArgs e)
        {                   
            // check product limit
            if (productList.Count == 36)
            {                
                await DisplayAlert("Alert", "Product limit [36] reached.", "OK");
                return;
            }
            else
            {
                // loading indicator
                AddProductButton.Text = "Loading...";

                // go to add product page
                await Shell.Current.GoToAsync(nameof(AddProductPage));             
            }                      
        }              

        private async void ViewProductButton_Clicked(object sender, EventArgs e)
        {
            // gets the id of the selected product          
            var tempId = (Button)sender;

            // loading indicator
            tempId.Text = "Loading...";

            // passes the selected Product's ID to the View Product Page
            await Shell.Current.GoToAsync($"{nameof(ViewProductPage)}?id={tempId.CommandParameter}");
        }
    }
}