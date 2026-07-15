using ContactManager.Application.DTOs;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ContactManager.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IContactRepository contactRepository, ILogger<ContactService> logger)
    {
        _contactRepository = contactRepository;
        _logger = logger;
    }

    public async Task AddContactsAsync(List<ContactDto> contacts, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Mapping {Count} contacts to domain entities.", contacts.Count);

        var contactEntities = contacts.Select(c => new Contact
        {
            Name = c.Name,
            DateOfBirth = c.DateOfBirth,
            Married = c.Married,
            Phone = c.Phone,
            Salary = c.Salary
        }).ToList();

        _logger.LogInformation("Uploading mapped contacts to the database.");
        
        await _contactRepository.UploadContactsAsync(contactEntities, cancellationToken);
    }
}