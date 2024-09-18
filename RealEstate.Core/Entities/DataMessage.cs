namespace RealEstate.Core.Entities
{
    public class DataMessage
    {
        public string SourceIp { get; set; }
        public string SourceEntity { get; set; }
        public string InformationSystem { get; set; }
        public string UserName { get; set; }
        public string MessageType { get; set; }
        public string MessageContent { get; set; }
        public string MessageDigest { get; set; }
    }
}
