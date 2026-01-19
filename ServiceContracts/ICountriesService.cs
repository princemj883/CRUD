using ServiceContracts.DTO;

namespace ServiceContracts;

public interface ICountriesService
{
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
    
    /// <summary>
    /// Retruns all countries from the list 
    /// </summary>
    /// <returns>All Countries from the list as list of CountryResponse</returns>
    List<CountryResponse> GetAllCountries();
    
    CountryResponse? GetCountryByCountryId(Guid? countryId);
}