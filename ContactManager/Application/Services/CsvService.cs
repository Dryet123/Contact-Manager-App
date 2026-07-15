using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ContactManager.Application.DTOs;
using CsvHelper;
using CsvHelper.Configuration;


namespace ContactManager.Application.Services;

public class CsvService : ICsvService
{

    public async Task<(List<ContactDto> Records, List<string> Errors)> ParseCsvFileAsync(IFormFile file)
    {
        var records = new List<ContactDto>();
        var errors = new List<string>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, config);

        await csv.ReadAsync();
        csv.ReadHeader();

        if (csv.Context?.Parser == null)
        {
            errors.Add("Failed to initialize CSV parser.");
            return (records, errors);
        }

        
        while (await csv.ReadAsync())
        {
            
            var currentRow = csv.Context.Parser.Row;

            try
            {
                var record = csv.GetRecord<ContactDto>();

                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(record, null, null);

                if (!Validator.TryValidateObject(record, validationContext, validationResults, true))
                {
                    var errorMessages = validationResults.Select(r => r.ErrorMessage);
                    errors.Add($"Invalid data in row {currentRow}: {string.Join(", ", errorMessages)}");
                    continue;
                }

                records.Add(record);
            }
            catch (CsvHelperException)
            {
                errors.Add($"Parse error in row {currentRow}: Incorrect data format for one of the fields.");
            }
            catch (Exception ex)
            {
                errors.Add($"Unexpected error in row {currentRow}: {ex.Message}");
            }
        }

       
        return (records, errors);
    }
    
}
