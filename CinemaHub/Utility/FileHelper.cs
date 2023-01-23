using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Utility
{
    public class FileHelper
    {
        
        public static string GetUniqueFileName(string fileName = "image")
        {

            fileName = Path.GetFileName(fileName);
            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                , "_"
                , Guid.NewGuid().ToString().AsSpan(0, 13)
                , Path.GetExtension(fileName));
        }
    }
}
