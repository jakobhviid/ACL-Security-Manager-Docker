using System.ComponentModel.DataAnnotations;

namespace API.DTOs.inputDTOs
{
    public class NewSuperUserDTO
    {
        [Required]
        public string APIKey { get; set; }
        [Required]
        public string PrincipalName { get; set; }
        public string Host { get; set; }
    }
}