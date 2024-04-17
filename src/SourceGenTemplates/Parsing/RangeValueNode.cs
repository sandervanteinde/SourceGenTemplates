using SourceGenTemplates.Generation;

namespace SourceGenTemplates.Parsing;

public abstract class RangeValueNode : Node
{
    public abstract int GetNumericValue(VariableContext variables);
}