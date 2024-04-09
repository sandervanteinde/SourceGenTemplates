using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable
{
    public abstract VariableKind Kind { get; }

    public abstract string GetCodeRepresentation();
}

public class ValueVariable(object value) : Variable
{
    public override VariableKind Kind => VariableKind.Value;

    public override string GetCodeRepresentation()
    {
        return value.ToString();
    }
}

public class ClassVariable(ClassDeclarationSyntax @class) : Variable
{
    public override VariableKind Kind => VariableKind.Class;

    public override string GetCodeRepresentation()
    {
        return @class.Identifier.ToString();
    }
}

public enum VariableKind
{
    Value,
    Class
}