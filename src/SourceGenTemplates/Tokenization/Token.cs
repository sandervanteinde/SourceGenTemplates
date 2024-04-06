namespace SourceGenTemplates.Tokenization;

public abstract class Token
{
    public abstract TokenType TokenType { get; }
    public abstract int GetLength();
}