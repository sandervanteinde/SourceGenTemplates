namespace SourceGenTemplates.Parsing.Directives;

public class FileNameDirectiveNode(FileNameNode fileName) : DirectiveNode(DirectiveNodeType.Filename)
{
    public FileNameNode FileName => fileName;
}