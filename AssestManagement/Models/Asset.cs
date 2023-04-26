using System.ComponentModel.DataAnnotations;

namespace AssestManagement.Models
{
    public class Asset
    {
        [Key]
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetCategory { get; set; }
        public string AssetType { get; set; }
        
        public AssetImage? AssetImages { get; set;}
        public Document? Documents { get; set; }

    }
}
