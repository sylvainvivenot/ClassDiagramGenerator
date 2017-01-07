using System.IO;
using System.Linq;
using Modelizer;
using NFluent;
using NUnit.Framework;

namespace ClassDiagramGeneratorLib.Test
{
    public class SolutionModelBuilderTests
    {
        [Test]
        public void Should_process_2_projects_when_solution_contains_2_projects()
        {
            string solution = @"SolutionWith2Projects";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            Check.That(solutionModel.Classes.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_model_contain_2_classes_when_solution_contains_1_project_with_2_classes()
        {
            string solution = @"ProjectWith2Files2Classes";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            Check.That(solutionModel.Classes.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_class_model_contain_2_privates_attributes_when_solution_contains_1_class_with_2_private_attributes()
        {
            string solution = @"OneClass2PrivatesAttributes";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var privateProperties = from p in solutionModel.Classes.First().Properties where p.Visibility == "Private" select p;
            Check.That(privateProperties.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_class_model_contain_2_publics_properties_when_solution_contains_1_class_with_2_public_properties()
        {
            string solution = @"OneClassWith2PublicProperties";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var publicProperties = from p in solutionModel.Classes.First().Properties where p.Visibility == "Public" select p;
            Check.That(publicProperties.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_class_model_contain_2_private_methods_when_solution_contains_1_class_with_2_private_methods()
        {
            string solution = @"OneClassWith2PrivateMethods";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var privateMethods = from m in solutionModel.Classes.First().Methods where m.Visibility=="Private"  select m;
            Check.That(privateMethods.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_class_model_contain_2_public_methods_when_solution_contains_1_class_with_2_public_methods()
        {
            string solution = @"OneClass2PublicMethods";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var publicMethods = from m in solutionModel.Classes.First().Methods where m.Visibility == "Public" select m;
            Check.That(publicMethods.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_class_model_contain_2_methods_with_arguments_when_solution_contains_1_class_with_2_methods_with_arguments()
        {
            string solution = @"OneClass2MEthodsWithArguments";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var methods = from m in solutionModel.Classes.First().Methods where m.Parameters.Any() select m;
            Check.That(methods.Count()).Equals(2);
            Clean(solution);
        }

        [Test]
        public void Should_create_an_inheritance_link_between_class1_and_class2_when_class1_inherits_from_class2()
        {
            string solution = @"Class1InheritFromClass2";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var link = solutionModel.Links.First();

            Check.That(link.From).IsEqualTo(0);
            Check.That(link.To).IsEqualTo(1);
            Check.That(link.Relashionship).IsEqualTo("generalization");
            Clean(solution);
        }

        [Test]
        public void Should_create_an_aggregation_link_between_class1_and_class2_when_class1_inherits_fromis_used_in_class2()
        {
            string solution = @"Class1UsedInClass2";
            var solutionModelBuilder = CreateSolutionModelBuilder(solution);
            var solutionModel = solutionModelBuilder.Build();
            var link = solutionModel.Links.First();

            Check.That(link.From).IsEqualTo(0);
            Check.That(link.To).IsEqualTo(1);
            Check.That(link.Relashionship).IsEqualTo("aggregation");
            Clean(solution);
        }

        private static SolutionModelBuilder CreateSolutionModelBuilder(string solution)
        {
            if (!Directory.Exists(solution))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory($@".\DataForTests\{solution}.zip", @".\");
            }
            var solutionModelBuilder = new SolutionModelBuilder($@".\{solution}\{solution}.sln");
            return solutionModelBuilder;
        }


        private static void Clean(string solution)
        {
            Directory.Delete($@".\{solution}\",true);
        }
    }
}
