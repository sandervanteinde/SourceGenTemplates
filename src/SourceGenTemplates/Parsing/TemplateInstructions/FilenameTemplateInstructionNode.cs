using System.Text;

namespace SourceGenTemplates.Parsing.TemplateInstructions;

public class FilenameTemplateInstructionNode(FileNameNode fileNameNode) : TemplateInstructionNode(TemplateInstructionNodeType.Filename)
{
    public FileNameNode FileName { get; } = fileNameNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        fileNameNode.AppendDebugString(sb);
    }
}