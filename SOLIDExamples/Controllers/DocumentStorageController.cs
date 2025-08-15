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
    public class DocumentStorageController : ControllerBase
    {
        private readonly DocumentStorageBase _storage;

        public DocumentStorageController(DocumentStorageBase storage)
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
            return Ok(result);
        }

        [HttpGet("download/{documentId}")]
        public async Task<IActionResult> Download(string documentId)
        {
            try
            {
                var stream = await _storage.DownloadDocumentAsync(documentId);
                var info = await _storage.GetDocumentInfoAsync(documentId);
                return File(stream, info.ContentType ?? "application/octet-stream", info.FileName);
            }
            catch (DocumentNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> Delete(string documentId)
        {
            var deleted = await _storage.DeleteDocumentAsync(documentId);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        [HttpGet("info/{documentId}")]
        public async Task<IActionResult> Info(string documentId)
        {
            try
            {
                var info = await _storage.GetDocumentInfoAsync(documentId);
                return Ok(info);
            }
            catch (DocumentNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] string prefix = null)
        {
            var docs = await _storage.ListDocumentsAsync(prefix);
            return Ok(docs);
        }
    }
}
