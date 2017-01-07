using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace Modelizer
{
    public class SolutionReader
    {
        private readonly string _solutionPath;
        private readonly string _compilationName;

        public List<string> DocumentsPaths { get; }
        public List<ITypeSymbol> TypeSymbols { get; }

        public SolutionReader(string solutionPath)
        {
            _solutionPath = solutionPath;
            DocumentsPaths = GenerateDocumentsPaths().ToList();
            _compilationName = "MyCompilation";
            TypeSymbols = GetTypeSymbols(GetSyntaxTrees(DocumentsPaths)).ToList();
        }



        private IEnumerable<string> GenerateDocumentsPaths()
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(_solutionPath).Result;
            foreach (var project in solution.Projects)
            {
                string projectPath = Path.GetDirectoryName(project.FilePath);
                foreach (var document in project.Documents)
                {
                    string documentPath = Path.GetDirectoryName(document.FilePath);
                    if (projectPath == documentPath)
                    {
                        yield return document.FilePath;
                    }
                }
            }
        }

        private SyntaxTree[] GetSyntaxTrees(List<string> documentsPaths)
        {
            SyntaxTree[] trees = new SyntaxTree[documentsPaths.Count];
            for (int i = 0; i < documentsPaths.Count; i++)
            {
                trees[i] = GetTreeFromDocument(documentsPaths[i]);
            }
            return trees;
        }

        private SyntaxTree GetTreeFromDocument(string documentPath)
        {
            string source = File.ReadAllText(documentPath);
            return CSharpSyntaxTree.ParseText(source, path: documentPath);
        }



        private IEnumerable<ITypeSymbol> GetTypeSymbols(SyntaxTree[] syntaxTrees)
        {
            var mscorLib = MetadataReference.CreateFromFile((typeof(object).Assembly.Location));
            var compilation = CSharpCompilation.Create(_compilationName, syntaxTrees: syntaxTrees, references: new[] { mscorLib });
            IEnumerable<INamespaceSymbol> namespaceMembers = compilation.GlobalNamespace.GetNamespaceMembers();
            return namespaceMembers.SelectMany(x => x.GetMembers())
                .Where(x => x.IsType && x.ContainingAssembly.Identity.Name == _compilationName)
                .Select(x => (ITypeSymbol)x);
        }
    }
}
