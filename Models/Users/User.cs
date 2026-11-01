namespace RentABike.Models.Users;

internal class User
{
    public Guid Id { get; }
    public string Name { get; }

    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
