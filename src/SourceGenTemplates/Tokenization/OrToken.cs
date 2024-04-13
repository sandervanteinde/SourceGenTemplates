using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class OrToken(LinePositionSpan position) : Token(TokenType.Or, position);

public class ReadonlyToken(LinePositionSpan position) : Token(TokenType.Readonly, position);