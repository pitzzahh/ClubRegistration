using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace ClubRegistration;

public class ClubRegistrationQuery
{
    private SqlConnection? SqlConnection { get; } =
        new("Server=localhost\\SQLEXPRESS;Database=ClubDB;Integrated Security=True;TrustServerCertificate=True");

    private SqlCommand? SqlCommand { get; set; }
    public ObservableCollection<Student> Students { get; } = new();

    public void ReadStudents()
    {
        SqlCommand = new SqlCommand("SELECT * FROM ClubMembers", SqlConnection);
        if (SqlConnection == null)
        {
            return;
        }

        SqlConnection.Open();
        using var reader = SqlCommand.ExecuteReader();
        if (reader.HasRows)
        {
            Students.Clear();
        }

        while (reader.Read())
        {
            Students.Add(new Student
            {
                Id = reader.GetInt32("ID"),
                StudentId = reader.GetInt64("StudentId"),
                FirstName = reader.GetString("FirstName"),
                MiddleName = reader.IsDBNull("MiddleName")
                    ? null
                    : reader.GetString("MiddleName"),
                LastName = reader.GetString("LastName"),
                Age = reader.GetInt32("Age"),
                Gender = reader.GetString("Gender"),
                Program = reader.GetString("Program")
            });
        }

        SqlConnection.Close();
    }

    public bool Process(Command command, Student student)
    {
        if (SqlConnection == null)
        {
            return false;
        }

        var sql = command switch
        {
            Command.Create =>
                "INSERT INTO ClubMembers VALUES (@StudentId, @FirstName, @MiddleName, @LastName, @Age, @Gender, @Program)",
            Command.Update => "UPDATE ClubMembers SET " + "FirstName = @FirstName, " + "MiddleName = @MiddleName, " +
                              "LastName = @LastName, " + "Age = @Age, " + "Gender = @Gender, " + "Program = @Program " +
                              "WHERE StudentId = @StudentId;",
            _ => throw new InvalidEnumArgumentException("Enum value not supported")
        };

        SqlCommand = new SqlCommand(sql, SqlConnection);

        SqlCommand.Parameters.Add("@StudentId", SqlDbType.BigInt).Value = student.StudentId;
        SqlCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = student.FirstName;
        SqlCommand.Parameters.Add("@MiddleName", SqlDbType.VarChar).Value = student.MiddleName;
        SqlCommand.Parameters.Add("@LastName", SqlDbType.VarChar).Value = student.LastName;
        SqlCommand.Parameters.Add("@Age", SqlDbType.Int).Value = student.Age;
        SqlCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = student.Gender;
        SqlCommand.Parameters.Add("@Program", SqlDbType.VarChar).Value = student.Program;

        try
        {
            if (SqlConnection.State == ConnectionState.Closed)
            {
                SqlCommand.Connection.Open();
            }

            return SqlCommand.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        finally
        {
            SqlConnection.Close();
        }
    }

    public enum Command
    {
        Create,
        Update
    }
}
