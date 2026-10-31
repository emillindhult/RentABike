namespace RentABike.Models;

internal class Customer
{
    public Guid Id { get; }
    public string Name { get; }
    public int Age { get; }
    public bool IsMember { get; }

    public Customer(string name, int age, bool isMember)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;
        IsMember = isMember;
    }
}