using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public class AwsS3StorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsS3BucketOptions _s3BucketOptions;

        public AwsS3StorageService(IAmazonS3 s3Client, AwsS3BucketOptions s3BucketOptions)
        {
            _s3Client = s3Client;
            _s3BucketOptions = s3BucketOptions;
        }

        public async Task<bool> DeleteFileByLink(string fileLink)
        {
            string fileName, directory;
            ExtractFileLinkParameters(fileLink, out fileName, out directory);

            return await DeleteFile(fileName, directory);
        }

        public async Task<bool> DeleteFile(string fileName, string directory = null)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                var bucketPath = !string.IsNullOrWhiteSpace(directory)
                    ? _s3BucketOptions.BucketName + @"/" + directory
                    : _s3BucketOptions.BucketName;

                await fileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = bucketPath,
                    Key = fileName
                });
                Debug.WriteLine($"successfully deleted {fileName} from {bucketPath}");
                return true;
            }
            catch(AmazonServiceException s3Err)            
            {
                if (s3Err.ErrorCode != null &&
                    (s3Err.ErrorCode.Equals("InvalidAccessKeyId") ||
                     s3Err.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Amazon Exception LOG: Please check the provided AWS Credentials.");
                }
                else
                {
                    Console.WriteLine(s3Err.Message);
                }

                return false;
            }

            catch (Exception err)
            {
                Debug.WriteLine($"Unknown encountered on server. Message:'{err.Message}' when writing an object");
                throw;
            }
        }

        public async Task<string> EditFileByLink(byte[] content, string fileLink)
        {
            string fileName, directory;
            ExtractFileLinkParameters(fileLink, out fileName, out directory);

            return await EditFile(content, fileName, directory);
        }

        public async Task<string> EditFile(byte[] content, string fileName, string directory = null, string prevFileLink = "")
        {            
            if (prevFileLink == "")
                await DeleteFile(fileName, directory);
            else
                await DeleteFileByLink(prevFileLink);

            return await SaveFile(content, fileName, directory);
        }

        public async Task<string> EditFile(byte[] content, string fileName, string directory = null)
        {
            await DeleteFile(fileName, directory);

            return await SaveFile(content, fileName, directory);
        }

        public async Task<string> SaveFile(byte[] content, string fileName, string directory = null, bool appendHttp = true, bool appendHttps = false)
        {
            MemoryStream msContent = null;

            try
            {
                msContent = new MemoryStream(content);

                var fileTransferUtility = new TransferUtility(_s3Client);

                var bucketPath = !string.IsNullOrWhiteSpace(directory)
                    ? _s3BucketOptions.BucketName + @"/" + directory
                    : _s3BucketOptions.BucketName;

                var fileUploadRequest = new TransferUtilityUploadRequest()
                {
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = bucketPath,
                    Key = fileName,
                    InputStream = msContent
                };

                fileUploadRequest.UploadProgressEvent += (sender, args) =>
                    Console.WriteLine($"{args.FilePath} upload complete : {args.PercentDone}%");

                await fileTransferUtility.UploadAsync(fileUploadRequest);
                Debug.WriteLine($"successfully uploaded {fileName} to {bucketPath} on {DateTime.UtcNow:O}");

                var s3FileLink = _s3BucketOptions.BucketName + ".s3.amazonaws.com" +
                    (!string.IsNullOrWhiteSpace(directory) ? @"/" + directory : "") +
                    @"/" + fileName;

                if (appendHttp)
                    s3FileLink = "http://" + s3FileLink;
                else if (appendHttps)
                    s3FileLink = "https://" + s3FileLink;

                msContent.Close();
                msContent = null;

                return s3FileLink;
            }
            catch (AmazonServiceException s3Err)
            {
                if (s3Err.ErrorCode != null &&
                    (s3Err.ErrorCode.Equals("InvalidAccessKeyId") ||
                     s3Err.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("LOG: Please check the provided AWS Credentials.");
                }
                else
                {
                    Console.WriteLine($"LOG: An error occurred with the message '{s3Err.Message}' when uploading { fileName}");
                }

                return "";
            }
            finally
            {
                if (msContent != null)
                    msContent.Close();
            }
        }

        private void ExtractFileLinkParameters(string fileLink, out string fileName, out string directory)
        {
            fileLink = fileLink.Replace("http://", "").Replace("https://", "").Replace(".s3.amazonaws.com", "");
            var fileLinkAry = fileLink.Split("/");
            fileName = fileLinkAry[fileLinkAry.Length - 1];
            directory = "";

            if (fileLinkAry.Length > 1)
            {
                for (int i = 0; i < fileLinkAry.Length - 1; i++)
                    directory += fileLinkAry[i] + "//";
            }
            directory = directory.Remove(directory.Length - 1, 1);
        }
    }
}
