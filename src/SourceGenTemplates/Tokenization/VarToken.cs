using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class VarToken(LinePositionSpan position) : Token(TokenType.Var, position);

public class StartDirectiveToken(LinePositionSpan position) : Token(TokenType.StartDirective, position);

public class EndDirectiveToken(LinePositionSpan position) : Token(TokenType.EndDirective, position);