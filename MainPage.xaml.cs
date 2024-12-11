using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace StudentRegistrationSystem
{
    public partial class MainPage : ContentPage
    {
        private readonly StudentDatabase _db; // Database reference

        public MainPage()
        {
            InitializeComponent();
            _db = new StudentDatabase("students.db");  // Initialize the database connection
        }

        // Register a new student (Only validate, no save yet)
        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            // Get the values from the input fields
            string firstName = FirstNameEntry.Text;
            string middleName = MiddleNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string email = EmailEntry.Text;
            string phoneNumber = PhoneEntry.Text;
            string lrn = LRNEntry.Text?.Trim();  // Trim whitespace from LRN

            // Validation checks for required fields
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Please fill in all required fields (First Name, Last Name, Email).", "OK");
                return;
            }

            // Validate LRN (must be numeric and allow zero)
            if (string.IsNullOrWhiteSpace(lrn) || !IsValidLrn(lrn))
            {
                await DisplayAlert("Error", "Please enter a valid LRN (numeric only, including zero).", "OK");
                return;
            }

            // Convert to long after validating
            long parsedLrn;
            if (!long.TryParse(lrn, out parsedLrn))
            {
                await DisplayAlert("Error", "Please enter a valid numeric LRN (larger numbers are allowed).", "OK");
                return;
            }

            // Check if the email already exists using the database (email uniqueness check)
            if (!await _db.IsEmailUniqueAsync(email))
            {
                await DisplayAlert("Error", "This email is already registered.", "OK");
                return;
            }

            // Check if the LRN already exists using the database (LRN uniqueness check)
            if (!await _db.IsLrnUniqueAsync(parsedLrn))
            {
                await DisplayAlert("Error", "This LRN is already registered.", "OK");
                return;
            }

            // Create the new student object with the entered data, but do not save it yet
            var newStudent = new StudentDatabase.Student
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                RegistrationDate = DateTime.Now,
                LRN = parsedLrn,  // Store parsed LRN as long
            };

            // Navigate to the next page with the student data, without saving it yet
            await Navigation.PushAsync(new CourseSelectionPage(newStudent));
        }

        // This method ensures that the LRN only contains numeric digits (including zero)
        private bool IsValidLrn(string lrn)
        {
            // Check if all characters in LRN are digits (numeric)
            return lrn.All(char.IsDigit);
        }





    }
}
