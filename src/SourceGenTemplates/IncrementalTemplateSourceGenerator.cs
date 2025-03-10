using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates;

[Generator(LanguageNames.CSharp)]
public class IncrementalTemplateSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var templateProviders = context.AdditionalTextsProvider
            .Where(static text => text.Path.EndsWith(".cstempl") && text.GetText() is { Length: > 0 });

        var fullCompilationProvider = context.CompilationProvider
            .Select((compilation, ct) =>
            {
                var dataCollector = new CompilationDataCollector();

                foreach (var syntaxTree in compilation.SyntaxTrees)
                {
                    dataCollector.Visit(syntaxTree.GetRoot());
                }

                return dataCollector.CreateCompilationContext(compilation);
            }
        );
        var combine = templateProviders
            .Combine(fullCompilationProvider)
            .Select(
                static (pair, ct) =>
                {
                    var (text, compilation) = pair;

                    var tokenizer = new Tokenizer(text.GetText()!.ToString());
                    var initialFileName = Path.GetFileNameWithoutExtension(text.Path);
                    var parser = new Parser(tokenizer);
                    var file = parser.ParseFileNode();
                    return (initialFileName, file, compilation);
               });

        context.RegisterSourceOutput(combine, static (context, data) =>
        {
            var (initialFileName, file, compilation) = data;
            var generator = new Generator(initialFileName, file, context, compilation);
            generator.AddToOutput();
        });
    }
}

public class CompilationDataCollector : CSharpSyntaxWalker
{
    private readonly List<ClassDeclarationSyntax> _classes = [];

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        _classes.Add(node);
    }

    public CompilationContext CreateCompilationContext(Compilation compilation)
    {
        return new CompilationContext(compilation, _classes);
    }

}