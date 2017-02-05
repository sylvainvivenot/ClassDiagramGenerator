using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClassDiagramGeneratorWeb.Controllers
{
    public class FileUploadController : Controller
    { 
        [HttpPost]
        [Route("Api/UploadFile")]
        public IActionResult Post()
        {
            int length = (int)(Request.ContentLength ?? 0);
            var bytes = new byte[length];
            var originFileName = Request.Headers["X-File-Name"];
            var fileSize = Request.Headers["X-File-Size"];
            var fileType = Request.Headers["X-File-Type"];
            var fileName = Path.GetFileName(originFileName);
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(),  fileName);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            fileStream.Write(bytes, 0, length);
            fileStream.Close();

            return Ok(new {filePath= filePath });
        }
    }
}
