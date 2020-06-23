using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using API.Contracts;
using API.DTOs.inputDTOs;
using CsvHelper;

namespace API.Helpers
{
    public static class ACLHelper
    {
        private static string ACLFilePath = "/Users/oliver/OfflineDocuments/GitProjects/Arbejde/ACL-Security-Manager-Docker/acls.csv";
        public static void AddEntry(AccessControlEntryDTO input)
        {
            FileExists();
            using(StreamWriter writer = new StreamWriter(ACLFilePath))
            using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(new ACLEntryDefinition
                {
                    KafkaPrincipal = "User:" + input.PrincipalName,
                        ResourceType = input.ResourceType.ToString(),
                        PatternType = input.PatternType.ToString(),
                        ResourceName = input.ResourceName,
                        Operation = input.Operation.ToString(),
                        PermissionType = input.PermissionType.ToString(),
                        Host = input.Host
                });
            }
        }

        // This method uses very little memory, as the acls.csv file can become very big
        public static void DeleteEntry(AccessControlEntryDTO input)
        {
            if (!File.Exists(ACLFilePath))throw new ArgumentException(SuccessMessages.ACLEntryDoesNotExist);

            using(var reader = new StreamReader(ACLFilePath))
            using(var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture)) // invairant culture has to do with date & time conversion
            {
                var entries = csvReader.GetRecords<ACLEntryDefinition>();
                // Filter list
                var updatedEntries = new List<ACLEntryDefinition>();
                foreach (var csvEntry in entries)
                {
                    if (!input.EqualsCsvEntry(csvEntry))updatedEntries.Add(csvEntry);
                }

                using(var writer = new StreamWriter(ACLFilePath))
                using(var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(updatedEntries);
                }
            }
        }

        public static List<ACLEntryDefinition> FetchEntriesForPrincipal(ReadAccessControlEntryDTO input)
        {
            if (!File.Exists(ACLFilePath))throw new ArgumentException(SuccessMessages.ACLEntryDoesNotExist);

            List<ACLEntryDefinition> foundEntries = new List<ACLEntryDefinition>();

            using(var reader = new StreamReader(ACLFilePath))
            using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) // invairant culture has to do with date & time conversion
            {
                var entries = csv.GetRecords<ACLEntryDefinition>();
                foreach (var entry in entries)
                {
                    if (entry.KafkaPrincipal.Equals(input.PrincipalName))foundEntries.Add(entry);
                }
            }
            return foundEntries;
        }

        // Checks that acls.csv exists and if it doesn't, it will set the header correctly.
        private static void FileExists()
        {
            if (!File.Exists(ACLFilePath))
            {
                using(StreamWriter sw = File.CreateText(ACLFilePath))
                {
                    sw.WriteLine("KafkaPrincipal,ResourceType,PatternType,ResourceName,Operation,PermissionType,Host");
                }
            }
        }
    }
}
