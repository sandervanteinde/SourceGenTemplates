namespace SourceGenTemplates.Tokenization;

public class IdentifierToken(string identifier) : Token
{
    public string Identifier => identifier;
    public override TokenType TokenType => TokenType.Identifier;

    public override int GetLength()
    {
        return Identifier.Length;
    }
}