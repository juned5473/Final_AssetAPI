using AssestManagement.DTO;
using AssestManagement.Interfaces;
using AssestManagement.Models;
using AssestManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssestManagement.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetRepository assetRepository, ILogger<AssetsController> logger)
        {
            _assetRepository = assetRepository;
            _logger = logger;
        }
        [Authorize]
        // GET: api/Assets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            try
            {
                var assets = await _assetRepository.GetAssets();
                return Ok(assets);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting assets: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Assets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            try
            {
                var asset = await _assetRepository.GetAssetById(id);

                if (asset == null)
                {
                    return NotFound();
                }

                return Ok(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting asset with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        // POST: api/Assets
        [HttpPost]
        public async Task<ActionResult<int>> AddAsset(Asset asset)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int assetId = await _assetRepository.AddAsset(asset);
                
                return Ok($"AssetID {assetId} Added");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding asset: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }





        // PUT: api/Assets/5
        [HttpPut]
        public async Task<IActionResult> UpdateAsset( Asset asset)
        {
            try
            {
                
                await _assetRepository.UpdateAsset(asset);
                return Ok("Asset Details Updated");
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating asset: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

      //  POST: api/Assets/BulkUpdate
      
        [HttpPost("bulkUpdate")]
        public async Task<IActionResult> BulkUpdateAssets([FromBody] IEnumerable<AssetBulkUpdateRequest> request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _assetRepository.BulkUpdateAssets(request);              

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating assets: {ex.Message}");
                return StatusCode(500, "Internal server error");



            }
        }

        [HttpGet("assetImage/{assetImageId}")]
        public async Task<IActionResult> GetAssetImageById(int assetImageId)
        {
            try
            {
                var assetImage = await _assetRepository.GetAssetImageById(assetImageId);
                if (assetImage == null)
                {
                    return NotFound();
                }
                return File(assetImage.Content, "image/jpeg"); // assuming the image content is JPEG
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Retrieving Asset Image: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost("addImage")]
        public async Task<IActionResult> AddAssetImage([FromForm] AssetImageDTO assetImageDTO)
        {
            try
            {
                byte[] imageData;
               using (var memoryStream = new MemoryStream())
                {
                    await assetImageDTO.File.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();                    
                }

                var assetImage = new AssetImage
                {
                    AssetId = assetImageDTO.AssetId,
                    Name = assetImageDTO.File.FileName,
                    Content = imageData
                };

                await _assetRepository.AddAssetImage(assetImage);
                return Ok("Asset Image Added");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Adding Asset: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }



        //[HttpPost("addImage")]
        //public async Task<IActionResult> AddAssetImage([FromBody] AssetImageDTO assetImageDTO )
        //{
        //    try
        //    {
        //        byte[] imageContent = System.IO.File.ReadAllBytes(assetImageDTO.ImagePath);
        //        string base64String = Convert.ToBase64String(imageContent);

        //        var newAssetImage = new AssetImage
        //        {
        //            AssetId = assetImageDTO.AssetId,
        //            Name = assetImageDTO.ImageName,
        //            //Content = base64String

        //        };
        //        await _assetRepository.AddAssetImage(newAssetImage);
        //        return Ok("Asset Image Added");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error Adding Asset: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }

        //}
        [HttpPut("updateAssetImage")]
        //public async Task<IActionResult> UpdateAssetImage([FromBody] AssetImageDTO assetImageDTO)
        //{
        //    try
        //    {

        //        byte[] imageContent = System.IO.File.ReadAllBytes(assetImageDTO.ImagePath);
        //        string base64String = Convert.ToBase64String(imageContent);

        //        var newAssetImage = new AssetImage
        //        {   
        //            AssetImageId = assetImageDTO.AssetImageId,
        //            AssetId = assetImageDTO.AssetId,
        //            Name = assetImageDTO.ImageName,
        //     //       Content = base64String

        //        };
        //        await _assetRepository.UpdateAssetImage(newAssetImage);
        //        return Ok("Asset Image Updated");

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error updating asset: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }

        //}
        [HttpGet("getDocument/{documentId}")]
        public async Task<IActionResult> GetAssetDocumentById(int documentId)
        {
            try
            {
                var document = await _assetRepository.GetAssetDocumentById(documentId);
                if (document == null)
                {
                    return NotFound();
                }
                return File(document.Content, "application/pdf"); // assuming the document content is a PDF
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Retrieving Asset Document: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("addDocument")]
        public async Task<IActionResult> AddAssetDocument([FromForm] DocumentDTO documentDTO)
        {
            try
            {
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await documentDTO.File.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                var assetDocument = new Document
                {
                    AssetId = documentDTO.AssetId,
                    DocumentTitle = documentDTO.DocumentTitle,
                    Description = documentDTO.Description,
                    FileName = documentDTO.File.FileName,
                    FileExtension = Path.GetExtension(documentDTO.File.FileName),
                    Content = fileData
                };

                await _assetRepository.AddAssetDocument(assetDocument);
                return Ok("Asset Document Added");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Adding Asset Document: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _assetRepository.DeleteAssetById(id);
            return Ok("Asset Data Deleted");
        }
    }
}
//[HttpPut("updateDocument")]
//public async Task<IActionResult> UpdateAssetDocument([FromBody] DocumentDTO documentDTO)
//{
//    try
//    {
//        {

//            var newDocument = new Document
//            {
//                DocumentId = documentDTO.DocumentId,
//                AssetId = documentDTO.AssetId,
//                DocumentTitle = documentDTO.DocumentTitle,
//                Description = documentDTO.Description,
//                FileName = documentDTO.FileName,
//                FileExtension = documentDTO.FileExtension,
//                Content = documentDTO.DocumentPath
//            };
//            await _assetRepository.UpdateAssetDocument(newDocument);
//            return Ok("Asset Document Updated");

//        }
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError($"Error Updating Document: {ex.Message}");
//        return StatusCode(500, "Internal server error");
//    }