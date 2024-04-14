using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class HasAttributeToken(LinePositionSpan position) : Token(TokenType.HasAttribute, position);