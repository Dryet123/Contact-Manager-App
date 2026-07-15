using ContactManager.Application.DTOs;

namespace ContactManager.Application.Services
{
    public interface IContactService
    {
        Task AddContactsAsync(List<ContactDto> contacts, CancellationToken cancellationToken = default);
    }
}