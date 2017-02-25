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
            
            
            var originFileName = Request.Headers["X-File-Name"];
            var fileSize = int.Parse(Request.Headers["X-File-Size"]);
            var fileType = Request.Headers["X-File-Type"];
            var fileName = Path.GetFileName(originFileName);
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(),  fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            int uploaded = 0;
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                while (uploaded < fileSize)
                {
                    int length = 1;
                    var buffer = new byte[length];
                    int newBytes = Request.Body.Read(buffer, 0, length);
                    fileStream.Write(buffer, 0, length);
                    uploaded += newBytes;
                }
            }


            

            return Ok(new {filePath= filePath });
        }
    }
}
