using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Text.Json;
using System.Windows.Controls;
using HotelBooking.App.Dialogs;
using HotelBooking.Domain.Entities;
using System.Text.Json.Serialization;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App
{
    /// <summary>
    /// Interaction logic for the Employee Page, responsible for managing employee records in the hotel booking system.
    /// This page allows viewing, adding, updating, and deleting employees from the system.
    /// </summary>
    public partial class EmployeePage : Page
    {
        /// <summary>
        /// A list of employees displayed on the page.
        /// </summary>
        private List<Employee> _employees = [];

        /// <summary>
        /// The repository responsible for interacting with the storage of employee data.
        /// </summary>
        private readonly BookingStorageRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeePage"/> class.
        /// </summary>
        /// <param name="repository">The repository used for managing employee data.</param>
        public EmployeePage(BookingStorageRepository repository)
        {
            InitializeComponent();

            this.repository = repository;

            // Set the data source for the employee grid view.
            EmployeesGridView.ItemsSource = _employees;
        }

        /// <summary>
        /// Event handler for selection changes in the EmployeesGridView. Populates the textboxes with the selected employee's details.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void EmployeesGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEmployee = EmployeesGridView.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                FirstNameTextBox.Text = selectedEmployee.Name;
                LastNameTextBox.Text = selectedEmployee.Surname;
                TitleTextBox.Text = selectedEmployee.Position.ToString();
            }
        }

        /// <summary>
        /// Event handler for reading employee data from the repository and displaying it in the EmployeesGridView.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ReadEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesGridView.ItemsSource = null;
            _employees = repository.GetAllEmployees();
            EmployeesGridView.ItemsSource = _employees;
        }

        /// <summary>
        /// Event handler for creating a new employee by showing a dialog to input employee details.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CreateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateEmployeeDialog();
            if (dialog.ShowDialog() == true)
            {
                var newEmployee = dialog.CreatedEmployee;
                _employees.Add(newEmployee);

                repository.AddEmployee(newEmployee);

                EmployeesGridView.ItemsSource = null;
                EmployeesGridView.ItemsSource = _employees;
            }
        }

        /// <summary>
        /// Event handler for updating an existing employee by showing a dialog to modify employee details.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UpdateEmployeeDialog(repository);
            if (dialog.ShowDialog() == true)
            {
                var newEmployee = dialog.UpdatedEmployee;
                var index = _employees.FindIndex(r => r.Name == newEmployee.Name && r.Surname == newEmployee.Surname);
                _employees[index] = newEmployee;
                EmployeesGridView.ItemsSource = null;
                EmployeesGridView.ItemsSource = _employees;
            }
        }

        /// <summary>
        /// Event handler for deleting the selected employee from the list and the repository.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeesGridView.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                _employees.Remove(selectedEmployee);
                repository.DeleteEmployee(selectedEmployee);
                EmployeesGridView.ItemsSource = null;
                EmployeesGridView.ItemsSource = _employees;
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.");
            }
        }

        /// <summary>
        /// Event handler for importing employee data from a JSON file and adding it to the repository.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ImportRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Open JSON File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                try
                {
                    string jsonData = File.ReadAllText(filePath, Encoding.UTF8);
                    var employees = JsonSerializer.Deserialize<List<Employee>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    });

                    foreach (var item in employees) item.Id = 0;

                    if (employees != null && employees.Any())
                    {
                        repository.ImportEmployees(employees);
                        MessageBox.Show("Data imported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No valid data found in the JSON file.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}