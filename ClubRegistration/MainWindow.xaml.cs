using System;
using System.Windows;

namespace ClubRegistration;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private ClubRegistrationQuery ClubRegistrationQuery { get; }

    public MainWindow()
    {
        InitializeComponent();
        ClubRegistrationQuery = new ClubRegistrationQuery();
        Programs.Items.Add("Bachelor of Science in Accounting Information Systems");
        Programs.Items.Add("Bachelor of Science in Business Administration");
        Programs.Items.Add("Bachelor of Science in Computer Engineering");
        Programs.Items.Add("Bachelor of Science in Hospitality Management");
        Programs.Items.Add("Bachelor of Science in Information Technology");

        Gender.Items.Add("Male");
        Gender.Items.Add("Female");
        StudentsDataGrid.ItemsSource = ClubRegistrationQuery.Students;
        ClubRegistrationQuery.ReadStudents();
    }

    private void OnRefresh(object sender, RoutedEventArgs e)
    {
        ClubRegistrationQuery.ReadStudents();
    }

    private void OnRegister(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!StudentId.Text.IsNumeric())
            {
                MessageBox.Show("Student ID must be numeric", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Age.Text.IsNumeric())
            {
                MessageBox.Show("Age must be numeric", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var student = new Student
            {
                StudentId = long.Parse(StudentId.Text),
                FirstName = FirstName.Text,
                MiddleName = MiddleName.Text,
                LastName = LastName.Text,
                Age = int.Parse(Age.Text),
                Gender = Gender.Text,
                Program = Programs.Text
            };
            var result = ClubRegistrationQuery.Process(ClubRegistrationQuery.Command.Create, student);
            MessageBox.Show(result ? "Student registered successfully" : "Student registration failed");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnUpdate(object sender, RoutedEventArgs e)
    {
        var updateMember = new UpdateMember();
        updateMember.ShowDialog();
    }
}