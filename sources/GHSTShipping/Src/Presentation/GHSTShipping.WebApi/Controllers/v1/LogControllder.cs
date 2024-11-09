using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    public class FileLogDto
    {
        public string Name { get; set; }
        public long Size { get; set; }
    }

    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogController : BaseApiController
    {

        private readonly string _logDirectoryPath = "logs";

        [HttpGet("files")]
        [Authorize]
        public async Task<BaseResult<List<FileLogDto>>> GetLogFiles()
        {
            if (!Directory.Exists(_logDirectoryPath))
            {
                return BaseResult<List<FileLogDto>>.Failure();
            }

            var logFiles = Directory.GetFiles(_logDirectoryPath, "*.txt")
                                     .Select(f => new FileLogDto
                                     {
                                         Name = Path.GetFileName(f),
                                         Size = new FileInfo(f).Length
                                     })
                                     .ToList();

            return BaseResult<List<FileLogDto>>.Ok(logFiles);
        }

        [HttpGet("read/{fileName}")]
        [Authorize]
        public async Task<BaseResult<string>> ReadLogFile(string fileName)
        {
            var filePath = Path.Combine(_logDirectoryPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return BaseResult<string>.Failure();
            }

            var logContent = System.IO.File.ReadAllText(filePath);
            return BaseResult<string>.Ok(logContent);
        }

        [HttpDelete("clear/{fileName}")]
        [Authorize]
        public async Task<BaseResult> ClearLogFile(string fileName)
        {
            var filePath = Path.Combine(_logDirectoryPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.WriteAllText(filePath, string.Empty);
                //return Ok("Log file cleared.");
                return BaseResult.Ok();
            }

            return BaseResult.Failure();
        }
    }
}
