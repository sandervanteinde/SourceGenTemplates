using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenTemplates;

public class CompilationContext
{
    private readonly List<ClassDeclarationSyntax> classes = new();

    public IReadOnlyCollection<ClassDeclarationSyntax> Classes => classes;

    public void AddClass(ClassDeclarationSyntax @class)
    {
        classes.Add(@class);
    }
}