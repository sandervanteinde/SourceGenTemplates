using System;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class Tokenizer(SourceText sourceText)
{
    private readonly string _text = sourceText.ToString();
    private Token? _currentToken;
    private bool _hasCurrentTokenCached;
    private bool _inCodeContext;
    public int Index { get; private set; }

    public bool TryPeek(out Token? token)
    {
        if (_hasCurrentTokenCached)
        {
            token = _currentToken;
            return token is not null;
        }

        _hasCurrentTokenCached = true;
        _currentToken = null;

        return _inCodeContext
            ? HandleCodeContext(out token)
            : HandleNonCodeContext(out token);
    }

    private bool HandleCodeContext(out Token? token)
    {
        var span = _text.AsSpan()
            .Slice(Index)
            .TrimStart([' ']);
        var nextSpaceOrWhiteSpace = span.IndexOfAny([' ', '\n', '\r']);
        var word = span.Slice(start: 0, nextSpaceOrWhiteSpace)
            .ToString();
        var nextIndex = Index + nextSpaceOrWhiteSpace + 1;

        if (string.IsNullOrWhiteSpace(word))
        {
            Index = nextIndex;
            _inCodeContext = false;
            return HandleNonCodeContext(out token);
        }

        Token newToken = word switch
        {
            "filename" => FileNameToken.Create(),
            _ => IdentifierToken.Create(word)
        };
        return SetValue(nextIndex, newToken, out token);
    }

    private bool HandleNonCodeContext(out Token? token)
    {
        var startIndex = Index;
        var endIndex = startIndex;

        while (endIndex < _text.Length && _text[endIndex] != ':')
        {
            endIndex++;
        }

        if (endIndex - startIndex > 0)
        {
            var sourceTextToken = SourceTextToken.Create(_text.Substring(startIndex, endIndex - startIndex));
            return SetValue(endIndex, sourceTextToken, out token);
        }

        if (_text.Length >= endIndex + 1 && _text[endIndex + 1] == ':')
        {
            _inCodeContext = true;
            Index = endIndex + 2;
            return HandleCodeContext(out token);
        }

        _currentToken = token = null;
        _hasCurrentTokenCached = true;
        return false;
    }

    private bool SetValue(int newIndex, Token token, out Token outToken)
    {
        outToken = token;
        _currentToken = token;
        _hasCurrentTokenCached = true;
        Index = newIndex;
        return true;
    }

    public Token Consume()
    {
        if (!TryPeek(out var result))
        {
            throw new NotSupportedException("End of file");
        }

        _currentToken = null;
        _hasCurrentTokenCached = false;
        return result!;
    }

    public LinePositionSpan GetCurrentLocation()
    {
        if (_currentToken is null)
        {
            return new LinePositionSpan();
        }

        var endPos = Index - _currentToken.GetLength() - 1;
        var lineNumber = 0;
        var column = 0;

        for (var i = 0; i < endPos; i++)
        {
            if (_text[i] == '\r')
            {
                lineNumber++;
                column = 0;
                continue;
            }

            if (_text[i] == '\n')
            {
                continue;
            }

            column++;
        }

        return new LinePositionSpan(
            new LinePosition(lineNumber, column),
            new LinePosition(lineNumber, column + _currentToken.GetLength())
        );
    }
}