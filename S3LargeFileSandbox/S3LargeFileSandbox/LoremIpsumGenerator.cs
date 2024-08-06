using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3LargeFileSandbox
{
    public class LoremIpsumGenerator
    {
        private static readonly string LoremIpsumText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        public static void GenerateLoremIpsumFile(string filePath, long fileSizeInBytes)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                long currentSize = 0;
                while (currentSize < fileSizeInBytes)
                {
                    writer.Write(LoremIpsumText);
                    currentSize += Encoding.UTF8.GetByteCount(LoremIpsumText);
                }
            }
        }
    }
}
