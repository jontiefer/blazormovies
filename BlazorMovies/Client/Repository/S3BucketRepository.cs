using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class S3BucketRepository : IS3BucketRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/s3";

        public S3BucketRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }        

        public async Task<List<BucketS3DTO>> ListBuckets()
        {
            var response = await _httpService.Get<List<BucketS3DTO>>($"{url}/listbuckets");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<List<ObjectS3DTO>> ListObjects(string bucketName)
        {
            var response = await _httpService.Get<List<ObjectS3DTO>>($"{url}/listobjects/{bucketName}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
    }
}
