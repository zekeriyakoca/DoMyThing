using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DoMyThing.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoMyThing.Common.Services
{
    public class BlobStorageService : IBlobStorageStreamService, IBlobStorageBase64Service, IBlobStorageByteService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<BlobStorageService> logger;
        private readonly BlobContainerClient container;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            var connectionString = configuration["BlobStorage"] ?? throw new ArgumentNullException("BlobStorage connectionstring cannot be null!");
            var containerName = configuration["BlobStorageSubtitleContainer"] ?? throw new ArgumentNullException("BlobStorage connectionstring cannot be null!");
            container = new BlobContainerClient(connectionString, containerName);
            container.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(string fileName, Stream stream)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (stream == null || stream.Length == 0)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            var normalizedFilename = Randomize(fileName);
            var client = GetBlobClient(normalizedFilename);

            stream.Position = 0;
            await client.UploadAsync(stream);

            return normalizedFilename;
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var client = GetBlobClient(fileName);
            var response = (await client.DownloadStreamingAsync()).Value;
            return response.Content;
        }

        public async Task<string> UploadFileAsync(string fileName, byte[] fileBytes)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (fileBytes == null || fileBytes.Length == 0)
            {
                throw new ArgumentNullException(nameof(fileBytes));
            }

            var normalizedFilename = Randomize(fileName);
            var client = GetBlobClient(normalizedFilename);

            if ((await client.ExistsAsync()).Value)
            {
                logger.LogInformation($"File[{fileName}] already exisit.");
                return normalizedFilename;
            }

            var data = new BinaryData(fileBytes);
            await client.UploadAsync(data);
            logger.LogInformation($"File[{fileName}] uploaded.");

            return normalizedFilename;
        }

        public async Task<byte[]> DownloadFileInBytesAsync(string fileName)
        {
            var client = container.GetBlobClient(Randomize(fileName));
            var response = (await client.DownloadContentAsync()).Value;
            return response.Content.ToArray();
        }

        public async Task<string> UploadFileAsync(string fileName, string fileAsBase64)
        {
            if (String.IsNullOrWhiteSpace(fileAsBase64))
            {
                throw new ArgumentNullException(nameof(fileAsBase64));
            }
            return await UploadFileAsync(fileName, Encoding.UTF8.GetBytes(fileAsBase64));
        }

        public async Task<string> DownloadFileAsBase64Async(string fileName)
        {
            var fileBytes = await DownloadFileInBytesAsync(fileName);
            if (fileBytes.Length == 0)
            {
                // TODO : Test here
                throw new Exception("Unable to download file!");
            }
            return Convert.ToBase64String(fileBytes);
        }


        private BlobClient GetBlobClient(string fileName)
        {
            return container.GetBlobClient(fileName);
        }

        private string Randomize(string fileName)
        {
            return Regex.Replace(fileName, @"/[^a-zA-Z ]/", "-");
        }
    }
}
