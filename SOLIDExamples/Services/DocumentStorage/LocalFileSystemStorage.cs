using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.DocumentStorage
{
    public class LocalFileSystemStorage : DocumentStorageBase
    {
        private readonly string _basePath;

        public LocalFileSystemStorage(string basePath, ILogger<LocalFileSystemStorage> logger = null)
            : base(basePath, logger)
        {
            _basePath = basePath;
            Directory.CreateDirectory(_basePath);
        }

        public override async Task<UploadResult> UploadDocumentAsync(string fileName, Stream content, string contentType = "application/octet-stream")
        {
            try
            {
                var documentId = GenerateDocumentId(fileName);
                var filePath = Path.Combine(_basePath, documentId);
                using var fileStream = File.Create(filePath);
                await content.CopyToAsync(fileStream);
                var fileInfo = new FileInfo(filePath);
                LogOperation("Upload", documentId);
                return new UploadResult
                {
                    Success = true,
                    DocumentId = documentId,
                    Size = fileInfo.Length,
                    Message = "Document uploaded successfully"
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to upload document: {fileName}");
                throw new DocumentStorageException($"Failed to upload document: {ex.Message}", ex);
            }
        }

        public override async Task<Stream> DownloadDocumentAsync(string documentId)
        {
            await Task.Delay(1);
            var filePath = Path.Combine(_basePath, documentId);
            if (!File.Exists(filePath))
            {
                LogOperation("Download", documentId, false);
                throw new DocumentNotFoundException(documentId);
            }
            LogOperation("Download", documentId);
            return File.OpenRead(filePath);
        }

        public override async Task<bool> DeleteDocumentAsync(string documentId)
        {
            await Task.Delay(1);
            var filePath = Path.Combine(_basePath, documentId);
            if (!File.Exists(filePath))
            {
                LogOperation("Delete", documentId, false);
                return false;
            }
            File.Delete(filePath);
            LogOperation("Delete", documentId);
            return true;
        }

        public override async Task<DocumentInfo> GetDocumentInfoAsync(string documentId)
        {
            await Task.Delay(1);
            var filePath = Path.Combine(_basePath, documentId);
            if (!File.Exists(filePath))
                throw new DocumentNotFoundException(documentId);
            var fileInfo = new FileInfo(filePath);
            return new DocumentInfo
            {
                DocumentId = documentId,
                FileName = documentId,
                Size = fileInfo.Length,
                ContentType = GetContentType(documentId),
                CreatedAt = fileInfo.CreationTimeUtc,
                ModifiedAt = fileInfo.LastWriteTimeUtc,
                StorageProvider = StorageProvider.LocalFileSystem.ToString()
            };
        }

        public override async Task<bool> DocumentExistsAsync(string documentId)
        {
            await Task.Delay(1);
            var filePath = Path.Combine(_basePath, documentId);
            return File.Exists(filePath);
        }

        public override async Task<IEnumerable<DocumentInfo>> ListDocumentsAsync(string prefix = null)
        {
            await Task.Delay(1);
            var files = Directory.GetFiles(_basePath);
            var documents = new List<DocumentInfo>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (prefix != null && !fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    continue;
                var fileInfo = new FileInfo(file);
                documents.Add(new DocumentInfo
                {
                    DocumentId = fileName,
                    FileName = fileName,
                    Size = fileInfo.Length,
                    ContentType = GetContentType(fileName),
                    CreatedAt = fileInfo.CreationTimeUtc,
                    ModifiedAt = fileInfo.LastWriteTimeUtc,
                    StorageProvider = StorageProvider.LocalFileSystem.ToString()
                });
            }
            return documents;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
    }
}
