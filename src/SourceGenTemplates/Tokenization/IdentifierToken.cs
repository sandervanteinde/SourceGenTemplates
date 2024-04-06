namespace SourceGenTemplates.Tokenization;

public class IdentifierToken : Token
{
    private IdentifierToken()
    {
    }

    public string Identifier { get; private set; } = null!;
    public override TokenType TokenType => TokenType.Identifier;

    public override int GetLength()
    {
        return Identifier.Length;
    }

    public static IdentifierToken Create(string identifier)
    {
        return new IdentifierToken
        {
            Identifier = identifier
        };
    }
}