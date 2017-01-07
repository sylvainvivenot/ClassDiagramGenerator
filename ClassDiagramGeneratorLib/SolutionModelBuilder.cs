using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyModel;
using Microsoft.CodeAnalysis;

namespace Modelizer
{
    public class SolutionModelBuilder
    {
        private readonly SolutionReader _solutionReader;

        public SolutionModelBuilder(string solutionPath)
        {
            _solutionReader = new SolutionReader(solutionPath);
        }

        public SolutionModel Build()
        {
            SolutionModel solution = new SolutionModel {Classes = GenerateClassModels(), Links = GenerateLinkModels()};
            return solution;
        }

        private LinkModel[] GenerateLinkModels()
        {
            List<LinkModel> linkModel = new List<LinkModel>();
            ProcessGeneralization(
                (type, baseType) =>
                {
                    linkModel.Add(new LinkModel{ From = type, To = baseType, Relashionship = "generalization"});
                });
            ProcessAggregation(
                (type, baseType) =>
                {
                    linkModel.Add(new LinkModel { From = type, To = baseType, Relashionship = "aggregation" });
                });
            return linkModel.ToArray();
        }

        public void ProcessGeneralization(Action<int, int> addLink)
        {
            var typeSymbols = _solutionReader.TypeSymbols;
            int indexOfType = 0;
            foreach (var typeSymbol in typeSymbols)
            {
                INamedTypeSymbol typeSymbolBaseType = typeSymbol.BaseType;
                if (typeSymbolBaseType != null && typeSymbolBaseType.Name != "Object")
                {
                    if (typeSymbols.Contains(typeSymbolBaseType))
                    {
                        int indexOfBaseType = typeSymbols.IndexOf(typeSymbolBaseType);
                        addLink(indexOfType, indexOfBaseType);
                    }
                }
                indexOfType++;
            }
        }

        public void ProcessAggregation(Action<int, int> addLink)
        {
        }


        private ClassModel[] GenerateClassModels()
        {
            var typeSymbols = _solutionReader.TypeSymbols.ToList(); 
            ClassModel[] classModels = new ClassModel[typeSymbols.Count];
            for (int i = 0; i < classModels.Length; i++)
            {
                classModels[i] = new ClassModel()
                {
                    Key=i,
                    Name = typeSymbols[i].Name,
                    Methods = GenerateMethodModels(typeSymbols[i]),
                    Properties = GeneratePropertyModels(typeSymbols[i])
                };
            }
            return classModels;
        }

        private PropertyModel[] GeneratePropertyModels(ITypeSymbol typeSymbol)
        {
            var classInformation = new ClassInformation(typeSymbol);
            List<IFieldSymbol> fields = classInformation.GetFields().ToList();
            List<IPropertySymbol> properties = classInformation.GetProperties().ToList();
            PropertyModel[] propertyModels = new PropertyModel[fields.Count+properties.Count];
            for (int i = 0; i < fields.Count; i++)
            {
                propertyModels[i] = new PropertyModel()
                {
                    Name = fields[i].Name,
                    Type = TypeName(fields[i].Type),
                    Visibility = GetAccessibilityName(fields[i].DeclaredAccessibility)
                };
            }
            for (int i = 0; i <  properties.Count; i++)
            {
                propertyModels[fields.Count + i] = new PropertyModel()
                {
                    Name = properties[i].Name,
                    Type = TypeName(properties[i].Type),
                    Visibility = GetAccessibilityName(properties[i].DeclaredAccessibility)
                };
            }
            return propertyModels;
        }

        private static string TypeName(ITypeSymbol type)
        {
            return type.TypeKind != TypeKind.Array ? type.Name : ((IArrayTypeSymbol) type).ElementType.Name + "[]";
        }

        private string GetAccessibilityName(Accessibility declaredAccessibility)
        {
            return declaredAccessibility.ToString();
        }

        private MethodModel[] GenerateMethodModels(ITypeSymbol typeSymbol)
        {
            var classInformation = new ClassInformation(typeSymbol);
            List<IMethodSymbol> methods = classInformation.GetMethods().ToList();
            MethodModel[] methodModels = new MethodModel[methods.Count];
            for (int i = 0; i < methods.Count; i++)
            {
                methodModels[i] = new MethodModel()
                {
                    Name = methods[i].Name,
                    Parameters = GeneratePrameterModels(methods[i]),
                    Visibility = GetAccessibilityName(methods[i].DeclaredAccessibility),
                    Type= TypeName(methods[i].ReturnType)
                };
            }

            return methodModels;
        }

        private ParameterModel[] GeneratePrameterModels(IMethodSymbol symbol)
        {
            var parameterSymbols = symbol.Parameters.ToList();
            ParameterModel[] parameterModels = new ParameterModel[parameterSymbols.Count];
            for (int i = 0; i < parameterSymbols.Count; i++)
            {
                parameterModels[i] = new ParameterModel()
                {
                    Name = parameterSymbols[i].Name,
                    Type = TypeName(parameterSymbols[i].Type),
                    Default = parameterSymbols[i].HasExplicitDefaultValue?parameterSymbols[i].ExplicitDefaultValue.ToString():""
                };
            }
            return parameterModels;
        }
    }
}
