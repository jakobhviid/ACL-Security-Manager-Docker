using System.ComponentModel.DataAnnotations;

namespace API.DTOs.inputDTOs
{
    public class UpdateAccessControlEntryDTO
    {

        [Required]
        public string APIKey { get; set; }
    }
}
