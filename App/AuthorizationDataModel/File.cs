namespace ApplicationDb.Entities
{
    public class File: NamedObject
    {
        
        public string ContentType { get; set; }
        public byte[] BinaryData { get; set; }
    }
}