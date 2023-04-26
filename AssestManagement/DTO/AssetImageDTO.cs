namespace AssestManagement.DTO
{
    public class AssetImageDTO
    {
        
        public int? AssetId { get; set; }
       // public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
