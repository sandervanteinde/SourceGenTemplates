using System.Text;

namespace SourceGenTemplates.Parsing.Directives;

public class FileNameDirectiveNode(FileNameNode fileName) : DirectiveNode(DirectiveNodeType.Filename)
{
    public FileNameNode FileName => fileName;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("filename");
    }
}