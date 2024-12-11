using Microsoft.Maui.Controls;
using StudentRegistrationSystem;

namespace StudentRegistrationSystem
{
    public partial class EditStudentPage : ContentPage
    {
        private readonly StudentDatabase _studentDatabase;
        private readonly StudentDatabase.Student _student;

        public EditStudentPage(StudentDatabase.Student student)
        {
            InitializeComponent();
            _studentDatabase = new StudentDatabase("students.db");
            _student = student;
            BindingContext = _student;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var updatedStudent = (StudentDatabase.Student)BindingContext;
            await _studentDatabase.UpdateStudentAsync(updatedStudent);
            await Navigation.PopAsync();  // Go back to the previous page
        }
    }
}
