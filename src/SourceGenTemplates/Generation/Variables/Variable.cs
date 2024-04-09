using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable(VariableKind kind)
{
    public VariableKind Kind => kind;

    public abstract string GetCodeRepresentation();

    public Variable AccessProperty(IdentifierToken identifier)
    {
        return TryAccessProperty(identifier)
            ?? throw new ParserException($"Cannot access property {identifier.Identifier} of variable of type {kind}", identifier);
    }

    protected abstract Variable? TryAccessProperty(IdentifierToken identifier);
}

public class ValueVariable(object value) : Variable(VariableKind.Value)
{
    public override string GetCodeRepresentation()
    {
        return value.ToString();
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}

public class ClassVariable(ClassDeclarationSyntax @class) : Variable(VariableKind.Class)
{
    public override string GetCodeRepresentation()
    {
        return @class.Identifier.ToString();
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return identifier.Identifier == "Namespace"
            ? new NamespaceVariable(FindNamespace())
            : null;
    }

    private BaseNamespaceDeclarationSyntax FindNamespace()
    {
        var currentParent = @class.Parent;

        while (currentParent is not null)
        {
            if (currentParent is BaseNamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration;
            }
            currentParent = currentParent.Parent;
        }
        throw new NotImplementedException();
    }
}

public class NamespaceVariable(BaseNamespaceDeclarationSyntax @namespace) : Variable(VariableKind.Namespace)
{
    public override string GetCodeRepresentation()
    {
        return @namespace.Name.ToFullString();
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}

public enum VariableKind
{
    Value,
    Class,
    Namespace
}