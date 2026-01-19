using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    // Private Field
    private readonly  List<Country> _countries;
    
    // Constructor
    public CountriesService()
    {
        _countries = new List<Country>();
    }
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        //Validation: countryAddRequest should not be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }
        
        // Validation: CountryName should not be null or empty
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }
        // Validation: CountryName cannot be duplicate
        if(_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
        {
            throw new ArgumentException("CountryName already exists");
        }
        Country country = countryAddRequest.ToCountry();
        country.CountryId = Guid.NewGuid();
        _countries.Add(country);
        return country.ToCountryResponse();
    }
}