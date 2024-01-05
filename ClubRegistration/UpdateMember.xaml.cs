using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClubRegistration;

public partial class UpdateMember
{
    private ClubRegistrationQuery ClubRegistrationQuery { get; }

    public UpdateMember()
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
        ClubRegistrationQuery.ReadStudents();
        ClubRegistrationQuery.Students
            .ToList()
            .ForEach(student => StudentId.Items.Add(student.StudentId));
    }

    private void OnConfirm(object sender, RoutedEventArgs e)
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
                StudentId = StudentId.Text != StudentId.SelectedValue.ToString()!
                    ? long.Parse(StudentId.Text)
                    : long.Parse(StudentId.SelectedValue.ToString()!),
                FirstName = FirstName.Text,
                MiddleName = MiddleName.Text,
                LastName = LastName.Text,
                Age = int.Parse(Age.Text),
                Gender = Gender.Text,
                Program = Programs.Text
            };
            var result = ClubRegistrationQuery.Process(ClubRegistrationQuery.Command.Update, student);
            MessageBox.Show(result ? "Student updated successfully" : "Student update failed");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnSelectId(object sender, SelectionChangedEventArgs e)
    {
        var student = ClubRegistrationQuery.Students 
            .First(student => student.StudentId == long.Parse(StudentId.SelectedValue.ToString()!));
        LastName.Text = student.LastName;
        FirstName.Text = student.FirstName;
        MiddleName.Text = student.MiddleName;
        Age.Text = student.Age.ToString();
        Gender.SelectedValue = student.Gender;
        Programs.SelectedValue = student.Program;
    }
}