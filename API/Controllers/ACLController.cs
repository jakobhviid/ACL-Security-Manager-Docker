using System.Collections.Generic;
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
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.APIKeyIncorrect });

            if (input.Host == null) input.Host = "*";

            var superUserEntries = new List<AccessControlEntryDTO>()
            {
                // This one gives the user access to all topics in the cluster as there is wildcard matching on topic and host (if host hasn't been defined)
                new AccessControlEntryDTO
                {
                PrincipalName = input.PrincipalName,
                ResourceType = ResourceType.Topic,
                PatternType = PatternType.Literal,
                ResourceName = "*",
                Operation = OperationType.All,
                PermissionType = PermissionType.Allow,
                Host = input.Host
                },
                // This one gives the user acccess to create topics in the cluster
                new AccessControlEntryDTO
                {
                PrincipalName = input.PrincipalName,
                ResourceType = ResourceType.Cluster,
                PatternType = PatternType.Literal,
                ResourceName = "kafka-cluster",
                Operation = OperationType.Create,
                PermissionType = PermissionType.Allow,
                Host = input.Host
                },
                // This one makes sure that the users group doesn't matter. No matter what group the user is in, they will still be able to access all topics and create topics
                new AccessControlEntryDTO
                {
                PrincipalName = input.PrincipalName,
                ResourceType = ResourceType.Group,
                PatternType = PatternType.Literal,
                ResourceName = "*",
                Operation = OperationType.All,
                PermissionType = PermissionType.Allow,
                Host = input.Host
                }
            };
            ACLHelper.AddEntries(superUserEntries);

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
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.APIKeyIncorrect });

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
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.APIKeyIncorrect });

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
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.APIKeyIncorrect });

            // TODO: Update entry in acls.csv

            return StatusCode(StatusCodes.Status200OK, new GenericReturnMessageDTO
            {
                Status = 200,
                Message = SuccessMessages.SuperUserCreated // TODO: Change return message
            });
        }

        [HttpDelete(ApiRoutes.DeleteAccessControlEntry)]
        public ActionResult DeleteAccessControlEntry(DeleteAccessControlEntryDTO input)
        {
            // Check that the API KEY is correct to the one supplied in environment files
            if (!ValidAPIKey(input.APIKey))
                return StatusCode(StatusCodes.Status403Forbidden, new GenericReturnMessageDTO { Status = 403, Message = ErrorMessages.APIKeyIncorrect });

            // TODO: Delete entry in acls.csv

            return StatusCode(StatusCodes.Status200OK, new GenericReturnMessageDTO
            {
                Status = 200,
                Message = SuccessMessages.SuperUserCreated // TODO: Change return message
            });
        }

        private bool ValidAPIKey(string apiKey)
        {
            return apiKey.Equals(_configuration["ACL_API_KEY"]);
        }
    }
}
