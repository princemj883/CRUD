using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonService : IPersonService
{
    //private Fields
    private readonly List<Person> _personList;
    private readonly ICountriesService _countriesService;
    
    //Constructor
    public PersonService()
    {
        _personList = new List<Person>();
        _countriesService = new CountriesService();
    }
    
    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
        return personResponse;
        
    }
    
    public PersonResponse AddPerson(PersonAddRequest personAddRequest)
    {
        //check if PersonAddRequest is not null
        if(personAddRequest == null)
            throw new ArgumentNullException(nameof(personAddRequest));
        
        //Model Validation
        ValidationHelper.ModelValidation(personAddRequest);
        
        //convert PersonAddRequest to Person entity
        Person person = personAddRequest.ToPerson();
        
        //Generate new Guid for PersonId
        person.PersonId = Guid.NewGuid();
        
        //add the person to the list
        _personList.Add(person);
        
        //convert the Person object into PersonResponse type
        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetPersonsList()
    {
        return _personList.Select(x => x.ToPersonResponse()).ToList();
    }

    public PersonResponse GetPersonByPersonId(Guid? personId)
    {
        if(personId == null)
            return null;
        
        Person? person = _personList.FirstOrDefault(x => x.PersonId == personId);
        if(person == null)
            return null;
        return person.ToPersonResponse();
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetPersonsList();
        List<PersonResponse> matchingPerson = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPerson;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPerson = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.PersonName)
                        ? x.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                        : true)).ToList();
                break;
            
            case nameof(Person.Email):
                matchingPerson = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Email)
                        ? x.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                        : true)).ToList();
                break;
            
            case nameof(Person.DateOfBirth):
                matchingPerson = allPersons.Where(x =>
                    (x.DateOfBirth != null)
                       ? x.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)
                       : true).ToList();
                break;
            
            case nameof(Person.Gender):
                matchingPerson = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Gender)
                        ? x.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                        : true)).ToList();
                break;
            
            case nameof(Person.Address):
                matchingPerson = allPersons.Where(x =>
                    (!string.IsNullOrEmpty(x.Address)
                        ? x.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                        : true)).ToList();
                break;
            
            default: matchingPerson = allPersons; break;
        }   
        
        return matchingPerson;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
            return allpersons;

        List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC)
                    => allpersons.OrderBy(x => x.ReceiveNewsLetter).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC)
                    => allpersons.OrderByDescending(x => x.ReceiveNewsLetter).ToList(),

                _ => allpersons
            };
        return sortedPersons;
    }
}