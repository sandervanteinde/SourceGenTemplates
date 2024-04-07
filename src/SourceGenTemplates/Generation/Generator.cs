using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using SourceGenTemplates.Parsing;

namespace SourceGenTemplates.Generation;

public class Generator(string fileName, FileNode file, GeneratorExecutionContext context)
{
    private readonly StringBuilder _sb = new();

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
        switch (block)
        {
            case CSharpBlockNode csharp:
                _sb.Append(csharp.SourceText.SourceText);
                break;
            case DirectiveBlockNode directive:
                GenerateDirectiveBlockNode(directive.Directive);
                break;
            default:
                throw new NotSupportedException($"Unknown block node: {block.GetType()}");
        }
    }

    private void GenerateDirectiveBlockNode(DirectiveNode directive)
    {
        switch (directive)
        {
            case FileNameDirectiveNode fileNameDirective:
                OutputCurrentContentsToFile();
                fileName = fileNameDirective.FileName.Identifier.Identifier.Identifier;
                break;
            case ForIDirectiveNode forIDirective:
                var node = forIDirective.ForINode;
                var start = node.Range.StartRange.Number;
                var end = node.Range.EndRange.Number;

                for (var i = start; i < end; i++)
                {
                    GenerateBlocks(node.Blocks);
                }

                break;
        }
    }

    private void OutputCurrentContentsToFile()
    {
        if (_sb.Length > 0)
        {
            // context.AddSource($"{fileName}.g.cs", _sb.ToString());
        }

        _sb.Clear();
    }
}