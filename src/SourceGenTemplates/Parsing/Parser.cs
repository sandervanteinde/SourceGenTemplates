using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class Parser(Tokenizer tokenizer, string initialFileName)
{
    private readonly ParseResult _parseResult = new();
    private readonly StringBuilder _sb = new();
    private string _fileName = initialFileName;

    public bool TryParse(out ParseResult? result, out string? diagnosticError)
    {
        while (tokenizer.TryPeek(out var token))
        {
            diagnosticError = token!.TokenType switch
            {
                TokenType.SourceText => TryConsume((SourceTextToken)token),
                TokenType.FileNameDirective => TryConsume((FileNameToken)token),
                TokenType.Identifier => $"Unknown identifier {((IdentifierToken)token).Identifier}"
            };

            if (diagnosticError is not null)
            {
                result = null;
                return false;
            }
        }

        _parseResult.Add(_fileName, _sb.ToString());
        result = _parseResult;
        diagnosticError = null;
        return true;
    }

    private string? TryConsume(FileNameToken fileNameToken)
    {
        tokenizer.Consume();

        if (!tokenizer.TryPeek(out var token) || token is not IdentifierToken identifierToken)
        {
            return "Expected file name token to be followed by an identifier";
        }

        tokenizer.Consume();
        _parseResult.Add(_fileName, _sb.ToString());
        _fileName = identifierToken.Identifier;
        _sb.Clear();
        return null;
    }

    private string? TryConsume(SourceTextToken sourceTextToken)
    {
        tokenizer.Consume();
        _sb.Append(sourceTextToken.SourceText);
        return null;
    }
}