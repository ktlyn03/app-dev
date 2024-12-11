using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentRegistrationSystem
{
    public partial class AdminPage : ContentPage
    {
        private readonly StudentDatabase _studentDatabase;

        public AdminPage()
        {
            InitializeComponent();
            _studentDatabase = new StudentDatabase("students.db");

            // Load student data into ListView
            LoadStudents();
        }

        private async void LoadStudents()
        {
            try
            {
                // Fetch list of students from the database
                List<StudentDatabase.Student> students = await _studentDatabase.GetStudentsAsync();

                // Check if the ListView is still available before updating it
                if (StudentListView != null && StudentListView.IsVisible)
                {
                    // Debugging: Check if data is retrieved
                    Console.WriteLine($"Number of students retrieved: {students.Count}");

                    // Set the ListView's ItemsSource to the list of students
                    StudentListView.ItemsSource = students;
                }
            }
            catch (Exception ex)
            {
                // Log or show an error if fetching students fails
                Console.WriteLine($"Error loading students: {ex.Message}");
            }
        }





        // Edit button clicked handler
        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int studentId = (int)button.CommandParameter;

            // Find the student by ID and navigate to the Edit page
            var student = await _studentDatabase.GetStudentAsync(studentId);
            if (student != null)
            {
                // Navigate to Edit page with selected student details
                await Navigation.PushAsync(new EditStudentPage(student));
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int studentId = (int)button.CommandParameter;

            // Find the student by ID
            var student = await _studentDatabase.GetStudentAsync(studentId);
            if (student != null)
            {
                // First, remove the student from the database
                await _studentDatabase.DeleteStudentAsync(student);

                // Make sure the ListView gets updated on the main thread, 
                // but ensure we're still on the main page and the ListView exists.
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (StudentListView != null && StudentListView.IsVisible)
                    {
                        // Reload the student list after deletion
                        LoadStudents();
                    }
                });
            }
        }




    }
}
