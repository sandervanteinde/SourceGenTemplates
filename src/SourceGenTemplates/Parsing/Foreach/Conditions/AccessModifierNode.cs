using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class AccessModifierNode(AccessModifierType type) : Node
{
    public bool IsApplicableFor(in SyntaxTokenList syntaxTokenList)
    {
        return type switch
        {
            AccessModifierType.Public => syntaxTokenList.Any(SyntaxKind.PublicKeyword),
            AccessModifierType.Private => syntaxTokenList.Any(SyntaxKind.PrivateKeyword),
            AccessModifierType.Protected => syntaxTokenList.Any(SyntaxKind.ProtectedKeyword),
            AccessModifierType.Internal => syntaxTokenList.Any(SyntaxKind.InternalKeyword),
            AccessModifierType.ProtectedInternal => syntaxTokenList.Any(SyntaxKind.ProtectedKeyword) && syntaxTokenList.Any(SyntaxKind.InternalKeyword),
            AccessModifierType.PrivateProtected => syntaxTokenList.Any(SyntaxKind.PrivateKeyword) && syntaxTokenList.Any(SyntaxKind.ProtectedKeyword)
        };
    }
}