using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using BlazorMovies.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public class AWSS3BucketService : IS3BucketService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsS3BucketOptions _s3BucketOptions;
        private readonly IMapper _mapper;

        public AWSS3BucketService(IAmazonS3 s3Client, 
            AwsS3BucketOptions s3BucketOptions,
            IMapper mapper)
        {
            _s3Client = s3Client;
            _s3BucketOptions = s3BucketOptions;
            _mapper = mapper;
        }

        public async Task<List<BucketS3DTO>> ListBuckets()
        {
            try
            {
                var response = await _s3Client.ListBucketsAsync();

                if (response == null)
                    return null;

                var buckets = new List<S3Bucket>();
                foreach (S3Bucket bucket in response.Buckets)
                {
                    if (bucket != null)
                        buckets.Add(bucket);
                }

                return _mapper.Map<List<BucketS3DTO>>(buckets);
            }
            catch(AmazonServiceException err)
            {
                if (err.ErrorCode == "AccessDenied")
                {
                    Console.WriteLine("LOG: Access Denied to list S3 Buckets");
                    return null;
                }
                else
                    throw new Exception(err.Message);
            }
            catch(Exception err)
            {
                Console.WriteLine("LOG: Unknown error thrown when attempting to list buckets.\r\n" + err.ToString());
                return null;
            }
        }

        public async Task<List<ObjectS3DTO>> ListObjects(string bucketName)
        {
            try
            {
                var listRequest = new ListObjectsRequest()
                {
                    BucketName = bucketName
                };

                var listResponse = await _s3Client.ListObjectsAsync(listRequest);

                if (listResponse == null)
                    return null;

                var objects = new List<S3Object>();
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    objects.Add(obj);
                }

                return _mapper.Map<List<ObjectS3DTO>>(objects);
            }
            catch (AmazonServiceException err)
            {
                if (err.ErrorCode == "AccessDenied")
                {
                    Console.WriteLine($"LOG: Access Denied to list S3 Bucket ({bucketName}) Objects");
                    return null;
                }
                else
                    throw new Exception(err.Message);
            }
            catch (Exception err)
            {
                Console.WriteLine("LOG: Unknown error thrown when attempting to list S3 bucket objects.\r\n" + err.ToString());
                return null;
            }
        }
    }
}
