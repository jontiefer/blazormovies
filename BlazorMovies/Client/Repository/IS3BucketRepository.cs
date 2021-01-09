using BlazorMovies.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IS3BucketRepository
    {
        Task<List<BucketS3DTO>> ListBuckets();
        Task<List<ObjectS3DTO>> ListObjects(string bucketName);
    }
}
