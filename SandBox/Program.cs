using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyModel;
using Microsoft.CodeAnalysis;
using Modelizer;
using Newtonsoft.Json;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            var solutionModelBuilder = new SolutionModelBuilder(@"C:\Users\sylva\Documents\visual studio 2017\Projects\Modelizer\Modelizer.sln");
            SolutionModel solutionModel = solutionModelBuilder.Build();
            File.WriteAllText("output.json", JsonConvert.SerializeObject(solutionModel).ToString());
            Console.WriteLine("Stop");
            Console.ReadKey();
        }
    }
}
