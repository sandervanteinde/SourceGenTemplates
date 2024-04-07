using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class Tokenizer(SourceText sourceText)
{
    private readonly List<Token> _tokens = Parse(sourceText);

    private int _index;

    public bool TryPeek(out Token? token)
    {
        return TryPeek(offset: 0, out token);
    }

    public bool TryPeek(int offset, out Token? token)
    {
        var index = _index + offset;

        if (index < _tokens.Count)
        {
            token = _tokens[index];
            return true;
        }

        token = null!;
        return false;
    }

    public Token Consume()
    {
        if (!TryPeek(out var result))
        {
            throw new ParserException("End of file", _tokens[_tokens.Count - 1]);
        }

        _index++;
        return result!;
    }

    public void Consume(int amount)
    {
        while (amount > 0)
        {
            Consume();
            amount--;
        }
    }

    private static List<Token> Parse(SourceText sourceText)
    {
        var text = sourceText.ToString();
        var inCodeContext = false;
        var index = 0;
        var lineNumber = 0;
        var column = 0;
        var list = new List<Token>();

        do
        {
            if (TryHandleContextSwitch(out var contextSwitchToken))
            {
                list.Add(contextSwitchToken!);
            }

            if (inCodeContext)
            {
                list.Add(HandleCodeContext());
            }
            else
            {
                if (HandleNonCodeContext(out var token))
                {
                    list.Add(token!);
                }

            }
        } while (index < text.Length);

        return list;

        Token HandleCodeContext()
        {
            var span = text.AsSpan()
                .Slice(index);

            while (span[index: 0] == ' ')
            {
                BumpIndex();
                span = span.Slice(start: 1);
            }

            var currentChar = span[index: 0];
            var endIndex = 0;

            if (char.IsDigit(currentChar))
            {
                while (char.IsDigit(span[0 + endIndex]))
                {
                    endIndex++;
                }

                var number = int.Parse(
                    span.Slice(start: 0, endIndex)
                        .ToString()
                );
                var position = BumpBy(endIndex);
                return new NumberToken(position, number);
            }

            if (currentChar == '.' && span.Length > 2 && span[index: 1] == '.')
            {
                var location = BumpBy(count: 2);
                return new DoubleDotToken(location);
            }

            if (!char.IsLetter(currentChar))
            {
                var position = CurrentPosition();
                throw new ParserException($"Unknown character in code context: {currentChar}", new LinePositionSpan(position, position));
            }

            endIndex++;

            while (char.IsLetterOrDigit(span[endIndex]))
            {
                endIndex++;
            }

            var word = span.Slice(start: 0, endIndex)
                .ToString();

            var namePosition = BumpBy(endIndex);

            return word switch
            {
                "filename" => new FileNameToken(namePosition),
                "for" => new ForToken(namePosition),
                "end" => new EndToken(namePosition),
                "as" => new AsToken(namePosition),
                "foreach" => new ForeachToken(namePosition),
                "class" => new ClassToken(namePosition),
                "assembly" => new AssemblyToken(namePosition),
                "in" => new InToken(namePosition),
                _ => new IdentifierToken(namePosition, word)
            };
        }

        bool HandleNonCodeContext(out Token? token)
        {
            var span = text.AsSpan(index);
            var textLength = 0;

            while (span.Length < 2 || span[index: 0] != ':' || span[index: 1] != ':')
            {
                textLength++;
                span = span.Slice(start: 1);

                if (span.Length == 1)
                {
                    textLength++;
                    break;
                }
            }

            if (textLength > 0)
            {
                var fullSourceText = text.Substring(index, textLength);
                var position = BumpBy(textLength);

                if (string.IsNullOrWhiteSpace(fullSourceText))
                {
                    token = null;
                    return false;
                }
                SourceTextToken sourceTextToken = new(position, fullSourceText);
                token = sourceTextToken;
                return true;
            }

            token = null;
            return false;
        }

        bool TryHandleContextSwitch(out Token? token)
        {
            var span = text.AsSpan(index);

            if (span.IsEmpty)
            {
                token = null;
                return false;
            }
            var incrementIndex = 0;

            while (char.IsWhiteSpace(span[index: 0]))
            {
                incrementIndex++;
                span = span.Slice(start: 1);
            }

            if (span.Length < 2)
            {
                token = null;
                return false;
            }

            if (span[index: 0] == ':' && span[index: 1] == ':')
            {
                var position = BumpBy(2 + incrementIndex);
                inCodeContext = !inCodeContext;
                token = new CodeContextToken(position);
                return true;
            }

            if (span[index: 0] == ';')
            {
                var position = BumpBy(1 + incrementIndex);
                token = new CodeContextEndToken(position);
                inCodeContext = false;
                span = text.AsSpan()
                    .Slice(index);

                var increment = 0;
                while (!span.IsEmpty && span[0] is '\r' or '\n')
                {
                    increment++;
                    span = span.Slice(1);
                }

                if (increment > 0)
                {
                    BumpIndex(increment);
                }

                return true;
            }

            token = null;
            return false;
        }

        LinePositionSpan BumpBy(int count)
        {
            var currentPosition = CurrentPosition();
            BumpIndex(count);
            var endPosition = CurrentPosition();
            return new LinePositionSpan(currentPosition, endPosition);
        }

        void BumpIndex(int count = 1)
        {
            while (count > 0)
            {
                count--;

                if (text[index] == '\r')
                {
                    lineNumber++;
                    column = 0;
                }
                else if (text[index] is not '\r' and not '\n')
                {
                    column++;
                }

                index++;
            }
        }

        LinePosition CurrentPosition()
        {
            return new LinePosition(lineNumber, column);
        }
    }
}