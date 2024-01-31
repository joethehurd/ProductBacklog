using CommunityToolkit.Maui.Markup;
using Capstone.Models;
using Capstone.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Capstone;

public partial class LogoutPage : ContentPage
{   
    public LogoutPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
	{
        Logout();
    }

    public static void Logout()
    {        
        // update user status
        DataAccessService.UpdateUser(MainPage.activeUser.Id, MainPage.activeUser.Username, MainPage.activeUser.Password, "false");
        
        // prepare main page
        MainPage.loggedIn = false;
        MainPage.firstLoad = true;
        
        // redirect to main page
        Application.Current.MainPage.Navigation.PopAsync();
        Application.Current.MainPage = new AppShell();
    }
}