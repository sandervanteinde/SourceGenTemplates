namespace SourceGenTemplates.Parsing.Directives;

public class FileNameDirectiveNode(FileNameNode fileName) : DirectiveNode
{
    public FileNameNode FileName => fileName;
    public override DirectiveNodeType Type => DirectiveNodeType.Filename;
}