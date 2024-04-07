namespace SourceGenTemplates.Tokenization;

public class DoubleDotToken : Token
{
    public override TokenType TokenType => TokenType.DoubleDot;

    public override int GetLength()
    {
        return 2;
    }
}