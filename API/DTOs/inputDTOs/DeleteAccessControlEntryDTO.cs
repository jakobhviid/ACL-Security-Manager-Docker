using System.ComponentModel.DataAnnotations;

namespace API.DTOs.inputDTOs
{
    public class DeleteAccessControlEntryDTO
    {

        [Required]
        public string APIKey { get; set; }
    }
}
