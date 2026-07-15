using System.Collections.Generic;

namespace ContactManager.Application.DTOs
{
    public class CsvUploadResultDto
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool Success => Errors.Count == 0;
    }
}