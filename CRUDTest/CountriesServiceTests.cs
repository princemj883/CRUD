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

    #region AddCountry Tests
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
    
    //When the CountryName is duplicate, AddCountry should throw ArgumentException
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
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = "Japan"
        };
        
        //Act
        CountryResponse response = _countriesService.AddCountry(request);
        List<CountryResponse> countriesFromGetAllCountries = _countriesService.GetAllCountries();
        
        //Assert
        Assert.True(response.CountryId != Guid.Empty);
        Assert.Contains(response, countriesFromGetAllCountries);
    }
    
    #endregion

    #region GetAllCountries
    //List of countries should be empty by default
    [Fact]
    public void GetAllCountries_EmptyList()
    {
        //Act 
        List<CountryResponse> actualCountriesResponseList = _countriesService.GetAllCountries();
        
        //Assert
        Assert.Empty(actualCountriesResponseList);
        
    }

    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        //Arrange
        List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>()
        {
            new CountryAddRequest(){ CountryName = "India"},
            new CountryAddRequest(){ CountryName = "USA"},
            new CountryAddRequest(){ CountryName = "UK"}
        };
        //Act
        List<CountryResponse> actualCountriesReponseList = new List<CountryResponse>();
        foreach (CountryAddRequest countryAddRequest in countryAddRequests)
        {
            actualCountriesReponseList.Add(_countriesService.AddCountry(countryAddRequest));
        }
        //read each element in actualCountriesReponseList
        foreach (CountryResponse expectedCountryResponse in actualCountriesReponseList)
        {
            Assert.Contains(expectedCountryResponse, actualCountriesReponseList);
        }
    }
    #endregion
}