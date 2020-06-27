using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using API.Contracts;
using API.DTOs.inputDTOs;
using static API.DTOs.inputDTOs.ACLCommon;

namespace API.Helpers
{
    public static class ACLHelper
    {
        private static string ACLFilePath = "/conf/acls.csv";
        public static void AddEntries(List<AccessControlEntryDTO> entries)
        {
            if (!File.Exists(ACLFilePath))throw new ArgumentException(ErrorMessages.ACLFileDoesNotExist);

            var recordsToAdd = new List<string>();
            foreach (var entry in entries)
            {
                recordsToAdd.Add($"User:{entry.PrincipalName}," +
                    $"{entry.ResourceType.ToString()}," +
                    $"{entry.PatternType.ToString()}," +
                    $"{entry.ResourceName}," +
                    $"{entry.Operation.ToString()}," +
                    $"{entry.PermissionType.ToString()}," +
                    $"{entry.Host}");
            }
            File.AppendAllLines(ACLFilePath, recordsToAdd);
        }

        // This method uses very little memory, as the acls.csv file can become very big
        public static void DeleteEntry(AccessControlEntryDTO entry)
        {
            if (!File.Exists(ACLFilePath))throw new ArgumentException(ErrorMessages.ACLFileDoesNotExist);

            var entryToDelete = $"User:{entry.PrincipalName}," +
                $"{entry.ResourceType.ToString()}," +
                $"{entry.PatternType.ToString()}," +
                $"{entry.ResourceName}," +
                $"{entry.Operation.ToString()}," +
                $"{entry.PermissionType.ToString()}," +
                $"{entry.Host}";

            var linesToKeep = File.ReadAllLines(ACLFilePath).Where(l => !l.Equals(entry));

            // Overwriting the original file. We cannot use a temporary file and switch them around as it causes acl-manager to throw an error

            File.WriteAllText(ACLFilePath, "KafkaPrincipal,ResourceType,PatternType,ResourceName,Operation,PermissionType,Host"); // Writing the header, overwriting the file
            File.WriteAllLines(ACLFilePath, linesToKeep); // Adding all the lines we still want to keep

        }

        public static List<AccessControlEntryDTO> FetchEntriesForPrincipal(ReadAccessControlEntryDTO input)
        {
            if (!File.Exists(ACLFilePath))throw new ArgumentException(ErrorMessages.ACLFileDoesNotExist);

            var foundEntriesInFile = File.ReadAllLines(ACLFilePath).Where(l => l.Contains($"User:{input.PrincipalName}"));
            List<AccessControlEntryDTO> foundEntries = new List<AccessControlEntryDTO>();
            foreach (var foundEntry in foundEntriesInFile)
            {
                var splittedParams = foundEntry.Split(",");
                // TODO: Try catch for each splitted argument, in case somehow an entry is added where it doesn't exist
                foundEntries.Add(new AccessControlEntryDTO
                {
                    PrincipalName = splittedParams[0],
                        ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), splittedParams[1], true),
                        PatternType = (PatternType)Enum.Parse(typeof(PatternType), splittedParams[2], true),
                        ResourceName = splittedParams[3],
                        Operation = (OperationType)Enum.Parse(typeof(OperationType), splittedParams[4], true),
                        PermissionType = (PermissionType)Enum.Parse(typeof(PermissionType), splittedParams[5], true),
                        Host = splittedParams[6]
                });
            }
            return foundEntries;
        }
    }
}
