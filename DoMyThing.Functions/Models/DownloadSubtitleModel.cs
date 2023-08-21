﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions.Models
{
    public class DownloadSubtitleModel : RequestModelBase
    {
        public string SearchText { get; set; }
        public string LanguageCode { get; set; } = "tur";
    }
}
