using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class CSharpBlockNode(SourceTextToken sourceText) : BlockNode
{
    public SourceTextToken SourceText => sourceText;
    public override BlockNodeType Type => BlockNodeType.CSharp;
}