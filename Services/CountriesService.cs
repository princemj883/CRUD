using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    // Private Field
    private readonly  PersonsDbContext _db;
    
    // Constructor
    public CountriesService(PersonsDbContext personsDbContext)
    {
        _db = personsDbContext;
    }
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
        if(await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
        {
            throw new ArgumentException("CountryName already exists");
        }
        Country country = countryAddRequest.ToCountry();
        country.CountryId = Guid.NewGuid();
        _db.Countries.Add(country);
        await _db.SaveChangesAsync();
        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
            return null;
        Country? countryResponse = await _db.Countries.FirstOrDefaultAsync(x => x.CountryId == countryId);
        if(countryResponse == null)
            return null;
        return countryResponse.ToCountryResponse();
    }
}