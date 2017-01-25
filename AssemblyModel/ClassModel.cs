using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyModel
{
    public class SolutionModel
    {
        public ClassModel[] Classes { get; set; }
        public LinkModel[] Links { get; set; }
    }

    public class ClassModelMother { }

    public class ClassModel:ClassModelMother
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public PropertyModel[] Properties { get; set; }
        public MethodModel[] Methods { get; set; }
    }

    public class MethodModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ParameterModel[] Parameters { get; set; }
        public string Visibility { get; set; }
    }

    public class ParameterModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
    }

    public class PropertyModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Visibility { get; set; }
    }

    public class LinkModel
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Relationship { get; set; }
    }
}
