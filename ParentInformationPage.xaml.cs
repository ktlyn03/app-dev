using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;

namespace StudentRegistrationSystem
{
    public partial class ParentInformationPage : ContentPage
    {
        private readonly StudentDatabase _db; // Initialize the database reference
        private readonly StudentDatabase.Student _student;

        // Constructor receives the student object
        public ParentInformationPage(StudentDatabase.Student student)
        {
            InitializeComponent();

            if (student == null)
            {
                Debug.WriteLine("Student data is null");
            }

            _student = student;
            BindingContext = _student;  // Bind the student object to this page

            // Initialize the database reference here
            _db = new StudentDatabase("students.db");
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            // Validate if parent information fields are filled
            if (string.IsNullOrWhiteSpace(ParentNameEntry.Text) ||
                string.IsNullOrWhiteSpace(ParentAgeEntry.Text) ||
                string.IsNullOrWhiteSpace(ParentContactEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all parent information fields (Name, Age, Contact).", "OK");
                return;
            }

            // Validate Parent Age (must be an integer)
            if (!int.TryParse(ParentAgeEntry.Text, out var parentAge))
            {
                await DisplayAlert("Error", "Please enter a valid parent's age (numeric only).", "OK");
                return;
            }

            // Update the student model with the entered parent information
            _student.ParentName = ParentNameEntry.Text;
            _student.ParentAge = parentAge;  // Set parent age
            _student.ParentContact = ParentContactEntry.Text;

            // Ensure the student has an ID before updating
            if (_student.Id == 0)
            {
                // If the student ID is 0, it hasn't been saved yet, so insert instead of update
                await _db.InsertStudentAsync(_student);
                await DisplayAlert("Success", "Student registered and parent information added!", "OK");
                await Navigation.PopToRootAsync();  // Example: Go back to the main page
            }
            else
            {
                // Insert or update the student in the database
                var result = await _db.UpdateStudentAsync(_student);

                if (result > 0)
                {
                    await DisplayAlert("Success", "Student Registration Completed Successfully!", "OK");
                    // Navigate to a final confirmation page or back to the main page
                    await Navigation.PopToRootAsync();  // Example: Go back to the main page
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update parent information.", "OK");
                }
            }
        }

    }
}
