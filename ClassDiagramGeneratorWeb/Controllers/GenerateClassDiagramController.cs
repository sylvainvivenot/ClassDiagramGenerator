﻿using System.IO;
using AssemblyModel;
using Microsoft.AspNetCore.Mvc;
using Modelizer;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClassDiagramGeneratorWeb.Controllers
{
    public class SolutionDescription
    {
        public string ArchivePath { get; set; }
        public string SlnFile { get; set; }
    }
    /// <summary>
    /// 
    /// 
    /// </summary>
    [Route("Api/ClassDiagram/")]
    public class GenerateClassDiagramController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
       
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="solutionFile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetClassDiagram([FromBody] SolutionDescription solution)
        {
            string filePath = Path.Combine(Path.GetTempPath(), solution.ArchivePath);
            string extractionFolder = UnZipSolution(filePath);

            string[] solutionFiles = Directory.GetFiles(extractionFolder, solution.SlnFile,SearchOption.AllDirectories);
            if (solutionFiles.Length == 0)
            {
                return Json(new { Success = false, Message = "no sln file found" });
            }
            var solutionModel = GetSolutionModel(extractionFolder, solutionFiles[0]);
            return Ok(solutionModel);
        }

        private SolutionModel GetSolutionModel(string extractionFolder, string solutionFile)
        {
            string solutionPath = Path.Combine(extractionFolder, solutionFile);
            var solutionModelBuilder = new SolutionModelBuilder(solutionPath);
            var solutionModel = solutionModelBuilder.Build();
            return solutionModel;
        }

        private string UnZipSolution(string fileName)
        {
            string filePath = Path.Combine(Path.GetTempPath(), fileName);
            string extractionFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(fileName));

            if (!Directory.Exists(extractionFolder))
            {
                Directory.CreateDirectory(extractionFolder);
                System.IO.Compression.ZipFile.ExtractToDirectory(filePath, extractionFolder);
            }
            return extractionFolder;
        }
    }
}
