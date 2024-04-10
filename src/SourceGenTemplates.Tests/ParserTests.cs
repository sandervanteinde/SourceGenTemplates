using FluentAssertions;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Tests;

public class ParserTests
{
    [Fact]
    public void GenerateForeach()
    {
        // ARRANGE
        var code = """
                   ::foreach class
                       where partial
                       as c;
                   ::end;
                   """;
        var tokenizer = new Tokenizer(code);
        var generator = new Parser(tokenizer);

        // ACT
        var fileNode = generator.ParseFileNode();

        // ASSERT
        fileNode.Blocks.Should()
            .ContainSingle()
            .Which
            .Should()
            .BeOfType<DirectiveBlockNode>()
            .Which
            .Directive
            .Should()
            .BeOfType<ForeachDirectiveNode>();
    }
}