using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContactManager.Domain.Entities;

namespace ContactManager.Domain.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Contact>> GetContactsAsync(int pageNumber, int pageSize, string sortBy, bool ascending, CancellationToken cancellationToken = default);
        
        Task UploadContactsAsync(IEnumerable<Contact> contacts, CancellationToken cancellationToken = default);
        
        Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken = default);
        
        Task DeleteContactAsync(int id, CancellationToken cancellationToken = default);
        
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    }
}
