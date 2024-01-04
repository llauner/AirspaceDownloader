using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AirspaceDownloader.Models
{
    public class FileDescription
    {
        public string Url { get; set; }
        public bool IsXCSoarOnly = false;
        public bool IsZip = false;

        public string Filename => Path.GetFileName(Url);

        public FileDescription(string url, bool isXCSoarOnly, bool isZip=false)
        {
            Url = url;
            IsXCSoarOnly = isXCSoarOnly;
            IsZip = isZip;
        }
    }
}
