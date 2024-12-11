namespace StudentRegistrationSystem;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    // Change to void so that it matches the expected event handler signature
    private async void OnLoginButtonClickedAsync(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        // Default credentials
        if (username == "admin" && password == "admin")
        {
            // Successful login - Redirect to another page (you can navigate or show a success message)
            ErrorMessageLabel.IsVisible = false;
            await DisplayAlert("Login Success", "Welcome, admin!", "OK");
            await Navigation.PushAsync(new AdminPage());
        }
        else
        {
            // Login failure
            ErrorMessageLabel.Text = "Invalid username or password.";
            ErrorMessageLabel.IsVisible = true;
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        // Navigate to the MainPage when Register button is clicked
        await Navigation.PushAsync(new MainPage());
    }
}
