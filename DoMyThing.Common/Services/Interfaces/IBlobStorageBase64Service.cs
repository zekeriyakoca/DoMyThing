using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Common.Services.Interfaces
{
    public interface IBlobStorageBase64Service
    {
        /// <summary>
        /// Upload file to Blob Storage
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileInBytes"></param>
        /// <returns>uploaded file name as in storage</returns>
        Task<string> UploadFileAsync(string containerName, string fileName, string base64FileContent);
        Task<string> DownloadFileAsBase64Async(string containerName, string fileName);
    }
}
