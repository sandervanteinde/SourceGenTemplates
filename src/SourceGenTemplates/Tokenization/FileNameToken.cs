using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class FileNameToken(LinePositionSpan position) : Token(TokenType.FileNameDirective, position);