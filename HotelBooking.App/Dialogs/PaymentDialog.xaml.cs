using System.Windows;
using System.Windows.Controls;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Entities;

namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for PaymentDialog.xaml
    /// </summary>
    public partial class PaymentDialog : Window
    {
        private readonly Booking _booking;
        private readonly decimal amount;

        /// <summary>
        /// Gets or sets the payment details.
        /// </summary>
        public Payment Payment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentDialog"/> class.
        /// </summary>
        /// <param name="booking">The booking associated with the payment.</param>
        /// <param name="amount">The amount to be paid.</param>
        public PaymentDialog(Booking booking, decimal amount)
        {
            InitializeComponent();
            _booking = booking;
            this.amount = amount;
            AmountTextBox.Text = amount.ToString();
        }

        /// <summary>
        /// Handles the click event for the Save button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && PaymentStatusComboBox.SelectedItem != null && PaymentDatePicker.SelectedDate.HasValue)
            {
                var paymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), ((ComboBoxItem)PaymentStatusComboBox.SelectedValue).Content.ToString());
                var paymentDate = PaymentDatePicker.SelectedDate.Value;

                var payment = new Payment
                {
                    BookingId = _booking.Id,
                    Booking = _booking,
                    Amount = amount,
                    PaymentStatus = paymentStatus,
                    PaymentDate = paymentDate
                };

                Payment = payment;

                MessageBox.Show("Payment added successfully!");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly.");
            }
        }

        /// <summary>
        /// Handles the click event for the Cancel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}