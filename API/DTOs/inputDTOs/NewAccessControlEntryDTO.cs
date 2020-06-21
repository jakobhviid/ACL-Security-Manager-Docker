using System.ComponentModel.DataAnnotations;

namespace API.DTOs.inputDTOs
{
    public class NewAccessControlEntryDTO
    {
        [Required]
        public string APIKey { get; set; }
    }
}
