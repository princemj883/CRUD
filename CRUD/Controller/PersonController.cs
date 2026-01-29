using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUD.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService) : ControllerBase
{
    private readonly IPersonService _personService = personService;
    
    [HttpGet]
    public IActionResult GetAllPersons()
    {
        List<PersonResponse> persons = _personService.GetPersonsList();

        return Ok(persons);
    }

    [HttpGet("{personId:guid}")]
    public IActionResult GetPersonById(Guid personId)
    {
        PersonResponse? person =
            _personService.GetPersonByPersonId(personId);

        if (person == null)
            return NotFound();

        return Ok(person);
    }

    [HttpPost]
    public IActionResult AddPerson([FromBody] PersonAddRequest request)
    {
        if (request == null)
            return BadRequest("Person data is required");
        PersonResponse response = _personService.AddPerson(request);
        
        return CreatedAtAction(nameof(GetPersonById), new { personId = response.PersonId }, response);
    }
    
    [HttpGet("filter")]
    public IActionResult GetFilteredPersons([FromQuery] string searchBy, [FromQuery] string? searchString)
    {
        List<PersonResponse> persons =
            _personService.GetFilteredPersons(searchBy, searchString);

        return Ok(persons);
    }
    
    [HttpGet("sort")]
    public IActionResult GetSortedPersons([FromQuery] string sortBy, [FromQuery] SortOrderOptions sortOrder)
    {
        List<PersonResponse> allpersons =
            _personService.GetPersonsList();
        List<PersonResponse> sortedPersons = _personService.GetSortedPersons(allpersons, sortBy, sortOrder);

        return Ok(sortedPersons);
    }

    [HttpPut("{personId:guid}")]
    public IActionResult UpdatePerson(Guid personId, [FromBody] PersonUpdateRequest? request)
    {
        if(request == null)
            return BadRequest("Person data is required to update");
        
        request.PersonId = personId;
        
        PersonResponse updatedPerson = _personService.UpdatePerson(request);
        
        return Ok(updatedPerson);
    }
    
    [HttpDelete("{personId:guid}")]
    public IActionResult DeletePerson(Guid personId)
    {
        _personService.DeletePerson(personId);
        return NoContent();
    }
}
