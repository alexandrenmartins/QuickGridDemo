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

    public static List<Employee> GenerateEmployees(int count = 10_000, int startId = 1)
    {
        var faker = new Faker<Employee>("pt_BR")
            .RuleFor(e => e.FirstName, f => f.Person.FirstName)
            .RuleFor(e => e.LastName, f => f.Person.LastName)
            .RuleFor(e => e.Title, f => f.Name.JobTitle())
            .RuleFor(e => e.TitleOfCourtesy, f => f.PickRandom("Sr.", "Sra.", "Dr.", "Dra."))
            .RuleFor(e => e.BirthDate, f => f.Date.Past(50, DateTime.Today.AddYears(-22)).ToUniversalTime())
            .RuleFor(e => e.HireDate, f => f.Date.Past(10, DateTime.Today).ToUniversalTime())
            .RuleFor(e => e.Address, f => f.Address.StreetAddress())
            .RuleFor(e => e.City, f => f.Address.City())
            .RuleFor(e => e.Region, f => f.Address.State())
            .RuleFor(e => e.PostalCode, f => f.Address.ZipCode("#####-###"))
            .RuleFor(e => e.Country, f => f.Address.Country())
            .RuleFor(e => e.HomePhone, f => f.Phone.PhoneNumber("(##) #####-####"))
            .RuleFor(e => e.Extension, f => f.Random.AlphaNumeric(4).ToUpper())
            .RuleFor(e => e.Notes, f => f.Lorem.Sentence(f.Random.Int(5, 15)));

        var employees = faker.Generate(count);
        for (int i = 0; i < employees.Count; i++)
        {
            employees[i].EmployeeID = startId + i;
        }

        return employees;
    }
}
