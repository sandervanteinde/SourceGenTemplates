using System.Text;
using SourceGenTemplates.Parsing.VariableExpressions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public class HasAttributeBooleanExpressionNode(VariableExpressionNode variableExpressionNode, StringToken @string)
    : BooleanExpressionNode(BooleanExpressionType.HasAttribute)
{
    public StringToken String => @string;
    public VariableExpressionNode VariableExpression => variableExpressionNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        variableExpressionNode.AppendDebugString(sb);
        sb.Append(" has_attribute ");
        sb.Append($"\"{@string.Value}\"");
    }
}