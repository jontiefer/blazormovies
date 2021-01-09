using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3Controller : ControllerBase
    {
        private readonly IS3BucketService _bucketService;

        public S3Controller(IS3BucketService bucketService)
        {
            _bucketService = bucketService;
        }

        [HttpGet("listbuckets")]
        public async Task<ActionResult<List<BucketS3DTO>>> ListBuckets()
        {
            var buckets = await _bucketService.ListBuckets();

            return buckets;
        }

        [HttpGet("listobjects/{bucketName}")]
        public async Task<ActionResult<List<ObjectS3DTO>>> ListObjects(string bucketName)
        {
            var objects = await _bucketService.ListObjects(bucketName);

            return objects;
        }
    }
}
