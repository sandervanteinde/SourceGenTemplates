namespace SourceGenTemplates.Tokenization;

public class FileNameToken : Token
{
    private FileNameToken()
    {
    }

    public override TokenType TokenType => TokenType.FileNameDirective;

    public override int GetLength()
    {
        return 8;
    }

    public static FileNameToken Create()
    {
        return new FileNameToken();
    }
}