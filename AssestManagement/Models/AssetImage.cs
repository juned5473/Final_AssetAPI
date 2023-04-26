using System.ComponentModel.DataAnnotations;

namespace AssestManagement.Models
{
    public class AssetImage
    {
        [Key]
        public int AssetImageId { get; set; }
        public int? AssetId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }

        public Asset Asset { get; set; }


    }
}
