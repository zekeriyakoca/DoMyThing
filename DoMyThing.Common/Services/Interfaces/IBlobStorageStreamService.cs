using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Common.Services.Interfaces
{
    public interface IBlobStorageStreamService
    {
        Task<string> UploadFileAsync(string fileName, Stream stream);
        Task<Stream> DownloadFileAsync(string fileName);
    }
}
