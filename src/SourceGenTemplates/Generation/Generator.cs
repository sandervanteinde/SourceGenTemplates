using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

using SourceGenTemplates.Parsing;
using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Parsing.Foreach;

namespace SourceGenTemplates.Generation;

public class Generator(string fileName, FileNode file, GeneratorExecutionContext context, CompilationContext compilationContext)
{
    private readonly StringBuilder _sb = new();
    private readonly VariableContext _variables = new();

    public void AddToOutput()
    {
        GenerateBlocks(file.Blocks);
        OutputCurrentContentsToFile();
    }

    private void GenerateBlocks(IEnumerable<BlockNode> blocks)
    {
        foreach (var block in blocks)
        {
            Generate(block);
        }
    }

    private void Generate(BlockNode block)
    {
        _ = block.Type switch
        {
            BlockNodeType.CSharp => GenerateSourceText((CSharpBlockNode)block),
            BlockNodeType.Directive => GenerateDirectiveBlockNode(((DirectiveBlockNode)block).Directive),
            BlockNodeType.VariableInsertion => GenerateDirectiveBlockNode((VariableInsertionBlockNode)block)
        };
    }

    private bool GenerateSourceText(CSharpBlockNode node)
    {
        _sb.Append(node.SourceText.SourceText);
        return true;
    }

    private bool GenerateDirectiveBlockNode(VariableInsertionBlockNode node)
    {
        var value = _variables.GetOrThrow<object>(node.Identifier);

        if (value is null)
        {
            throw new ParserException($"Variable with name {node.Identifier.Identifier} was not defined", node.Identifier);
        }

        _sb.Append(value);
        return true;
    }

    private bool GenerateDirectiveBlockNode(DirectiveNode directive)
    {
        return directive.Type switch
        {
            DirectiveNodeType.Filename => GenerateFileNameDirectiveNode(((FileNameDirectiveNode)directive).FileName),
            DirectiveNodeType.ForI => GenerateForIDirectiveNode(((ForIDirectiveNode)directive).ForINode),
            DirectiveNodeType.Foreach => GenerateForeachDirectiveNode(((ForeachDirectiveNode)directive).ForeachNode)
        };
    }

    private bool GenerateFileNameDirectiveNode(FileNameNode node)
    {
        OutputCurrentContentsToFile();
        fileName = node.Identifier.Identifier.Identifier;
        return true;
    }

    private bool GenerateForeachDirectiveNode(ForeachNode node)
    {
        var identifier = node.Identifier;
        foreach (var @class in compilationContext.Classes)
        {
            using var variableContext = _variables.AddVariableToContext(identifier?.Identifier, @class);
            GenerateBlocks(node.Blocks);
        }
        return true;
    }

    private bool GenerateForIDirectiveNode(ForINode node)
    {
        var start = node.Range.StartRange.Number;
        var end = node.Range.EndRange.Number;

        for (var i = start; i < end; i++)
        {
            using var variableContext = _variables.AddVariableToContext(node.Identifier?.Identifier, i);
            GenerateBlocks(node.Blocks);
        }

        return true;
    }

    private void OutputCurrentContentsToFile()
    {
        if (_sb.Length > 0)
        {
            context.AddSource($"{fileName}.g.cs", _sb.ToString());
        }

        _sb.Clear();
    }
}