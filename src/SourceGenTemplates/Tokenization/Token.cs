namespace SourceGenTemplates.Tokenization;

public abstract class Token
{
    public abstract TokenType TokenType { get; }
    public abstract int GetLength();
}

public class CodeContextToken : Token
{
    public override TokenType TokenType => TokenType.CodeContextSwitch;
    public override int GetLength()
    {
        return 2;
    }
}

public class CodeContextEndToken : Token 
{
    public override TokenType TokenType => TokenType.CodeContextEnd;
    public override int GetLength()
    {
        return 1;
    }
}