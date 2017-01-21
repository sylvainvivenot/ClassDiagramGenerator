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
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null)
            {
                return NotFound();
            }
            var fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(Path.GetTempPath(),  fileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok(new {filePath=fileName});
        }
    }
}
