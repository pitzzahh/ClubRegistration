namespace ClubRegistration;

public class Student
{
    public int Id { get; init; }
    public long StudentId { get; init; }
    public string FirstName { get; init; } = null!;
    public string? MiddleName { get; init; }
    public string LastName { get; init; } = null!;
    public int Age { get; init; }
    public string Gender { get; init; } = null!;
    public string Program { get; init; } = null!;
  
    public override string ToString()
    {
        return $"StudentId: {StudentId}, Name: {FirstName} {MiddleName ?? ""} {LastName}, Age: {Age}, Gender: {Gender}, Program: {Program}";
    }
}