namespace SourceGenTemplates.Parsing;

public class FileNameNode(IdentifierNode identifier) : Node
{
    public IdentifierNode Identifier => identifier;
}