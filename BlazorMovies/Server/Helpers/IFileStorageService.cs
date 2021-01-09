using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public interface IFileStorageService
    {
        Task<bool> DeleteFile(string fileName, string directory = null);
        Task<bool> DeleteFileByLink(string fileLink);
        Task<string> EditFile(byte[] content, string fileName, string directory = null, string prevFileLink = "");
        Task<string> EditFileByLink(byte[] content, string fileLink);
        Task<string> SaveFile(byte[] content, string fileName, string directory = null, bool appendHttp = true, bool appendHttps = false);        
    }
}
