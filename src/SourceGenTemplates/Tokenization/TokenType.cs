namespace SourceGenTemplates.Tokenization;

public enum TokenType
{
    SourceText,
    FileNameDirective,
    Identifier,
    For,
    EndFor,
    Number,
    DoubleDot,
    As,
    CodeContextSwitch,
    CodeContextEnd
}