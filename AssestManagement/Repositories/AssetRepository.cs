using AssestManagement.Interfaces;
using AssestManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc;
using AssestManagement.DTO;

namespace AssestManagement.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly string _ConnectionString;

        public AssetRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddAsset(Asset asset)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetName", asset.AssetName);
                    parameters.Add("@AssetCategory", asset.AssetCategory);
                    parameters.Add("@AssetType", asset.AssetType);
                    parameters.Add("@AssetId", SqlDbType.Int, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("InsertAsset", parameters, commandType: CommandType.StoredProcedure);

                    return parameters.Get<int>("@AssetId");
                  
                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Error in AddAsset method", ex);
            }
        } 

        public async Task<IEnumerable<Asset>> GetAssets()
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    return await connection.QueryAsync<Asset>("GetAssets", commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in GetAssets method", ex);
            }
        }

        public async Task<Asset> GetAssetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetId", id);

                    return await connection.QueryFirstOrDefaultAsync<Asset>("GetAssetById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in GetAssetById method", ex);
            }
        }
        public async Task UpdateAsset(Asset asset)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetId", asset.AssetId);
                    parameters.Add("@AssetName", asset.AssetName);
                    parameters.Add("@AssetCategory", asset.AssetCategory);
                    parameters.Add("@AssetType", asset.AssetType);

                    await connection.ExecuteAsync("UpdateAsset", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in UpdateAsset method", ex);
            }
        }

        public async Task<bool> BulkUpdateAssets(IEnumerable<AssetBulkUpdateRequest> request)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                  
                    var dataTable = new DataTable();

                    dataTable.Columns.Add("@AssetId", typeof(int));
                    dataTable.Columns.Add("@AssetName", typeof(string));
                    dataTable.Columns.Add("@AssetCategory", typeof(string));
                    dataTable.Columns.Add("@AssetType", typeof(string));

                    foreach (var asset in request)
                    {
                        dataTable.Rows.Add(asset.AssetId, asset.AssetName, asset.AssetCategory, asset.AssetType);
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetsToUpdate", dataTable.AsTableValuedParameter("dbo.AssetTableType"));
                    var rowsAffected = await connection.ExecuteAsync("BulkUpdate", parameters, commandType: CommandType.StoredProcedure);

                    return rowsAffected > 0;

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in BulkUpdateAsset ", ex);
            }
        }



        public async Task<AssetImage> GetAssetImageById(int assetImageId)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetImageId", assetImageId);

                    var result = await connection.QueryFirstOrDefaultAsync<AssetImage>("GetAssetImageById", parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in GetAssetImageById", ex);
            }
        }





        public async Task AddAssetImage(AssetImage assetImage)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetId", assetImage.AssetId);
                    parameters.Add("@Name", assetImage.Name);
                    parameters.Add("@Content", assetImage.Content);

                    await connection.ExecuteAsync("InsertAssetImage", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Error in AddAssetImage", ex);
            }
        }

        public async Task UpdateAssetImage(AssetImage assetImage)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetImageId", assetImage.AssetImageId);                    
                    parameters.Add("@Name", assetImage.Name);
                    parameters.Add("@Content", assetImage.Content);

                    await connection.ExecuteAsync("UpdateAssetImage", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in UpdateAssetImage method", ex);
            }
        }


        public async Task<Document> GetAssetDocumentById(int documentId)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@DocumentId", documentId);

                    var document = await connection.QuerySingleOrDefaultAsync<Document>("GetDocumentById", parameters, commandType: CommandType.StoredProcedure);

                    return document;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in GetDocument method", ex);
            }
        }


        public async Task AddAssetDocument(Document document)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {


                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetId", document.AssetId);
                    parameters.Add("@DocumentTitle", document.DocumentTitle);
                    parameters.Add("@Description", document.Description);
                    parameters.Add("@FileName", document.FileName);
                    parameters.Add("@FileExtension", document.FileExtension);
                    parameters.Add("@Content", document.Content);

                    await connection.ExecuteAsync("InsertDocument", parameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in AddAssetDocument method", ex);
            }
        }
        public async Task UpdateAssetDocument(Document document)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@DocumentId", document.DocumentId);
                    parameters.Add("@DocumentTitle", document.DocumentTitle);
                    parameters.Add("@Description", document.Description);
                    parameters.Add("@FileName", document.FileName);
                    parameters.Add("@FileExtension", document.FileExtension);
                    parameters.Add("@Content", document.Content);

                    await connection.ExecuteAsync("UpdateAssetDocument", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in UpdateAssetImage method", ex);
            }
        }

        public async Task<Asset> DeleteAssetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AssetId", id);



                    return await connection.QueryFirstOrDefaultAsync<Asset>("DeleteAssetById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error in DeleteAssetById method", ex);
            }
        }
    }
}
