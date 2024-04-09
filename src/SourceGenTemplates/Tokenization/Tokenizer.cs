using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class Tokenizer(SourceText sourceText)
{
    private readonly List<Token> _tokens = Parse(sourceText);

    private int _index;
    public Token Last => _tokens[_tokens.Count - 1];

    public bool TryPeek(out Token? token)
    {
        return TryPeek(0, out token);
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
            throw new ParserException("Unexpected end of file", _tokens[_tokens.Count - 1]);
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
            else if (inCodeContext)
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

            while (char.IsWhiteSpace(span[0]))
            {
                BumpIndex();
                span = span.Slice(1);
            }

            var currentChar = span[0];
            var endIndex = 0;

            if (char.IsDigit(currentChar))
            {
                while (char.IsDigit(span[endIndex]))
                {
                    endIndex++;
                }

                var number = int.Parse(
                    span.Slice(0, endIndex)
                        .ToString()
                );
                var position = BumpBy(endIndex);
                return new NumberToken(position, number);
            }

            if (currentChar == '.' && span.Length > 2 && span[1] == '.')
            {
                var location = BumpBy(2);
                return new DoubleDotToken(location);
            }

            if (currentChar == '"')
            {
                var nextIndex = span
                    .Slice(1)
                    .IndexOfAny('"', '\n', '\r');

                if (nextIndex == -1 || span[nextIndex + 1] is not '"')
                {
                    var position = CurrentPosition();
                    throw new ParserException("String has no matching close quote", new LinePositionSpan(position, position));
                }

                var location = BumpBy(nextIndex + 2);
                var stringToken = new StringToken(
                    location, span.Slice(1, nextIndex)
                        .ToString()
                );
                return stringToken;
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

            var word = span.Slice(0, endIndex)
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

            while (span.Length < 2 || span[0] != ':' || span[1] != ':')
            {
                textLength++;
                span = span.Slice(1);

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

            if (inCodeContext)
            {
                while (char.IsWhiteSpace(span[0]))
                {
                    incrementIndex++;
                    span = span.Slice(1);
                }
            }

            if (span[0] == ';')
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

            if (span.Length < 2)
            {
                token = null;
                return false;
            }

            if (span[0] == ':' && span[1] == ':')
            {
                var position = BumpBy(2 + incrementIndex);
                inCodeContext = !inCodeContext;
                token = new CodeContextToken(position);
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