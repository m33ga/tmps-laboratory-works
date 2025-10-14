namespace DocumentPipeline.Domain.Models
{
    public class Document
    {
        public string Name { get; set; }
        public DocumentType Type { get; set; }
        public int SizeKb { get; set; }

        public Document(string name, DocumentType type, int sizeKb)
        {
            Name = name;
            Type = type;
            SizeKb = sizeKb;
        }

        public override string ToString()
        {
            return $"{Name} ({Type}, {SizeKb}KB)";
        }
    }
}