using AssestManagement.DTO;
using AssestManagement.Models;

namespace AssestManagement.Interfaces
{
    public interface IAssetRepository
    {
       Task<int> AddAsset(Asset asset);
        Task UpdateAsset(Asset asset);
        Task<Asset> GetAssetById(int id);
        Task<IEnumerable<Asset>> GetAssets();
        Task<bool> BulkUpdateAssets(IEnumerable<AssetBulkUpdateRequest> request);

        Task AddAssetImage(AssetImage assetImage);
        Task<AssetImage> GetAssetImageById(int assetImageId);
        Task UpdateAssetImage(AssetImage assetImage);
        Task AddAssetDocument(Document document);
        Task<Document> GetAssetDocumentById(int documentId);
        Task UpdateAssetDocument (Document document);
        Task<Asset> DeleteAssetById(int id);
    }
}
