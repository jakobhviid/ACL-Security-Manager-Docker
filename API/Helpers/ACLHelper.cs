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
        public static void AddEntries(List<AccessControlEntryDTO> entries)
        {
            var newRecords = new List<ACLEntryDefinition>();
            foreach (var entry in entries)
            {
                newRecords.Add(new ACLEntryDefinition
                {
                    KafkaPrincipal = "User:" + entry.PrincipalName,
                        ResourceType = entry.ResourceType.ToString(),
                        PatternType = entry.PatternType.ToString(),
                        ResourceName = entry.ResourceName,
                        Operation = entry.Operation.ToString(),
                        PermissionType = entry.PermissionType.ToString(),
                        Host = entry.Host
                });
            }
            if (File.Exists(ACLFilePath))
            {
                using(var reader = new StreamReader(ACLFilePath))
                using(var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var existingRecords = csvReader.GetRecords<ACLEntryDefinition>();
                    newRecords.AddRange(existingRecords);
                }
            }
            using(StreamWriter writer = new StreamWriter(ACLFilePath))
            using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<ACLEntryDefinition>();
                
                csv.NextRecord(); // Flushes the writer and appends a new line. Ready to write the new records
                
                csv.WriteRecords<ACLEntryDefinition>(newRecords);
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
    }
}
