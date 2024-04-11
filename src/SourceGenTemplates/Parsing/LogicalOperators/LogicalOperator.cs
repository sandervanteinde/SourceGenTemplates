namespace SourceGenTemplates.Parsing.LogicalOperators;

public abstract class LogicalOperator(LogicalOperatorType type) : Node
{
    public LogicalOperatorType Type => type;
    public abstract int Precedence { get; }
}