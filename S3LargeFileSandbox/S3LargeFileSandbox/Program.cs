using Amazon;

namespace S3LargeFileSandbox
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var fileName = "lorem_ipsum_20mb.txt";
            var bucketName = "<REPLACE_WITH_BUCKNAME>";
            var keyName = fileName;
            var filePath = fileName;
            var fileSize = 20 * 10224 * 1024; // 20MB

            LoremIpsumGenerator.GenerateLoremIpsumFile(fileName, fileSize); // Generate a file for testing
            var s3FileHandler = new S3FileHandler();

            s3FileHandler.UploadProgressEvent += (sender, e) =>
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                Console.Write($"Uploaded {e.TransferredBytes} of {e.TotalBytes} bytes. {e.PercentDone}% complete.".PadRight(Console.WindowWidth));
            };

            s3FileHandler.DownloadProgressEvent += (sender, e) =>
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                Console.Write($"Downloaded {e.TransferredBytes} of {e.TotalBytes} bytes. {e.PercentDone}% complete.".PadRight(Console.WindowWidth));
            };

            await s3FileHandler.UploadFileWithTransferUtilityAsync(bucketName, keyName, filePath);
            Console.WriteLine("File uploaded successfully.");
            Console.Clear();
            await s3FileHandler.DownloadFileWithTransferUtilityAsync(bucketName, keyName, "downloaded_" + fileName);
            Console.WriteLine("File downloaded successfully.");
            Console.ReadLine();
        }

        static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

    }
}
