using Bogus;
using QuickGridDemo.Models;

namespace QuickGridDemo.Data;

public static class SeedData
{
    public static List<Customer> GenerateCustomers(int count = 10_000, int startId = 1)
    {
        var faker = new Faker<Customer>("pt_BR")
            .RuleFor(c => c.CustomerID, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
            .RuleFor(c => c.ContactName, f => f.Person.FullName)
            .RuleFor(c => c.ContactTitle, f => f.Name.JobTitle())
            .RuleFor(c => c.Address, f => f.Address.StreetAddress())
            .RuleFor(c => c.City, f => f.Address.City())
            .RuleFor(c => c.Region, f => f.Address.State())
            .RuleFor(c => c.PostalCode, f => f.Address.ZipCode("#####"))
            .RuleFor(c => c.Country, f => f.Address.Country())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("(##) ####-####"))
            .RuleFor(c => c.Fax, f => f.Phone.PhoneNumber("(##) ####-####"));

        var customers = faker.Generate(count);
        for (int i = 0; i < customers.Count; i++)
        {
            customers[i].CustomerID = (startId + i).ToString("D5");
        }

        return customers;
    }
}
