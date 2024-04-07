using System;
using Microsoft.CodeAnalysis.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates;

public class ParserException(string message, Tokenizer tokenizer) : Exception(message)
{
    public LinePositionSpan LinePosition { get; } = tokenizer.GetCurrentLocation();
}