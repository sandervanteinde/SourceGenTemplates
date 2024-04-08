using SourceGenTemplates.Parsing.Expressions;

namespace SourceGenTemplates.Parsing;

public class FileNameNode(ExpressionNode expression) : Node
{
    public ExpressionNode Expression => expression;
}