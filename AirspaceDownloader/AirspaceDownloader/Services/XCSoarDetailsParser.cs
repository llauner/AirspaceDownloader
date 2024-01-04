using System;
using System.Collections.Generic;
using System.Text;

namespace AirspaceDownloader.Services
{
    public class XCSoarDetailsParser : IXCSoarDetailsParser
    {
        public List<string> GetFilesList(string detailsContent)
        {
            var listFiles = new List<string>();

            string[] lines = detailsContent.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (line.StartsWith("image"))
                {
                    var filePath = line.Replace("image=", "");
                    listFiles.Add(filePath);
                }
            }
            return listFiles;
        }
    }
}
