using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
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
        throw new NotImplementedException();
    }
}