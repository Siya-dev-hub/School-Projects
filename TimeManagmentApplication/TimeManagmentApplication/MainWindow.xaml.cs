using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Security.Cryptography;
using TimeManagementApp;

namespace TimeManagmentApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyDbContext dbContext;

        private Semester semester;

        private List<User> users = new List<User>();

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new MyDbContext(); // Initialize the instance of MyDbContext
            semester = new Semester();
            semester.Modules = new List<Module>();
        }


        // Calculate and display remaining self-study hours for all modules
        private void CalculateAndDisplayRemainingSelfStudyHours()
        {
            RemainingSelfStudyHoursListBox.Items.Clear();

            foreach (Module module in modules)
            {
                // Calculate total self-study hours for a module
                int totalSelfStudyHours = ((module.Credits * 10) / semester.NumberOfWeeks) - module.ClassHoursPerWeek;
                // Calculate the recorded hours for the module within the current week
                int recordedHours = module.StudyRecords
                    .Where(record => record.Date >= DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek))
                    .Sum(record => record.Hours);
                // Calculate remaining self-study hours for the module
                int remainingHours = totalSelfStudyHours - recordedHours;
                // Add module details and remaining hours to the list box
                RemainingSelfStudyHoursListBox.Items.Add($"Module: {module.Name}, Remaining Hours: {remainingHours}");
            }
        }


        

        
        private List<Module> modules = new List<Module>(); // Collection to store modules

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;

            // Perform user authentication here, e.g., check against your user database
            if (AuthenticateUser(username, password))
            {
                // Authentication successful, open the main application window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Close the current login window
                this.Close();
            }
            else
            {
                // Authentication failed, show an error message
                MessageBox.Show("Login failed. Please check your credentials.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (var context = new MyDbContext())
            {
                // Find a user with the provided username in the database
                User user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    // Hash the provided password using the salt from the user record
                    byte[] hashedPassword = HashPassword(password, user.PasswordSalt);

                    // Compare the hashed password with the one stored in the database
                    if (hashedPassword.SequenceEqual(user.PasswordHash))
                    {
                        // Authentication successful
                        return true;
                    }
                }
            }

            // Authentication failed
            return false;
        }








        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // Generate a random salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                // Hash the password securely using the salt
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
                {
                    byte[] hashedPassword = pbkdf2.GetBytes(20); // 20 is the size of the hashed password
                    User newUser = new User(username, hashedPassword, salt);

                    // Store the user data securely (e.g., in a database)
                    users.Add(newUser);

                    // Inform the user that registration is successful
                    MessageBox.Show("Registration successful!");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid username and password.");
            }
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(20); // 20 is the size of the hashed password
            }
        }







        // Event handler for adding a module
        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve user input from text boxes
                string code = ModuleCodeTextBox.Text;
                string name = ModuleNameTextBox.Text;
                int credits = int.Parse(ModuleCreditsTextBox.Text);
                int classHoursPerWeek = int.Parse(ModuleClassHoursTextBox.Text);

                if (modules.Any(existingModule => existingModule.Code == code || existingModule.Name == name))
                {
                    MessageBox.Show("A module with the same code or name already exists.");
                    return; // Exit the method if the module is a duplicate
                }

                // Create a new Module instance
                Module module = new Module
                {
                    Code = code,
                    Name = name,
                    Credits = credits,
                    ClassHoursPerWeek = classHoursPerWeek
                };
                
                // Calculate and set self-study hours for the module
                module.SelfStudyHoursPerWeek = ((credits * 10) / semester.NumberOfWeeks) - classHoursPerWeek;

                // Add the module to the collection
                modules.Add(module);

                // Clear the input fields
                ModuleCodeTextBox.Clear();
                ModuleNameTextBox.Clear();
                ModuleCreditsTextBox.Clear();
                ModuleClassHoursTextBox.Clear();

                // Update the user interface to display the added module
                UpdateModuleListView();
                // Calculate and display remaining self-study hours for all modules
                CalculateAndDisplayRemainingSelfStudyHours();

            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for Credits and Class Hours per Week.");
            }
        }

        // Update the module list view with the current module collection
        private void UpdateModuleListView()
        {
            // Clear the current items in the ListView
            ModuleListView.Items.Clear();

            // Add the modules to the ListView
            foreach (Module module in modules)
            {
                ModuleListView.Items.Add(module);
            }
        }

        // Event handler for saving semester information
        private void SaveSemesterInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve user input
                int numberOfWeeks = int.Parse(NumberOfWeeksTextBox.Text);
                DateTime startDate = StartDateDatePicker.SelectedDate ?? DateTime.Now;

                // Validate the start date
                if (startDate < DateTime.Today)
                {
                    MessageBox.Show("Start date cannot be in the past.");
                    return; // Exit the method if the date is invalid
                }

                // Store semester information in your Semester object (you should have created it earlier)
                semester.NumberOfWeeks = numberOfWeeks;
                semester.StartDate = startDate;

                // Recalculate SelfStudyHoursPerWeek for all modules
                semester.CalculateSelfStudyHours();

                // UI to confirm that semester information is saved
                MessageBox.Show("Semester information saved successfully.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number of weeks.");
            }
        }


        // Event handler for recording study hours
        private void RecordStudyHours_Click(object sender, RoutedEventArgs e)
        {
            if (ModuleListView.SelectedItem == null)
            {
                MessageBox.Show("Please select a module before recording study hours.");
                return; // Exit the method if no module is selected
            }

            // Check if the user entered valid hours
            if (int.TryParse(StudyHoursTextBox.Text, out int hours))
            {
                if (hours < 0)
                {
                    MessageBox.Show("Study hours cannot be negative.");
                    return; // Exit the method if the hours are negative
                }

                Module selectedModule = (Module)ModuleListView.SelectedItem;

                // Get the selected date from the DatePicker
                DateTime selectedDate = StudyDatePicker.SelectedDate ?? DateTime.Now;

                // Create a new study record and add it to the selected module
                StudyRecord studyRecord = new StudyRecord
                {
                    Date = selectedDate,
                    Hours = hours,
                };

                selectedModule.StudyRecords.Add(studyRecord);

                // Update the module list view to reflect the study record
                ModuleListView.Items.Refresh();

                // Calculate and display remaining self-study hours for all modules
                CalculateAndDisplayRemainingSelfStudyHours();
            }
            else
            {
                // Handle invalid input (e.g., display an error message to the user)
                MessageBox.Show("Please enter a valid number of hours.");
            }
        }



        // Event handler for textbox got focus to clear placeholder text
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter the number of weeks" || textBox.Text == "Select Start Date" ||
                textBox.Text == "Module Code" || textBox.Text == "Module Name" ||
                textBox.Text == "Number of Credits" || textBox.Text == "Class Hours per Week" ||
                textBox.Text == "Filter by Module Code")
            {
                textBox.Text = string.Empty; // Clear placeholder text
            }
        }

        // Event handler for textbox lost focus to restore placeholder text
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "NumberOfWeeksTextBox")
                {
                    textBox.Text = "Enter the number of weeks";
                }
                else if (textBox.Name == "StartDateDatePicker")
                {
                    textBox.Text = "Select Start Date";
                }
                else if (textBox.Name == "ModuleCodeTextBox")
                {
                    textBox.Text = "Module Code";
                }
                else if (textBox.Name == "ModuleNameTextBox")
                {
                    textBox.Text = "Module Name";
                }
                else if (textBox.Name == "ModuleCreditsTextBox")
                {
                    textBox.Text = "Number of Credits";
                }
                else if (textBox.Name == "ModuleClassHoursTextBox")
                {
                    textBox.Text = "Class Hours per Week";
                }
                
            }
        }

        // Event handler for DatePicker got focus to clear placeholder text
        private void DatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            if (datePicker.SelectedDate == null)
            {
                // When the DatePicker receives focus and no date is selected, clear the DatePicker.
                datePicker.SelectedDate = DateTime.Now;
            }
        }

        // Event handler for DatePicker lost focus to restore placeholder text
        private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            if (datePicker.SelectedDate == null)
            {
                // When the DatePicker loses focus and no date is selected, restore a placeholder text.
                datePicker.Text = "Select Start Date";
            }
        }
        // Event handler for module selection change
        private void ModuleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if any items were selected
            if (e.AddedItems.Count > 0)
            {
                // Access the selected items
                Module selectedModule = (Module)e.AddedItems[0]; // Assuming single selection

                
                MessageBox.Show($"Selected Module: {selectedModule.Name}");

                // Calculate and display remaining self-study hours for all modules
                CalculateAndDisplayRemainingSelfStudyHours();
            }
        }



        // Event handler for textbox click to clear placeholder text
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholderText = GetPlaceholderText(textBox.Name);
                if (string.Equals(textBox.Text.Trim(), placeholderText, StringComparison.OrdinalIgnoreCase))
                {
                    textBox.Clear();
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                string placeholderText = GetPlaceholderText(passwordBox.Name);
                if (string.Equals(passwordBox.Password.Trim(), placeholderText, StringComparison.OrdinalIgnoreCase))
                {
                    passwordBox.Clear();
                }
            }
        }


        private string GetPlaceholderText(string controlName)
        {
            switch (controlName)
            {
                case "NumberOfWeeksTextBox":
                    return "Enter the number of weeks";
                case "ModuleCodeTextBox":
                    return "Module Code";
                case "ModuleNameTextBox":
                    return "Module Name";
                case "ModuleCreditsTextBox":
                    return "Number of Credits";
                case "ModuleClassHoursTextBox":
                    return "Class Hours per Week";
                case "StudyHoursTextBox":
                    return "Enter Study Hours";
                case "UsernameTextBox":
                    return "Enter Username";

                // Add cases for PasswordBox if needed
                default:
                    return string.Empty;
            }
        }





    }
}
