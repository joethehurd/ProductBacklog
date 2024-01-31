using Capstone.Models;
using Capstone.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Capstone;

public partial class ReportsPage : ContentPage
{
    public static ReportsPage instance;

    public static IList<Product> productList;
    public static IList<Sprint> sprintList;

    public static List<Report> reportList = new List<Report>();

    public static List<string> typeOptions = new List<string>()
    {
        "Product",
        "Task"
    };

    public static List<string> eventOptions = new List<string>()
    {
        "Kickoff",
        "Deadline"
    };

    public static List<string> monthOptions = new List<string>()
    {
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    };

    public ReportsPage()
	{
        instance = this;
        InitializeComponent();     
    }

    public void ResetPage()
    {
        // reset report list
        reportList = new List<Report>();
        ReportCollectionView.ItemsSource = reportList;

        // reset labels
        TypeLabel.Text = String.Empty;
        EventLabel.Text = String.Empty;
        TypeLabel.TextDecorations = TextDecorations.Underline;       

        // reset picker fields if needed
        ReportTypePicker.SelectedItem = null;
        ReportEventPicker.SelectedItem = null;
        ReportMonthPicker.SelectedItem = null;
    }

    protected override void OnAppearing()
    {
        // reset button text
        GenerateReportButton.Text = "Generate Report";

        // populate product list
        productList = DataAccessService.GetProductsByUser(MainPage.activeUser.Id);

        // populate sprint list
        sprintList = DataAccessService.GetSprintsByUser(MainPage.activeUser.Id);

        // populate xaml type picker with type options
        ReportTypePicker.ItemsSource = typeOptions;

        // populate xaml event picker with event options
        ReportEventPicker.ItemsSource = eventOptions;

        // populate xaml month picker with month options
        ReportMonthPicker.ItemsSource = monthOptions;

        // reset search results
        SearchPage.instance?.ResetPage();
    }

    public void GenerateReport()
    {
        // search for matches
        if (ReportTypePicker.SelectedItem.ToString() == "Product")
        {
            foreach (Product product in productList)
            {
                if (ReportEventPicker.SelectedItem.ToString() == "Kickoff")
                {
                    if (product.Start.ToLocalTime().ToString("MMMM") == ReportMonthPicker.SelectedItem.ToString())
                    {                        
                        var result = new Report
                        {
                            Name = product.Name,
                            Date = product.Start.ToLocalTime().ToString("MMMM d, yyyy")
                        };

                        reportList.Add(result);                       
                    }
                }
                else if (ReportEventPicker.SelectedItem.ToString() == "Deadline")
                {
                    if (product.End.ToLocalTime().ToString("MMMM") == ReportMonthPicker.SelectedItem.ToString())
                    {                        
                        var result = new Report
                        {
                            Name = product.Name,
                            Date = product.End.ToLocalTime().ToString("MMMM d, yyyy")
                        };

                        reportList.Add(result);                       
                    }
                }
            }
        }
        else if (ReportTypePicker.SelectedItem.ToString() == "Task")
        {
            foreach (Sprint sprint in sprintList)
            {
                if (ReportEventPicker.SelectedItem.ToString() == "Kickoff")
                {
                    if (sprint.Start.ToLocalTime().ToString("MMMM") == ReportMonthPicker.SelectedItem.ToString())
                    {                        
                        var result = new Report
                        {
                            Name = sprint.Name,
                            Date = sprint.Start.ToLocalTime().ToString("MMMM d, yyyy")
                        };

                        reportList.Add(result);
                    }
                }
                else if (ReportEventPicker.SelectedItem.ToString() == "Deadline")
                {
                    if (sprint.End.ToLocalTime().ToString("MMMM") == ReportMonthPicker.SelectedItem.ToString())
                    {
                        var result = new Report
                        {
                            Name = sprint.Name,
                            Date = sprint.End.ToLocalTime().ToString("MMMM d, yyyy")
                        };

                        reportList.Add(result);
                    }
                }
            }
        }
        
        // format labels
        if (reportList.Count == 0)
        {
            EventLabel.Text = String.Empty;
            TypeLabel.TextDecorations = TextDecorations.None;         

            if (ReportTypePicker.SelectedItem.ToString() == "Product")
            {
                TypeLabel.Text = "No products match the selected criteria.";
            }
            else if (ReportTypePicker.SelectedItem.ToString() == "Task")
            {
                TypeLabel.Text = "No tasks match the selected criteria.";
            }           
        }       
        else if (reportList.Count == 1)
        {
            TypeLabel.TextDecorations = TextDecorations.Underline;          

            if (ReportTypePicker.SelectedItem.ToString() == "Product")
            {
                TypeLabel.Text = "Product";
            }
            else if (ReportTypePicker.SelectedItem.ToString() == "Task")
            {
                TypeLabel.Text = "Task";
            }

            if (ReportEventPicker.SelectedItem.ToString() == "Kickoff")
            {
                EventLabel.Text = "Start Date";
            }
            else if (ReportEventPicker.SelectedItem.ToString() == "Deadline")
            {
                EventLabel.Text = "Due Date";
            }          
        }
        else
        {
            TypeLabel.TextDecorations = TextDecorations.Underline;           

            if (ReportTypePicker.SelectedItem.ToString() == "Product")
            {
                TypeLabel.Text = "Products";
            }
            else if (ReportTypePicker.SelectedItem.ToString() == "Task")
            {
                TypeLabel.Text = "Tasks";
            }

            if (ReportEventPicker.SelectedItem.ToString() == "Kickoff")
            {
                EventLabel.Text = "Start Date";
            }
            else if (ReportEventPicker.SelectedItem.ToString() == "Deadline")
            {
                EventLabel.Text = "Due Date";
            }            
        }

        // order list            
        var orderedList = reportList.OrderBy(r => DateTime.Parse(r.Date));

        // display results
        ReportCollectionView.ItemsSource = orderedList;
       
        // reset button text
        GenerateReportButton.Text = "Generate Report";
    }

    private void GenerateReportButton_Clicked(object sender, EventArgs e)
    {
        // validate fields
        if (ReportTypePicker.SelectedItem is null)
        {
            // Report Type = Empty
            DisplayAlert("Alert", "All fields with * must be selected.", "OK");
            return;
        }
        if (ReportEventPicker.SelectedItem is null)
        {
            // Report Event = Empty
            DisplayAlert("Alert", "All fields with * must be selected.", "OK");
            return;
        }
        if (ReportMonthPicker.SelectedItem is null)
        {
            // Report Month = Empty
            DisplayAlert("Alert", "All fields with * must be selected.", "OK");
            return;
        }

        // loading indicator
        GenerateReportButton.Text = "Loading...";

        // prepare list
        reportList = new List<Report>();
        ReportCollectionView.ItemsSource = reportList;

        // generate report
        GenerateReport();
    }
}