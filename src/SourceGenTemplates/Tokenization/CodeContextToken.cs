using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class StartCodeContextToken(LinePositionSpan position) : Token(TokenType.StartCodeContext, position);

public class EndCodeContextToken(LinePositionSpan position) : Token(TokenType.EndCodeContext, position);