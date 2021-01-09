using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.DTOs
{
    public class BucketS3DTO
    {
        public string BucketName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
    