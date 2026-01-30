using ServiceContracts.DTO;

namespace ServiceContracts;

public interface ICountriesService
{
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    
    /// <summary>
    /// Retruns all countries from the list 
    /// </summary>
    /// <returns>All Countries from the list as list of CountryResponse</returns>
    Task<List<CountryResponse>> GetAllCountries();
    
    Task<CountryResponse>? GetCountryByCountryId(Guid? countryId);
}