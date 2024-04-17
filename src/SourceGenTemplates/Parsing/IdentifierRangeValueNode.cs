using System.Text;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Generation.Variables;

namespace SourceGenTemplates.Parsing;

public class IdentifierRangeValueNode(IdentifierNode identifierNode) : RangeValueNode
{
    protected internal override void AppendDebugString(StringBuilder sb)
    {
        identifierNode.AppendDebugString(sb);
    }

    public override int GetNumericValue(VariableContext variables)
    {
        var variable = variables.GetOrThrow(identifierNode.Identifier);

        if (variable is not IntegerVariable integerVariable)
        {
            throw new ParserException("Expected an integer variable as range", identifierNode.Identifier);
        }

        return integerVariable.Integer;
    }
}