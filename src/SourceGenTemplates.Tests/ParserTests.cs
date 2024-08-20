using FluentAssertions;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.TemplateBlocks;
using SourceGenTemplates.Parsing.TemplateInstructions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Tests;

public class ParserTests
{
    [Fact]
    public void GenerateForeach()
    {
        // ARRANGE
        var code = """
                   {{#foreach var c in class
                       where c is partial}}
                   {{/foreach}}
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
            .BeOfType<TemplateBlockBlockNode>()
            .Which
            .TemplateBlockNode
            .Should()
            .BeOfType<TemplateInstructionBlockNode>()
            .Which
            .TemplateInstruction
            .Should()
            .BeOfType<ForeachTemplateInstructionNode>();
    }
}