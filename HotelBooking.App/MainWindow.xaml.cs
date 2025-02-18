using System.Windows;
using System.Diagnostics;
using System.Configuration;
using HotelBooking.App.Pages;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly BookingStorageRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        var frame = new Frame();
        frame.Navigate(new HomePage());
        MainFrame.Content = frame;

        var dbContext = new BookingDbContext(GetConnectionString());
        repository = new BookingStorageRepository(dbContext);
    }

    /// <summary>
    /// Retrieves the database connection string from the application configuration.
    /// </summary>
    /// <returns>A properly formatted SQL connection string.</returns>
    private string GetConnectionString()
    {
        var consStringBuilder = new SqlConnectionStringBuilder
        {
            UserID = ConfigurationManager.AppSettings["Name"],
            Password = ConfigurationManager.AppSettings["Password"],
            InitialCatalog = ConfigurationManager.AppSettings["Database"],
            DataSource = ConfigurationManager.AppSettings["DataSource"],
            TrustServerCertificate = true,
            ConnectTimeout = 5
        };

        return consStringBuilder.ConnectionString;
    }

    /// <summary>
    /// Navigates to the Home page.
    /// </summary>
    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        var frame = new Frame();
        frame.Navigate(new HomePage());
        MainFrame.Content = frame;
    }

    /// <summary>
    /// Navigates to the Employees page.
    /// </summary>
    private void EmployeesButton_Click(object sender, RoutedEventArgs e)
    {
        var frame = new Frame();
        frame.Navigate(new EmployeePage(repository));
        MainFrame.Content = frame;
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        App.Current.Shutdown();
        Process.GetCurrentProcess().Kill();
    }

    /// <summary>
    /// Navigates to the Rooms page.
    /// </summary>
    private void RoomsButton_Click(object sender, RoutedEventArgs e)
    {
        var frame = new Frame();
        frame.Navigate(new RoomsPage(repository));
        MainFrame.Content = frame;
    }

    /// <summary>
    /// Navigates to the Guests page.
    /// </summary>
    private void GuestsButton_Click(object sender, RoutedEventArgs e)
    {
        var frame = new Frame();
        frame.Navigate(new GuestsPage(repository));
        MainFrame.Content = frame;
    }

    /// <summary>
    /// Navigates to the Bookings page.
    /// </summary>
    private void BookingsButton_Click(object sender, RoutedEventArgs e)
    {
        var frame = new Frame();
        frame.Navigate(new BookingsPage(repository));
        MainFrame.Content = frame;
    }
}