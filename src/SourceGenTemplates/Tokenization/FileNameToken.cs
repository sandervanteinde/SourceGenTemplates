namespace SourceGenTemplates.Tokenization;

public class FileNameToken : Token
{
    public override TokenType TokenType => TokenType.FileNameDirective;

    public override int GetLength()
    {
        return 8;
    }
}