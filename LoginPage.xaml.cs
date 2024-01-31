using CommunityToolkit.Maui.Markup;
using Capstone.Models;
using Capstone.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Capstone;

public partial class LoginPage : ContentPage
{   
    public static IList<User> userList;

    public LoginPage()
	{
        InitializeComponent();       
    }   

    protected override void OnAppearing()
	{       
        // hide navigation bar
        Shell.SetNavBarIsVisible(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);

        // refresh user list
        InitializeUserList();

        // reset fields
        UsernameEntry.Text = String.Empty;
		PasswordEntry.Text = String.Empty;
        LoginButton.Text = "Log In";
    }

    public static void InitializeUserList()
    {
        // populate user list              
        userList = MainPage.userList;
    }

    public void GrantAccess(User user)
    {
        // loading indicator
        LoginButton.Text = "Logging in...";

        // update user status
        DataAccessService.UpdateUser(user.Id, user.Username, user.Password, "true");
       
        // log-in to main page
        Shell.Current.GoToAsync("..");
    }

    public void AttemptLogin(string username, string password)
    {                
        User last = userList.Last();
        foreach (User user in userList)
        {
            if (username == user.Username && password == user.Password)
            {
                // grant access                
                GrantAccess(user);
                return;
            }
            else if (user.Equals(last))
            {                
                // deny access
                DisplayAlert("Alert", "Invalid log-in credentials.", "OK");
                UsernameEntry.Text = String.Empty;
                PasswordEntry.Text = String.Empty;
                return;
            }         
        }        
    }

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text.Trim();
        var password = PasswordEntry.Text.Trim();

        if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
        {
            DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }

        AttemptLogin(username, password);
    }
}