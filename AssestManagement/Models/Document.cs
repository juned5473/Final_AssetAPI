using System.ComponentModel.DataAnnotations;

namespace AssestManagement.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public int AssetId { get; set; }
        public string DocumentTitle { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] Content { get; set; }

        public Asset Asset { get; set; }

    }
}
