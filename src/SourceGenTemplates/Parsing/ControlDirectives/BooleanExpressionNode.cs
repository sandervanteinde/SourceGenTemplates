namespace SourceGenTemplates.Parsing.ControlDirectives;

public abstract class BooleanExpressionNode(BooleanExpressionType type) : Node
{
    public BooleanExpressionType ExpressionType => type;
}