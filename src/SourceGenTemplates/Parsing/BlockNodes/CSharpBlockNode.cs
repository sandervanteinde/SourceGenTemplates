using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class CSharpBlockNode(SourceTextToken sourceText) : BlockNode(BlockNodeType.CSharp)
{
    public SourceTextToken SourceText => sourceText;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(sourceText.SourceText);
    }
}