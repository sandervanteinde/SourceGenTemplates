namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable
{
    public abstract VariableKind Kind { get; }
}

public class ValueVariable : Variable
{
    public requir object Value { get; init; }

    public override VariableKind Kind => VariableKind.Value;
}

public class ClassVariable : Variable
{
    public override VariableKind Kind => VariableKind.Class;
}

public enum VariableKind
{
    Value,
    Class
}