using System.Text;

namespace SourceGenTemplates.Parsing;

public abstract class Node
{
    protected internal abstract void AppendDebugString(StringBuilder sb);

    public sealed override string ToString()
    {
        var sb = new StringBuilder();
        AppendDebugString(sb);
        return sb.ToString();
    }
}