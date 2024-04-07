using System.Collections.Generic;
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
        if (tokenizer.TryPeek(out var first) && tokenizer.TryPeek(1, out var second) && tokenizer.TryPeek(2, out var third))
        {
            if (first!.TokenType == TokenType.CodeContextSwitch && third!.TokenType == TokenType.CodeContextSwitch && second is IdentifierToken identifierToken)
            {
                tokenizer.Consume(3);
                node = new VariableInsertionBlockNode(identifierToken);
                return true;
            }
        }

        node = null;
        return false;
    }

    private bool TryParseDirective(out DirectiveNode directiveNode)
    {
        _ = ConsumeExpectedToken<CodeContextToken>("Expected a code context token");
        if (!tokenizer.TryPeek(out var token))
        {
            directiveNode = null!;
            return false;
        }

        tokenizer.Consume();

        if (token is FileNameToken)
        {
            tokenizer.Consume();
            var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected file name token to be followed by an identifier");
            var identifierNode = new IdentifierNode(identifierToken);
            var fileNameNode = new FileNameNode(identifierNode);
            directiveNode = new FileNameDirectiveNode(fileNameNode);
            return true;
        }

        if (token is ForToken)
        {
            tokenizer.Consume();
            var rangeToken = ParseRangeNode();
            IdentifierNode? identifier = null;

            if (tokenizer.TryPeek(out var possibleAsToken) && possibleAsToken is AsToken)
            {
                tokenizer.Consume();
                identifier = ParseIdentifierNode();
            }

            var blocks = ParseBlocks();
            _ = ConsumeExpectedToken<EndForToken>("Expected for loop to be closed with a endfor statement");
            var forInode = new ForINode(blocks, rangeToken, identifier);
            directiveNode = new ForIDirectiveNode(forInode);
            return true;
        }

        directiveNode = null!;
        return false;
    }

    private IdentifierNode ParseIdentifierNode()
    {
        var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected identifier");
        var identifierNode = new IdentifierNode(identifierToken);
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
            throw new ParserException(errorMessage, tokenizer);
        }

        return expectedToken;
    }
}