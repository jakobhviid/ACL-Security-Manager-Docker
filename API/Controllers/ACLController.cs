using System.Threading.Tasks;
using API.Contracts;
using API.DTOs.inputDTOs;
using API.DTOs.outputDTOs;
using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static API.DTOs.inputDTOs.ACLCommon;

namespace API.Controllers
{
    [ApiController]
    public class ACLController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ACLController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost(ApiRoutes.CreateNewSuperUser)]
        public ActionResult NewSuperUser(NewSuperUserDTO input)
        {
            // Check that the API KEY is correct to the one supplied in environment files
            if (!ValidAPIKey(input.APIKey))
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.AdminPasswordInCorrect });

            // TODO: Check if all of theses entries are needed to create a super user
            ACLHelper.AddEntry(new AccessControlEntryDTO
            {
                PrincipalName = input.PrincipalName,
                    ResourceType = ResourceType.Topic,
                    PatternType = PatternType.Literal,
                    ResourceName = "_schemas",
                    Operation = OperationType.All,
                    PermissionType = PermissionType.Allow,
                    Host = input.Host
            });
            ACLHelper.AddEntry(new AccessControlEntryDTO
            {
                PrincipalName = input.PrincipalName,
                    ResourceType = ResourceType.Group,
                    PatternType = PatternType.Literal,
                    ResourceName = "schema-registry",
                    Operation = OperationType.All,
                    PermissionType = PermissionType.Allow,
                    Host = input.Host
            });
            ACLHelper.AddEntry(new AccessControlEntryDTO
            {
                PrincipalName = input.PrincipalName,
                    ResourceType = ResourceType.Topic,
                    PatternType = PatternType.Literal,
                    ResourceName = "*",
                    Operation = OperationType.Describe,
                    PermissionType = PermissionType.Allow,
                    Host = input.Host
            });
            ACLHelper.AddEntry(new AccessControlEntryDTO
            {
                PrincipalName = input.PrincipalName,
                    ResourceType = ResourceType.Topic,
                    PatternType = PatternType.Literal,
                    ResourceName = "*",
                    Operation = OperationType.All,
                    PermissionType = PermissionType.Allow,
                    Host = input.Host
            });

            return StatusCode(StatusCodes.Status201Created, new GenericReturnMessageDTO
            {
                Status = 201,
                    Message = SuccessMessages.SuperUserCreated
            });
        }

        [HttpPost(ApiRoutes.CreateAccessControlEntry)]
        public ActionResult CreateAccessControlEntry(NewAccessControlEntryDTO input)
        {
            // Check that admin password is correct to the one supplied in environment files
            if (!ValidAPIKey(input.APIKey))
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.AdminPasswordInCorrect });

            // TODO: Create new entry in acls.csv

            return StatusCode(StatusCodes.Status201Created, new GenericReturnMessageDTO
            {
                Status = 201,
                    Message = SuccessMessages.SuperUserCreated // TODO: Change return message
            });
        }

        [HttpGet(ApiRoutes.ReadAccessControlEntry)]
        public ActionResult ReadAccessControlEntry(ReadAccessControlEntryDTO input)
        {
            // Check that the API KEY is correct to the one supplied in environment files
            if (!ValidAPIKey(input.APIKey))
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.AdminPasswordInCorrect });

            // TODO: Get entry in acls.csv

            return StatusCode(StatusCodes.Status200OK, new GenericReturnMessageDTO
            {
                Status = 200,
                    Message = SuccessMessages.SuperUserCreated // TODO: Change return message
            });
        }

        [HttpPatch(ApiRoutes.UpdateAccessControlEntry)]
        public ActionResult UpdateAccessControlEntry(UpdateAccessControlEntryDTO input)
        {
            // Check that the API KEY is correct to the one supplied in environment files
            if (!ValidAPIKey(input.APIKey))
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.AdminPasswordInCorrect });

            // TODO: Update entry in acls.csv

            return StatusCode(StatusCodes.Status200OK, new GenericReturnMessageDTO
            {
                Status = 200,
                    Message = SuccessMessages.SuperUserCreated // TODO: Change return message
            });
        }

        [HttpPatch(ApiRoutes.DeleteAccessControlEntry)]
        public ActionResult DeleteAccessControlEntry(DeleteAccessControlEntryDTO input)
        {
            // Check that the API KEY is correct to the one supplied in environment files
            if (!ValidAPIKey(input.APIKey))
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.AdminPasswordInCorrect });

            // TODO: Delete entry in acls.csv

            return StatusCode(StatusCodes.Status200OK, new GenericReturnMessageDTO
            {
                Status = 200,
                    Message = SuccessMessages.SuperUserCreated // TODO: Change return message
            });
        }

        private bool ValidAPIKey(string adminPassword)
        {
            return adminPassword.Equals(_configuration["KERBEROS_ADMIN_PW"]);
        }
    }
}
