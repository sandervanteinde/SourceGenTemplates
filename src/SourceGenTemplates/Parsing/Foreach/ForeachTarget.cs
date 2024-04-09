using SourceGenTemplates.Generation;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach;

public abstract class ForeachTarget(ForeachTargetType type, Token token)
{
    public ForeachTargetType Type => type;
    public Token Token => token;

    public abstract Variable GetVariableForType(ForeachType type, CompilationContext compilation, VariableContext variables);
}