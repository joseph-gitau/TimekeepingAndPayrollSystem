using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimekeepingAndPayrollSystem
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<WeeklyHours> WeeklyHoursData { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitializeWeeklyHoursData();
            dgWeeklyHours.ItemsSource = WeeklyHoursData;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
                // Set the Placeholder text accordingly
                switch (textBox.Name)
                {
                    case "txtEmployeeName":
                        textBox.Text = "Enter employee name";
                        break;
                    case "txtSupervisorName":
                        textBox.Text = "Enter supervisor name";
                        break;
                    case "txtWeekNumber":
                        textBox.Text = "Enter week number";
                        break;
                }
            }
            // Ensure txtWeekNumber is a valid number
            if (textBox.Name == "txtWeekNumber" && !int.TryParse(textBox.Text, out int weekNumber))
            {
                MessageBox.Show("Please enter a valid week number (1-52).");
                textBox.Text = "Enter week number";
                textBox.Foreground = Brushes.Gray;
            } 
            else if (textBox.Name == "txtWeekNumber" && (int.Parse(textBox.Text) < 1 || int.Parse(textBox.Text) > 52))
            {
                MessageBox.Show("Please enter a valid week number (1-52).");
                textBox.Text = "Enter week number";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void InitializeWeeklyHoursData()
        {
            // Sample data initialization
            WeeklyHoursData = new ObservableCollection<WeeklyHours>
            {

                 // Add an empty row for gap
                new WeeklyHours
                {
                    IsGap = true // You will need to handle this property in your DataGrid style to make the row appear as a gap
                },

                // Initialize Fixed bottom row for weekend/holiday/vacation checkboxes
                new WeeklyHours
                {
                    ClientName = "Weekend/Holiday/Vacation"
                }
            };
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (ValidateInputs())
                {
                    CalculatePayroll();
                    MessageBox.Show("Payroll calculated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please correct the input errors.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while calculating payroll: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private bool ValidateInputs()
        {
            try
            { 
                // Validate Employee Name
                if (string.IsNullOrWhiteSpace(txtEmployeeName.Text) || txtEmployeeName.Text == "Enter employee name")
                {
                    MessageBox.Show("Please enter the employee name.");
                    return false;
                }

                // Validate Supervisor Name
                if (string.IsNullOrWhiteSpace(txtSupervisorName.Text) || txtSupervisorName.Text == "Enter supervisor name")
                {
                    MessageBox.Show("Please enter the supervisor name.");
                    return false;
                }
    
                // Additional validations can be added here, such as checking the list of daily hours for each day
                // Validate Week Number
                if (!int.TryParse(txtWeekNumber.Text, out int weekNumber) || weekNumber < 1 || weekNumber > 52)
                {
                    MessageBox.Show("Please enter a valid week number (1-52).");
                    return false;
                }
                return true; // All validations passed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while validating inputs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        private void CalculatePayroll()
        {
            const decimal regularHourRate = 15m;
            const decimal overtimeMultiplier = 1.5m;
            const int regularHoursPerWeek = 40;

            decimal totalHoursWorked = 0m;
            

            decimal regularHours = Math.Min(totalHoursWorked, regularHoursPerWeek);
            decimal overtimeHours = Math.Max(0, totalHoursWorked - regularHoursPerWeek);

            decimal regularPay = regularHours * regularHourRate;
            decimal overtimePay = overtimeHours * regularHourRate * overtimeMultiplier;
            decimal grossPay = regularPay + overtimePay;

          
        }



        private void ResetFields()
        {
            txtEmployeeName.Text = "Enter employee name";
            txtSupervisorName.Text = "Enter supervisor name";
            txtWeekNumber.Text = "Enter week number";


            // Reset the whole datagrid
        }

        // Helper method to find all visual children of a specified type within a parent control
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private void dgWeeklyHours_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Optionally, you can add logic here to ensure that the entered value is a number before trying to parse it.
            var textBox = e.EditingElement as TextBox;
            if (textBox != null && int.TryParse(textBox.Text, out int value))
            {
                if (value > 24)
                {
                    MessageBox.Show("You cannot enter more than 24 hours for a single day.");
                    // You may want to reset the edited value here or mark it in some way.
                }
                else
                {
                    // The value is valid, so update totals if necessary.
                    CalculateTotals();
                }
            }
        }

        private void CalculateTotals()
        {
            // Assuming WeeklyHoursData is the collection bound to the DataGrid
            foreach (var entry in WeeklyHoursData)
            {
                entry.TotalHours = entry.HoursMonday + entry.HoursTuesday + entry.HoursWednesday + entry.HoursThursday + entry.HoursFriday + entry.HoursSaturday + entry.HoursSunday;
            }

            // Refresh the DataGrid to show the updated totals
            dgWeeklyHours.Items.Refresh();
        }

    }
}
