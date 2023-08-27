using DoMyThing.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions.Services
{
    public class SubtitleStorageAppService
    {
        private readonly IBlobStorageByteService blobStorageService;
        private readonly string containerName;

        public SubtitleStorageAppService(IConfiguration configuration, IBlobStorageByteService blobStorageService)
        {
            this.blobStorageService = blobStorageService;
            containerName = configuration["BlobStorageSubtitleContainer"] ?? throw new ArgumentNullException("Container name cannot be null!");

        }

        public async Task<string> UploadFileAsync(string fileName, byte[] fileInBytes)
        {
            return await blobStorageService.UploadFileAsync(containerName, fileName, fileInBytes);
        }
    }
}
