using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions.Models
{
    public class DownloadSubtitleResponseModel : ResponseModelBase
    {
        public DownloadSubtitleResponseModel(string fileName, string movieName)
        {
            FileName = fileName;
            MovieName = movieName;
        }
        public string FileName { get; set; }
        public string MovieName { get; set; }
    }
}
