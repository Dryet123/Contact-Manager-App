using ContactManager.Application.DTOs;
using ContactManager.Application.Services;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactRepository;
    private readonly IContactService _contactService;
    private readonly ICsvService _csvService;

    public ContactsController(
        IContactRepository contactRepository, 
        IContactService contactService, 
        ICsvService csvService)
    {
        _contactRepository = contactRepository;
        _contactService = contactService;
        _csvService = csvService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetContacts(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string sortBy = "Name", 
        [FromQuery] bool ascending = true,
        CancellationToken cancellationToken = default)
    {
        var contacts = await _contactRepository.GetContactsAsync(pageNumber, pageSize, sortBy, ascending, cancellationToken);
        var totalCount = await _contactRepository.GetTotalCountAsync(cancellationToken);

        
        return Ok(new { 
            data = contacts, 
            totalCount 
        });
    }

    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return BadRequest("Please upload a valid CSV file.");
        }

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Invalid file format. Only .csv files are allowed.");
        }

        
        var (records, errors) = await _csvService.ParseCsvFileAsync(file);

       
        if (records.Any())
        {
            await _contactService.AddContactsAsync(records, cancellationToken);
        }

       
        return Ok(new 
        { 
            InsertedCount = records.Count, 
            Errors = errors 
        });
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDto contactDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        
        var contactToUpdate = new Contact
        {
            Id = id,
            Name = contactDto.Name,
            DateOfBirth = contactDto.DateOfBirth,
            Married = contactDto.Married,
            Phone = contactDto.Phone,
            Salary = contactDto.Salary
        };

        await _contactRepository.UpdateContactAsync(contactToUpdate, cancellationToken);

        return NoContent(); 
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id, CancellationToken cancellationToken)
    {
        await _contactRepository.DeleteContactAsync(id, cancellationToken);
        return NoContent();
    }
}