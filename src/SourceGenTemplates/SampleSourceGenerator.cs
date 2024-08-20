using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates;

[Generator]
public class SampleSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // If you would like to put some data to non-compilable file (e.g. a .txt file), mark it as an Additional File.

        // Go through all files marked as an Additional File in file properties.
        var additionalFiles = context.AdditionalFiles
            .Where(file => file is not null && Path.GetExtension(file.Path) == ".cstempl");

        foreach (var additionalFile in additionalFiles)
        {
            var sourceText = additionalFile.GetText();

            if (sourceText is null)
            {
                continue;
            }

            try
            {
                var tokenizer = new Tokenizer(sourceText.ToString());
                var initialFileName = Path.GetFileNameWithoutExtension(additionalFile.Path);
                var parser = new Parser(tokenizer);
                var file = parser.ParseFileNode();
                var generator = new Generator(initialFileName, file, context, ((SyntaxReceiver)context.SyntaxContextReceiver!).CompilationContext);
                generator.AddToOutput();
            }
            catch (ParserException exception)
            {
#if DEBUG
                const DiagnosticSeverity severity = DiagnosticSeverity.Warning;
#else
                const DiagnosticSeverity severity = DiagnosticSeverity.Error;
#endif
                var diagnosticDescriptor = new DiagnosticDescriptor(
                    "sourcegentemplates001", "Invalid expression", "Parse error: {0}", "Design", severity, isEnabledByDefault: true
                );
                var diagnostic = Diagnostic.Create(
                    diagnosticDescriptor,
                    Location.Create(additionalFile.Path, new TextSpan(start: 0, sourceText.Length), exception.LinePosition),
                    exception.Message
                );
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}