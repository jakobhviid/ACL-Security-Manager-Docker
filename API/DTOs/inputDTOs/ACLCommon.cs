using System.ComponentModel;

namespace API.DTOs.inputDTOs
{
    public class ACLCommon
    {
        public enum ResourceType
        {
            [Description("topic")]
            Topic, [Description("group")]
            Group, [Description("cluster")]
            Cluster
        }

        public enum PatternType
        {
            [Description("literal")]
            Literal, [Description("prefixed")]
            Prefixed
        }

        public enum OperationType
        {
            [Description("read")]
            Read, [Description("write")]
            Write, [Description("create")]
            Create, [Description("all")]
            All, [Description("describe")]
            Describe
        }

        public enum PermissionType
        {
            [Description("allow")]
            Allow, [Description("deny")]
            Deny
        }
    }
}
