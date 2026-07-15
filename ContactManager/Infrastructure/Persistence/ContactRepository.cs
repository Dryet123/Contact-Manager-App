using ContactManager.Domain.Entities;
using ContactManager.Domain.Interfaces;
using ContactManager.Infrastructure.Data;
using Dapper;

namespace ContactManager.Infrastructure.Persistence
{
    public class ContactRepository : IContactRepository
    {
        private readonly DapperContext _context;

        public ContactRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(int pageNumber, int pageSize, string sortBy, bool ascending, CancellationToken cancellationToken = default)
        {
            
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Name", "DateOfBirth", "Married", "Phone", "Salary"
            };

            if (string.IsNullOrWhiteSpace(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                sortBy = "Name"; 
            }

            var sortDirection = ascending ? "ASC" : "DESC";
            var offset = (pageNumber - 1) * pageSize;

            
            var query = $@"
                SELECT Id, Name, DateOfBirth, Married, Phone, Salary 
                FROM Contacts 
                ORDER BY {sortBy} {sortDirection} 
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var command = new CommandDefinition(
                query, 
                new { Offset = offset, PageSize = pageSize }, 
                cancellationToken: cancellationToken);

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Contact>(command);
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            var query = "SELECT COUNT(*) FROM Contacts";
            var command = new CommandDefinition(query, cancellationToken: cancellationToken);

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(command);
        }

        public async Task UploadContactsAsync(IEnumerable<Contact> contacts, CancellationToken cancellationToken = default)
        {
            var query = @"
                INSERT INTO Contacts (Name, DateOfBirth, Married, Phone, Salary) 
                VALUES (@Name, @DateOfBirth, @Married, @Phone, @Salary)";

            // Передаем коллекцию contacts в параметры. 
            // Dapper автоматически выполнит этот запрос для каждого элемента в списке.
            var command = new CommandDefinition(query, contacts, cancellationToken: cancellationToken);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(command);
        }

        public async Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken = default)
        {
            var query = @"
                UPDATE Contacts 
                SET Name = @Name, 
                    DateOfBirth = @DateOfBirth, 
                    Married = @Married, 
                    Phone = @Phone, 
                    Salary = @Salary 
                WHERE Id = @Id";

            var command = new CommandDefinition(query, contact, cancellationToken: cancellationToken);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(command);
        }

        public async Task DeleteContactAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = "DELETE FROM Contacts WHERE Id = @Id";
            var command = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(command);
        }
    }
}