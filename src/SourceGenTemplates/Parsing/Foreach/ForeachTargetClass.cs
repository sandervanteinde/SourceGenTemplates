using System.Linq;
using System.Text;
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

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("class");
    }
}