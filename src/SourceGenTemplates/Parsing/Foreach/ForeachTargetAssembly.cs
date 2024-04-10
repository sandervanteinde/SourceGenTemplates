using System.Linq;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach;

public class ForeachTargetClass(ClassToken token) : ForeachTarget(ForeachTargetType.Class, token)
{
    public override Variable GetVariableForType(CompilationContext compilation, VariableContext variables)
    {
        var allClasses = compilation.Classes
            .Select(c => new ClassVariable(c))
            .ToList();
        return new VariableCollection(allClasses);
    }
}