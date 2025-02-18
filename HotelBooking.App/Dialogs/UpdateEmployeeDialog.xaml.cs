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

        /// <summary>
        /// Gets the updated employee.
        /// </summary>
        public Employee? UpdatedEmployee { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEmployeeDialog"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used for data access.</param>
        public UpdateEmployeeDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        /// <summary>
        /// Handles the click event for the Create button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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