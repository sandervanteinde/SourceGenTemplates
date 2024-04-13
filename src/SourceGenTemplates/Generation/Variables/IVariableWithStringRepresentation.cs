namespace SourceGenTemplates.Generation.Variables;

public interface IVariableWithStringRepresentation
{
    string GetCodeRepresentation(CompilationContext compilationContext);
}