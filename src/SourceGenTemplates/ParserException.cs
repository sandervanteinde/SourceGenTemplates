using System;
using Microsoft.CodeAnalysis.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates;

public class ParserException(string message, LinePositionSpan position) : Exception(message)
{
    public ParserException(string message, Token token)
        : this(message, token.Position)
    {
    }

    public LinePositionSpan LinePosition { get; } = position;
}