namespace AssestManagement.DTO
{
    public class DocumentDTO
    {
        public int AssetId { get; set; }
        public string DocumentTitle { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
