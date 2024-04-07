namespace SourceGenTemplates.Tokenization;

public class ForToken : Token
{
    public override TokenType TokenType => TokenType.For;

    public override int GetLength()
    {
        return 3;
    }
}