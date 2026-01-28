using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

public interface IPersonService
{
   PersonResponse AddPerson(PersonAddRequest personAddRequest);
   List<PersonResponse> GetPersonsList();
   
   /// <summary>
   /// Return the PersonResponse based on personId
   /// </summary>
   /// <param name="personId"></param>
   /// <returns>Returns Matching person object</returns>
   PersonResponse GetPersonByPersonId(Guid? personId);

   /// <summary>
   /// 
   /// </summary>
   /// <param name="searchBy"></param>
   /// <param name="searchString"></param>
   /// <returns></returns>
   List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
   
   /// <summary>
   /// 
   /// </summary>
   /// <param name="allpersons"></param>
   /// <param name="sortBy"></param>
   /// <param name="sortOrder"></param>
   /// <returns></returns>
   List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder);
}
