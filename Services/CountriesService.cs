using Entities;
using Microsoft.AspNetCore.Http;
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

    public async Task<ExcelUploadResponse> UploadCountriesFromExcelFile(IFormFile fromFile)
    {
        if(fromFile == null || fromFile.Length == 0)
            throw new ArgumentException("File is required");
        using var memoryStream = new MemoryStream();
        await fromFile.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        using var workbook = new ClosedXML.Excel.XLWorkbook(memoryStream);
        var worksheet = workbook.Worksheets.First();
        var rows = worksheet.RowsUsed().Skip(1); // Skip header row
        
        var existingCountryNames = await _db.Countries
            .Select(c => c.CountryName!.ToLower())
            .ToListAsync();
        
        var countries = new List<Country>();
        int duplicateCount = 0;

        foreach (var row in rows)
        {
            var countryName = row.Cell(1).GetString().Trim();
            if(string.IsNullOrEmpty(countryName))
                continue;
            //Skip duplicates (Excel + DB)
            if (existingCountryNames.Contains(countryName.ToLower()))
            {
                duplicateCount++;
                continue;
            }

            // Prevent duplicates within same Excel file
            if (countries.Any(c =>
                    c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase)))
            {
                duplicateCount++;
                continue;
            }

            countries.Add(new Country
            {
                CountryId = Guid.NewGuid(),
                CountryName = countryName
            });
        }

        if (countries.Any())
        {
            _db.Countries.AddRange(countries);
            await _db.SaveChangesAsync();
        }
        return new ExcelUploadResponse
        {
            InsertedCount = countries.Count,
            DuplicateCount = duplicateCount
        };
    }
}