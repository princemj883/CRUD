using Entities;

namespace ServiceContracts.DTO;

public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetter { get; set; }
    public double? Age { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (obj.GetType() != typeof(PersonResponse))
            return false;
        PersonResponse personResponse = (PersonResponse)obj;
        return PersonId == personResponse.PersonId && PersonName == personResponse.PersonName && 
               Email == personResponse.Email && DateOfBirth == personResponse.DateOfBirth && Gender == personResponse.Gender &&
               CountryId == personResponse.CountryId && CountryName == personResponse.CountryName && 
               Address == personResponse.Address && ReceiveNewsLetter == personResponse.ReceiveNewsLetter;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
}
public static class PersonExtensions
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse()
        {
            PersonId = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            ReceiveNewsLetter = person.ReceiveNewsLetter,
            Address = person.Address,
            CountryId = person.CountryId,
            Gender = person.Gender,
            Age = (person.DateOfBirth != null)
                ? Math.Round((DateTime.Now - person.DateOfBirth).TotalDays / 365.25)
                : null
        };
    }
}