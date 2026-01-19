using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest;

public class CountriesServiceTests
{
    private readonly ICountriesService _countriesService;
    
    public CountriesServiceTests()
    {
        _countriesService = new CountriesService();
    }
    // When CountryAddRequest is null, AddCountry should throw ArgumentNullException
    [Fact]
    public void AddCountry_CountryAddRequestIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        CountryAddRequest request = null;
        
        //Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //Act
             _countriesService.AddCountry(request);
        });
    }
    
    //When CountryName is null or empty, AddCountry should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = null
        };
        
        //Assert
        Assert.Throws<ArgumentException>(() =>        
        {
            //Act
            _countriesService.AddCountry(request);
        });
    }
    
    //When the CountryName is duplicate, AddCountry should throw ArugmentException
    [Fact]
    public void AddCountry_DuplicateCountryName_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest request1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        CountryAddRequest request2 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        
        //Assert
        Assert.Throws<ArgumentException>(() =>        
        {
            //Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
        });
    }
    
    //When  you supply valid CountryName, it should insert(add) the country to the existing list of countries
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        // Arrange
        CountryAddRequest request1 = new CountryAddRequest()
        {
            CountryName = "Japan"
        };
        
        //Act
        CountryResponse response = _countriesService.AddCountry(request1);
        
        //Assert
        Assert.True(response.CountryId != Guid.Empty);
    }
}