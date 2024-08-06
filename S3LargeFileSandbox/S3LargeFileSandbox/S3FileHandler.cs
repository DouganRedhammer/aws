using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class S3FileHandler
{
    private static IAmazonS3 _s3Client;

    public event EventHandler<UploadProgressArgs> UploadProgressEvent;
    public event EventHandler<WriteObjectProgressArgs> DownloadProgressEvent;

    public S3FileHandler()
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USWest1,
            Timeout = TimeSpan.FromMinutes(10), 
            ReadWriteTimeout = TimeSpan.FromMinutes(10), 
            MaxErrorRetry = 3
        };

        _s3Client = new AmazonS3Client(config);
    }

    public async Task UploadFileWithTransferUtilityAsync(string bucketName, string keyName, string filePath)
    {
        try
        {
            var fileTransferUtility = new TransferUtility(_s3Client);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                Key = keyName,
                FilePath = filePath,
                PartSize = 5 * 1024 * 1024,
                CannedACL = S3CannedACL.Private
            };

            uploadRequest.UploadProgressEvent += (sender, e) =>
            {
                UploadProgressEvent?.Invoke(sender, e);
            };

            var stopwatch = Stopwatch.StartNew();
            await fileTransferUtility.UploadAsync(uploadRequest);
            stopwatch.Stop();

            Console.WriteLine($"File uploaded successfully in {stopwatch.Elapsed.TotalSeconds} seconds.");
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Amazon S3 error: {e.Message}");
        }
        catch (AmazonServiceException e)
        {
            Console.WriteLine($"Amazon Service error: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Argument error: {e.Message}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"IO error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown error: {e.Message}");
        }
    }

    public async Task DownloadFileWithTransferUtilityAsync(string bucketName, string keyName, string filePath)
    {
        try
        {
            var fileTransferUtility = new TransferUtility(_s3Client);

            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = keyName,
                FilePath = filePath
            };

            downloadRequest.WriteObjectProgressEvent += (sender, e) =>
            {
                DownloadProgressEvent?.Invoke(sender, e);
            };

            var stopwatch = Stopwatch.StartNew();
            await fileTransferUtility.DownloadAsync(downloadRequest);
            stopwatch.Stop();

            Console.WriteLine($"File downloaded successfully in {stopwatch.Elapsed.TotalSeconds} seconds.");
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Amazon S3 error: {e.Message}");
        }
        catch (AmazonServiceException e)
        {
            Console.WriteLine($"Amazon Service error: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Argument error: {e.Message}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"IO error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown error: {e.Message}");
        }
    }

    public async Task UploadFileWithPutObjectAsync(string bucketName, string keyName, string filePath)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                FilePath = filePath
            };

            var stopwatch = Stopwatch.StartNew();
            var response = await _s3Client.PutObjectAsync(putRequest);
            stopwatch.Stop();

            Console.WriteLine($"File uploaded successfully in {stopwatch.Elapsed.TotalSeconds} seconds. HTTP Status Code: {response.HttpStatusCode}");
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Amazon S3 error: {e.Message}");
        }
        catch (AmazonServiceException e)
        {
            Console.WriteLine($"Amazon Service error: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Argument error: {e.Message}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"IO error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown error: {e.Message}");
        }
    }

    public async Task DownloadFileWithGetObjectAsync(string bucketName, string keyName, string filePath)
    {
        try
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            var stopwatch = Stopwatch.StartNew();
            using (var response = await _s3Client.GetObjectAsync(getRequest))
            {
                await response.WriteResponseStreamToFileAsync(filePath, false, default);
            }
            stopwatch.Stop();

            Console.WriteLine($"File downloaded successfully in {stopwatch.Elapsed.TotalSeconds} seconds.");
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Amazon S3 error: {e.Message}");
        }
        catch (AmazonServiceException e)
        {
            Console.WriteLine($"Amazon Service error: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Argument error: {e.Message}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"IO error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown error: {e.Message}");
        }
    }
}
