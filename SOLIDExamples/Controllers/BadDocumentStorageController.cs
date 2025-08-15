using Microsoft.AspNetCore.Mvc;
using SOLIDExamples.Models;
using SOLIDExamples.Services.DocumentStorage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SOLIDExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadDocumentStorageController : ControllerBase
    {
        private readonly BadDocumentStorage _storage;

        public BadDocumentStorageController(BadDocumentStorage storage)
        {
            _storage = storage;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] string fileName, [FromForm] IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(fileName))
                return BadRequest("File and fileName are required.");
            using var stream = file.OpenReadStream();
            var result = await _storage.UploadDocumentAsync(fileName, stream, file.ContentType);
            if (result == null)
                return StatusCode(500, "Upload failed (null result)");
            return Ok(result);
        }

        [HttpGet("download/{documentId}")]
        public async Task<IActionResult> Download(string documentId)
        {
            try
            {
                var stream = await _storage.DownloadDocumentAsync(documentId);
                // This will throw generic Exception, not DocumentNotFoundException
                return File(stream, "application/octet-stream", documentId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> Delete(string documentId)
        {
            try
            {
                var deleted = await _storage.DeleteDocumentAsync(documentId);
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("info/{documentId}")]
        public async Task<IActionResult> Info(string documentId)
        {
            var info = await _storage.GetDocumentInfoAsync(documentId);
            if (info == null)
                return NotFound();
            return Ok(info);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] string prefix = null)
        {
            var docs = await _storage.ListDocumentsAsync(prefix);
            if (docs == null)
                return StatusCode(500, "List returned null");
            return Ok(docs);
        }
    }
}
