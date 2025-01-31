namespace consoleshoppen.MongoDB;

public class Customer
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string ZipCode { get; set; }
    public required string Country { get; set; }
}
