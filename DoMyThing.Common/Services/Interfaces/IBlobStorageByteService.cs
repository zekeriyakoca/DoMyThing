using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Common.Services.Interfaces
{
    public interface IBlobStorageByteService
    {
        Task<string> UploadFileAsync(string fileName, byte[] fileInBytes);
        Task<byte[]> DownloadFileInBytesAsync(string fileName);
    }
}
