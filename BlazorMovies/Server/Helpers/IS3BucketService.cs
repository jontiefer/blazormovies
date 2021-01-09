using Amazon.S3.Model;
using BlazorMovies.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public interface IS3BucketService
    {
        Task<List<BucketS3DTO>> ListBuckets();
        Task<List<ObjectS3DTO>> ListObjects(string bucketName);
    }
}
