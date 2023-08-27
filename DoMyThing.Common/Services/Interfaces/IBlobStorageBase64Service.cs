using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Common.Services.Interfaces
{
    public interface IBlobStorageBase64Service
    {
        Task<string> UploadFileAsync(string fileName, string base64FileContent);
        Task<string> DownloadFileAsBase64Async(string fileName);
    }
}
