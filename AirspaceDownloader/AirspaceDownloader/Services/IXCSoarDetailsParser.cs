using System;
using System.Collections.Generic;
using System.Text;

namespace AirspaceDownloader.Services
{
    public interface IXCSoarDetailsParser
    {
        List<string> GetFilesList(string detailsContent);
    }
}
