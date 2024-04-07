namespace SourceGenTemplates.Tokenization;

public class NumberToken(int number) : Token
{
    public override TokenType TokenType => TokenType.Number;
    public int Number => number;

    public override int GetLength()
    {
        return number.ToString()
            .Length;
    }
}