using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class DoubleDotToken(LinePositionSpan position) : Token(TokenType.DoubleDot, position);
