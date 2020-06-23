using System.ComponentModel.DataAnnotations;

namespace API.DTOs.inputDTOs
{
    public class ReadAccessControlEntryDTO
    {
        [Required]
        public string APIKey { get; set; }

        [Required]
        public string PrincipalName { get; set; }
    }
}
