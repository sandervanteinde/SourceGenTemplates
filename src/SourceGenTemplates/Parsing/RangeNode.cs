using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class RangeNode(NumberToken startRange, NumberToken endRange) : Node
{
    public NumberToken StartRange => startRange;
    public NumberToken EndRange => endRange;
}