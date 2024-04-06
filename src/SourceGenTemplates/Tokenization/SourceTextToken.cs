namespace SourceGenTemplates.Tokenization;

public class SourceTextToken : Token
{
    private SourceTextToken()
    {
    }

    public override TokenType TokenType => TokenType.SourceText;

    public string SourceText { get; private set; } = null!;

    public override int GetLength()
    {
        return SourceText.Length;
    }

    public static SourceTextToken Create(string sourceText)
    {
        return new SourceTextToken { SourceText = sourceText };
    }
}