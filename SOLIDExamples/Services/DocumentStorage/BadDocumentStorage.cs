using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SOLIDExamples.Models;

namespace SOLIDExamples.Services.DocumentStorage
{
    // BAD: Violates LSP by returning different types and throwing inconsistent exceptions
    public class BadDocumentStorage : DocumentStorageBase
    {
        public BadDocumentStorage(string connectionString, ILogger<BadDocumentStorage> logger = null)
            : base(connectionString, logger) { }

        public override async Task<UploadResult> UploadDocumentAsync(string fileName, Stream content, string contentType = "application/octet-stream")
        {
            // Returns null instead of UploadResult on error (violates contract)
            return null;
        }

        public override async Task<Stream> DownloadDocumentAsync(string documentId)
        {
            // Throws generic Exception instead of DocumentNotFoundException
            throw new Exception("File not found");
        }

        public override async Task<bool> DeleteDocumentAsync(string documentId)
        {
            // Throws instead of returning false if not found
            throw new FileNotFoundException();
        }

        public override async Task<DocumentInfo> GetDocumentInfoAsync(string documentId)
        {
            // Returns null instead of throwing if not found
            return null;
        }

        public override async Task<bool> DocumentExistsAsync(string documentId)
        {
            // Always returns true (incorrect)
            return true;
        }

        public override async Task<IEnumerable<DocumentInfo>> ListDocumentsAsync(string prefix = null)
        {
            // Returns null instead of empty list
            return null;
        }
    }
}
