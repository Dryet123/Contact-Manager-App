using ContactManager.Application.DTOs;

namespace ContactManager.Application.Services;

public interface ICsvService
{
    Task<(List<ContactDto> Records, List<string> Errors)> ParseCsvFileAsync(IFormFile file);
}