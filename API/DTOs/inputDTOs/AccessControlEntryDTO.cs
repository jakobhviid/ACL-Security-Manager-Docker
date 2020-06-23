using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static API.DTOs.inputDTOs.ACLCommon;

namespace API.DTOs.inputDTOs
{
    public class AccessControlEntryDTO
    {

        [Required]
        public string PrincipalName { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        public PatternType PatternType { get; set; }

        [Required]
        public string ResourceName { get; set; }

        [Required]
        public OperationType Operation { get; set; }

        [Required]
        public PermissionType PermissionType { get; set; }

        [Required]
        public string Host { get; set; }

        public override string ToString()
        {
            return $"{PrincipalName},{ResourceType},{PatternType},{ResourceName},{Operation},{PermissionType},{Host}";
        }
        public bool EqualsCsvEntry(ACLEntryDefinition comparison)
        {
            return this.PrincipalName.Equals(comparison.KafkaPrincipal) &&
                this.ResourceType.ToString().Equals(comparison.ResourceType) &&
                this.PatternType.ToString().Equals(comparison.PatternType) &&
                this.ResourceName.Equals(comparison.ResourceName) &&
                this.Operation.ToString().Equals(comparison.Operation) &&
                this.PermissionType.ToString().Equals(comparison.PermissionType) &&
                this.Host.Equals(comparison.Host);
        }
    }
}
