namespace FootballManager;

public class Coach(string name, string nationality, int age)
{
    public string Name { get; set; } = name;
    public string Nationality { get; set; } = nationality;
    public int Age { get; set; } = age;
}