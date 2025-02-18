using System.Windows;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for UpdateEmployeeDialog.xaml
    /// </summary>
    public partial class UpdateEmployeeDialog : Window
    {
        private readonly BookingStorageRepository repository;

        public Employee? UpdatedEmployee { get; private set; }

        public UpdateEmployeeDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(TitleTextBox.Text) || string.IsNullOrEmpty(EmployeeId.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }


            UpdatedEmployee = new Employee
            {
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                Position = (EmployeePosition)Enum.Parse(typeof(EmployeePosition), TitleTextBox.Text),
            };

            try
            {
                if (repository.UpdateEmployee(int.Parse(EmployeeId.Text), UpdatedEmployee))
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot update employee: {ex.Message}");
                this.DialogResult = false;
            }
        }
    }
}