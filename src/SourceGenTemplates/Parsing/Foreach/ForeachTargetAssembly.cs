using System.Linq;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach;

public class ForeachTargetAssembly(AssemblyToken token) : ForeachTarget(ForeachTargetType.Assembly, token)
{
    public override Variable GetVariableForType(ForeachType type, CompilationContext compilation, VariableContext variables)
    {
        return type.Type switch
        {
            ForEachTypeType.Class => new VariableCollection(
                compilation.Classes.Select(c => new ClassVariable(c))
                    .ToList()
            )
        };
    }
}