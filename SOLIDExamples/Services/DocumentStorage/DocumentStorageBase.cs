using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.DocumentStorage
{
    public abstract class DocumentStorageBase
    {
        protected readonly string _connectionString;
        protected readonly ILogger _logger;

        protected DocumentStorageBase(string connectionString, ILogger logger = null)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger;
        }

        public abstract Task<UploadResult> UploadDocumentAsync(string fileName, Stream content, string contentType = "application/octet-stream");
        public abstract Task<Stream> DownloadDocumentAsync(string documentId);
        public abstract Task<bool> DeleteDocumentAsync(string documentId);
        public abstract Task<DocumentInfo> GetDocumentInfoAsync(string documentId);
        public abstract Task<bool> DocumentExistsAsync(string documentId);
        public abstract Task<IEnumerable<DocumentInfo>> ListDocumentsAsync(string prefix = null);

        protected virtual string GenerateDocumentId(string fileName)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var guid = Guid.NewGuid().ToString("N")[..8];
            var extension = Path.GetExtension(fileName);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            return $"{nameWithoutExtension}_{timestamp}_{guid}{extension}";
        }

        protected virtual void LogOperation(string operation, string documentId, bool success = true)
        {
            _logger?.LogInformation($"{operation} - DocumentId: {documentId}, Success: {success}");
        }
    }
}
