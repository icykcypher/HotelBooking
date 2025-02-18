using System.Windows;
using System.Windows.Controls;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;
using HotelBooking.App.Dialogs;
using Microsoft.Win32;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.IO;

namespace HotelBooking.App
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        private List<Employee> _employees = [];
        private readonly BookingStorageRepository repository;

        public EmployeePage(BookingStorageRepository repository)
        {
            InitializeComponent();

            this.repository = repository;

            EmployeesGridView.ItemsSource = _employees;
        }

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

        private void ReadEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesGridView.ItemsSource = null;
            _employees = repository.GetAllEmployees();
            EmployeesGridView.ItemsSource = _employees;
        }

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