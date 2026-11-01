namespace RentABike.Models.Users;

internal class Customer : User
{
    public int Age { get; }
    public bool IsMember { get; }

    public Customer(string name, int age, bool isMember) : base(name)
    {
        Age = age;
        IsMember = isMember;
    }
}