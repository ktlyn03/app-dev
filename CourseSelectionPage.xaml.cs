using Microsoft.Maui.Controls;
using System;

namespace StudentRegistrationSystem
{
    public partial class CourseSelectionPage : ContentPage
    {
        private readonly StudentDatabase _db;
        private readonly StudentDatabase.Student _student;
        private string _selectedCourse;

        // Constructor receives the student object
        public CourseSelectionPage(StudentDatabase.Student student)
        {
            InitializeComponent();
            _student = student;
            _db = new StudentDatabase("students.db");
        }

        // This will be triggered when any of the RadioButtons is checked or unchecked
        private void OnCourseCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && e.Value)  // If this radio button is checked
            {
                _selectedCourse = radioButton.Value.ToString();  // Store the selected course value
            }
        }

        // Handle the Submit button click
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            // Validate if a course has been selected
            if (string.IsNullOrEmpty(_selectedCourse))
            {
                await DisplayAlert("Error", "Please select a course.", "OK");
                return;
            }

            // Save the selected course to the student's information in the student object
            _student.Course = _selectedCourse;

            // Navigate to the Parent Information page, passing the student object
            var parentInfoPage = new ParentInformationPage(_student);
            await Navigation.PushAsync(parentInfoPage);  // Navigate to the Parent Information page
        }
    }
}
