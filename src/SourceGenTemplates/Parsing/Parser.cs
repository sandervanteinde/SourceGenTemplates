using System.Collections.Generic;

using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Parsing.Foreach;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class Parser(Tokenizer tokenizer)
{
    public FileNode ParseFileNode()
    {
        var blocks = ParseBlocks();
        return new FileNode(blocks);
    }

    private IReadOnlyCollection<BlockNode> ParseBlocks()
    {
        var result = new List<BlockNode>();

        while (tokenizer.TryPeek(out var token))
        {
            if (token is SourceTextToken sourceText)
            {
                result.Add(new CSharpBlockNode(sourceText));
                tokenizer.Consume();
                continue;
            }

            if (TryParseVariableInsertion(out var variableInsertion))
            {
                result.Add(variableInsertion!);
                continue;
            }

            if (!TryParseDirective(out var directiveToken))
            {
                return result;
            }

            result.Add(new DirectiveBlockNode(directiveToken));
        }

        return result;
    }

    private bool TryParseVariableInsertion(out VariableInsertionBlockNode? node)
    {
        if (tokenizer.TryPeek(out var first) && tokenizer.TryPeek(offset: 1, out var second) && tokenizer.TryPeek(offset: 2, out var third))
        {
            if (first!.TokenType == TokenType.CodeContextSwitch && third!.TokenType == TokenType.CodeContextSwitch && second is IdentifierToken identifierToken)
            {
                tokenizer.Consume(amount: 3);
                node = new VariableInsertionBlockNode(identifierToken);
                return true;
            }
        }

        node = null;
        return false;
    }

    private bool TryParseDirective(out DirectiveNode directiveNode)
    {
        if (!tokenizer.TryPeek(1, out var token))
        {
            directiveNode = null!;
            return false;
        }

        if (token is FileNameToken)
        {
            tokenizer.Consume(2);
            var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected file name token to be followed by an identifier");
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected for statement to end with ;");
            IdentifierNode identifierNode = new(identifierToken);
            FileNameNode fileNameNode = new(identifierNode);
            directiveNode = new FileNameDirectiveNode(fileNameNode);

            return true;
        }

        if (token is ForToken)
        {
            tokenizer.Consume(2);
            var rangeToken = ParseRangeNode();
            IdentifierNode? identifier = null;

            if (tokenizer.TryPeek(out var possibleAsToken) && possibleAsToken is AsToken)
            {
                tokenizer.Consume();
                identifier = ParseIdentifierNode();
            }

            ConsumeExpectedToken<CodeContextEndToken>("Expected for statement to end with ;");

            var blocks = ParseBlocks();
            _ = ConsumeExpectedToken<CodeContextToken>("Expected for loop to be closed with an end statement");
            _ = ConsumeExpectedToken<EndToken>("Expected for loop to be closed with a end statement");
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");
            ForINode forInode = new(blocks, rangeToken, identifier);
            directiveNode = new ForIDirectiveNode(forInode);
            return true;
        }

        if (token is ForeachToken)
        {
            tokenizer.Consume(2);
            ConsumeExpectedToken<ClassToken>("Only classes are supported as foreach type");
            var foreachType = new ForeachTypeClass();
            ConsumeExpectedToken<InToken>("Expected 'in' keyword");
            ConsumeExpectedToken<AssemblyToken>("Only assemblies are supported as foreach target");
            var foreachTarget = new ForeachTargetAssembly();
            IdentifierNode? identifierNode = null;

            if (tokenizer.TryPeek(out var asToken) && asToken is AsToken)
            {
                tokenizer.Consume();
                var identifier = ConsumeExpectedToken<IdentifierToken>("Expected identifier after 'as'");
                identifierNode = new IdentifierNode(identifier);
            }

            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");

            var blocks = ParseBlocks();
            _ = ConsumeExpectedToken<CodeContextToken>("Expected ::end");
            _ = ConsumeExpectedToken<EndToken>("Expected ::end");
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");
            var foreachNode = new ForeachNode(foreachType, foreachTarget, identifierNode, blocks);
            directiveNode = new ForeachDirectiveNode(foreachNode);
            return true;
        }

        directiveNode = null!;
        return false;
    }

    private IdentifierNode ParseIdentifierNode()
    {
        var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected identifier");
        IdentifierNode identifierNode = new(identifierToken);
        return identifierNode;
    }

    private RangeNode ParseRangeNode()
    {
        var startIndex = ConsumeExpectedToken<NumberToken>("Expected for to be followed with a number");
        _ = ConsumeExpectedToken<DoubleDotToken>("Expected number of for loop to be followed with a range indicator (..)");
        var endIndex = ConsumeExpectedToken<NumberToken>("Expected range to be closed with a final number");
        return new RangeNode(startIndex, endIndex);
    }

    private TToken ConsumeExpectedToken<TToken>(string errorMessage)
    {
        var token = tokenizer.Consume();

        if (token is not TToken expectedToken)
        {
            throw new ParserException(errorMessage, token);
        }

        return expectedToken;
    }
}