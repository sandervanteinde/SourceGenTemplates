using SourceGenTemplates.Parsing.Directives;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public abstract class BooleanExpressionNode(BooleanExpressionType type) : DirectiveNode(DirectiveNodeType.If)
{
    public BooleanExpressionType ExpressionType => type;
}