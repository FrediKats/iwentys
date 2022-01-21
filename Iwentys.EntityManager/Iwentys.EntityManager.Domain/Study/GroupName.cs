namespace Iwentys.EntityManager.Domain;

public class GroupName
{
    public GroupName(string name)
    {
        //FYI: russian letter
        Name = name
            .ToUpper()
            .Substring(0, 5)
            .Replace("М", "M")
            .Replace("м", "M")
            .Replace("m", "M");

        Course = int.Parse(Name.Substring(2, 1));
        Number = int.Parse(Name.Substring(3, 2));
    }

    public GroupName(int course, int number)
    {
        Course = course;
        Number = number;
        Name = $"M3{course}{number:00}";
    }

    public int Course { get; }
    public int Number { get; }
    public string Name { get; }
}