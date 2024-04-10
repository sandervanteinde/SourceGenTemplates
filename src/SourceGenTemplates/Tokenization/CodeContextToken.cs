using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class CodeContextToken(LinePositionSpan position) : Token(TokenType.CodeContextSwitch, position);