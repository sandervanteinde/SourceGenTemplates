using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenTemplates;

public class CompilationContext(Compilation contextSemanticModel, IReadOnlyCollection<ClassDeclarationSyntax> classes)
{
    public IReadOnlyCollection<ClassDeclarationSyntax> Classes => classes;

    public SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
    {
        return contextSemanticModel.GetSemanticModel(syntaxTree);
    }
}