namespace SourceGenTemplates.Tokenization;

public class EndForToken : Token
{
    public override TokenType TokenType => TokenType.EndFor;

    public override int GetLength()
    {
        return 6;
    }
}

public class AsToken : Token
{
    public override TokenType TokenType => TokenType.As;

    public override int GetLength()
    {
        return 2;
    }
}