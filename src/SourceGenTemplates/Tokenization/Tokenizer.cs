using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class Tokenizer(SourceText sourceText)
{
    private int _index;

    private readonly Token[] _tokens = Parse(sourceText)
        .ToArray();

    public bool TryPeek(out Token? token)
    {
        return TryPeek(0, out token);
    }
    
    public bool TryPeek(int offset, out Token? token)
    {
        var index = _index + offset;
        if (index < _tokens.Length)
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
            throw new ParserException("End of file", this);
        }

        _index++;
        return result!;
    }
    
    public void Consume(int amount)
    {
        while(amount > 0)
        {
            Consume();
            amount--;
        }
    }

    public LinePositionSpan GetCurrentLocation()
    {
        return new LinePositionSpan();
    }

    private static IEnumerable<Token> Parse(SourceText sourceText)
    {
        var text = sourceText.ToString();
        var inCodeContext = false;
        var index = 0;

        do
        {
            if (TryHandleContextSwitch(out var contextSwitchToken))
            {
                yield return contextSwitchToken!;
            }

            if (inCodeContext)
            {
                yield return HandleCodeContext();
            }
            else
            {
                if (!HandleNonCodeContext(out var token))
                {
                    break;
                }
                yield return token!;
            }
        } while (true);
        
        Token HandleCodeContext()
        {
            var span = text.AsSpan()
                .Slice(index);
    
            while (span[index: 0] == ' ')
            {
                index++;
                span = span.Slice(start: 1);
            }
    
            var currentChar = span[0];
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
                index += endIndex;
                return new NumberToken(number);
            }

            if (currentChar == '.' && span[1] == '.')
            {
                index += 2;
                return new DoubleDotToken();
            }
    
            if (currentChar == ';')
            {
                inCodeContext = false;
                index++;
                
    
                while (text[index] is '\n' or '\r')
                {
                    index++;
                }
    
                return new CodeContextEndToken();
            }
    
            if (!char.IsLetter(currentChar))
            {
                throw new NotSupportedException($"Unknown character: {currentChar}");
            }
    
            endIndex++;
    
            while (char.IsLetterOrDigit(span[endIndex]))
            {
                endIndex++;
            }
    
            var word = span.Slice(start: 0, endIndex)
                .ToString();

            index += endIndex;
    
            return word switch
            {
                "filename" => new FileNameToken(),
                "for" => new ForToken(),
                "endfor" => new EndForToken(),
                "as" => new AsToken(),
                _ => new IdentifierToken(word)
            };
        }
         bool HandleNonCodeContext(out Token? token)
         {
             var startIndex = index;
             var endIndex = startIndex;
     
             while (endIndex < text.Length && text[endIndex] != ':')
             {
                 endIndex++;
             }
     
             if (endIndex - startIndex > 0)
             {
                 var sourceTextToken = SourceTextToken.Create(text.Substring(startIndex, endIndex - startIndex));
                 index = endIndex;
                 token = sourceTextToken;
                 return true;
             }
     
             if (text.Length >= endIndex + 1 && text[endIndex + 1] == ':')
             {
                 inCodeContext = true;
                 index = endIndex + 2;
                 token = HandleCodeContext();
                 return true;
             }

             token = null;
             return false;
         }
         bool TryHandleContextSwitch(out Token? token)
         {
             if (index + 2 >= text.Length)
             {
                 token = null;
                 return false;
             }
             if (text[index] == ':' && text[index + 1] == ':')
             {
                 index += 2;
                 inCodeContext = !inCodeContext;
                 token = new CodeContextToken();
                 return true;
             }
     
             token = null;
             return false;
         }
    } 
}