using Capstone.Services;
using SQLitePCL;

namespace Capstone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Initialize Database
            Batteries.Init();
            DataAccessService.Init();

            MainPage = new AppShell();
        }
    }
}