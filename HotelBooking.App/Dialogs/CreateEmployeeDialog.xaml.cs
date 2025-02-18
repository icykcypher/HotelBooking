using System.Windows;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Entities;

namespace HotelBooking.App
{
    /// <summary>
    /// Interaction logic for CreateEmployeeDialog.xaml
    /// </summary>
    public partial class CreateEmployeeDialog : Window
    {
        /// <summary>
        /// Gets the created employee.
        /// </summary>
        public Employee? CreatedEmployee { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEmployeeDialog"/> class.
        /// </summary>
        public CreateEmployeeDialog()
        {
            InitializeComponent();
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
                string.IsNullOrEmpty(TitleTextBox.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            CreatedEmployee = new Employee
            {
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                Position = (EmployeePosition)Enum.Parse(typeof(EmployeePosition), TitleTextBox.Text),
            };

            this.DialogResult = true;
            this.Close();
        }
    }
}