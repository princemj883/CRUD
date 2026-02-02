using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUD.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService, IPersonsPdfGenerator pdfGenerator) : ControllerBase
{
    private readonly IPersonService _personService = personService;
    private readonly IPersonsPdfGenerator _pdfGenerator = pdfGenerator;
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        List<PersonResponse> persons = await _personService.GetPersonsList();

        return Ok(persons);
    }

    [HttpGet("{personId:guid}")]
    public async Task<IActionResult> GetPersonById(Guid personId)
    {
        PersonResponse? person =await _personService.GetPersonByPersonId(personId);

        return Ok(person);
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] PersonAddRequest request)
    {
        if (request == null)
            return BadRequest("Person data is required");
        PersonResponse response = await _personService.AddPerson(request);
        
        return CreatedAtAction(nameof(GetPersonById), new { personId = response.PersonId }, response);
    }
    
    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredPersons([FromQuery] string searchBy, [FromQuery] string? searchString)
    {
        List<PersonResponse> persons = await _personService.GetFilteredPersons(searchBy, searchString);

        return Ok(persons);
    }
    
    [HttpGet("sort")]
    public async Task<IActionResult> GetSortedPersons([FromQuery] string sortBy, [FromQuery] SortOrderOptions sortOrder)
    {
        List<PersonResponse> allpersons = await _personService.GetPersonsList();
        List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(allpersons, sortBy, sortOrder);

        return Ok(sortedPersons);
    }

    [HttpPut("{personId:guid}")]
    public async Task<IActionResult> UpdatePerson(Guid personId, [FromBody] PersonUpdateRequest? request)
    {
        if(request == null)
            return BadRequest("Person data is required to update");
        
        request.PersonId = personId;
        
        PersonResponse updatedPerson = await _personService.UpdatePerson(request);
        
        return Ok(updatedPerson);
    }
    
    [HttpDelete("{personId:guid}")]
    public IActionResult DeletePerson(Guid personId)
    {
        _personService.DeletePerson(personId);
        return NoContent();
    }
    
    [HttpGet("pdf")]
    public async Task<IActionResult> PersonsPdf()
    {
        var persons = await _personService.GetPersonsList();

        byte[] pdf = _pdfGenerator.GeneratePersonsPdf(persons);

        return File(pdf, "application/pdf", "Persons.pdf");
    }

}
