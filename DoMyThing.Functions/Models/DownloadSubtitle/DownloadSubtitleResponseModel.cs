using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions.Models
{
    public class DownloadSubtitleResponseModel : ResponseModelBase
    {
        public DownloadSubtitleResponseModel(string firstFileName, string secondFileName, string movieName)
        {
            FirstFileName = firstFileName;
            SecondFileName = secondFileName;
            MovieName = movieName;
        }
        public string FirstFileName { get; set; }
        public string SecondFileName { get; set; }
        public string MovieName { get; set; }
    }
}
