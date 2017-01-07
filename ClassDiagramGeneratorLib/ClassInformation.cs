using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Modelizer
{
    public class ClassInformation
    {
        private readonly ITypeSymbol _typeSymbol;

        public ClassInformation(ITypeSymbol typeSymbol)
        {
            _typeSymbol = typeSymbol;
        }

        public IEnumerable<IFieldSymbol> GetFields()
        {
            return _typeSymbol.GetMembers().ToList().Where(x => x.Kind == SymbolKind.Field && !x.IsImplicitlyDeclared).Select(x => (IFieldSymbol)x);
        }

        public IEnumerable<IPropertySymbol> GetProperties()
        {
            return _typeSymbol.GetMembers().ToList().Where(x => x.Kind == SymbolKind.Property && !x.IsImplicitlyDeclared).Select(x => (IPropertySymbol)x);
        }

        public IEnumerable<IMethodSymbol> GetMethods()
        {
            return _typeSymbol.GetMembers().ToList().Where(x => x.Kind == SymbolKind.Method && !x.IsImplicitlyDeclared).Select(x => (IMethodSymbol)x).Where(x => x.MethodKind == MethodKind.Ordinary);
        }
    }
}