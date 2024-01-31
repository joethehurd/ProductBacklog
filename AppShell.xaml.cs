namespace Capstone
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
           
            Routing.RegisterRoute(nameof(ViewProductPage), typeof (ViewProductPage));
            Routing.RegisterRoute(nameof(ViewSprintPage), typeof (ViewSprintPage));
            Routing.RegisterRoute(nameof(ViewRequirementPage), typeof (ViewRequirementPage));
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
            Routing.RegisterRoute(nameof(AddSprintPage), typeof(AddSprintPage));
            Routing.RegisterRoute(nameof(AddRequirementPage), typeof(AddRequirementPage));
            Routing.RegisterRoute(nameof(EditProductPage), typeof(EditProductPage));
            Routing.RegisterRoute(nameof(EditSprintPage), typeof(EditSprintPage));
            Routing.RegisterRoute(nameof(EditRequirementPage), typeof(EditRequirementPage));           
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));           
        }
    }
}