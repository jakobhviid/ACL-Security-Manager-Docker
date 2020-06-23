namespace API
{
    public class ACLEntryDefinition
    {
        public string KafkaPrincipal { get; set; }
        public string ResourceType { get; set; }
        public string PatternType { get; set; }
        public string ResourceName { get; set; }
        public string Operation { get; set; }
        public string PermissionType { get; set; }
        public string Host { get; set; }
    }
}