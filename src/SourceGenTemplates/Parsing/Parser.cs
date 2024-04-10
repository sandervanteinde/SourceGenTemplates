using System.Collections.Generic;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Parsing.Expressions;
using SourceGenTemplates.Parsing.Foreach;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.VariableExpressions;
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

        while (tokenizer.HasMore)
        {
            if (tokenizer.ConsumeIfNextIs<SourceTextToken>(out var sourceText))
            {
                result.Add(new CSharpBlockNode(sourceText));
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
        node = null;

        if (!tokenizer.ConsumeIfNextIsOfType(TokenType.CodeContextSwitch))
        {
            return false;
        }

        if (!TryParseVariableExpression(out var variableExpressionNode))
        {
            tokenizer.Return();
            return false;
        }

        node = new VariableInsertionBlockNode(variableExpressionNode!);
        _ = ConsumeExpectedToken<CodeContextToken>("Expected variable insertion to end with ::");
        return true;
    }

    private bool TryParseVariableExpression(out VariableExpressionNode? variableExpressionNode)
    {
        if (!tokenizer.TryPeek(out var identifierTokenTypeCheck) || identifierTokenTypeCheck!.TokenType != TokenType.Identifier)
        {
            variableExpressionNode = null;
            return false;
        }

        var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected identifier token");
        var propertyAccess = TryParsePropertyAccessNode();

        variableExpressionNode = propertyAccess switch
        {
            null => new VariableExpressionNodeVariableAccess(identifierToken),
            not null => new VariableExpressionNodePropertyAccess(identifierToken, propertyAccess)
        };
        return true;
    }

    private PropertyAccessNode? TryParsePropertyAccessNode()
    {
        if (!tokenizer.ConsumeIfNextIsOfType(TokenType.Dot))
        {
            return null;
        }

        var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected property accessor to be followed by an identifier");

        var furtherAccess = TryParsePropertyAccessNode();
        return new PropertyAccessNode(identifierToken, furtherAccess);
    }

    private ForeachTarget ParseForeachTarget()
    {
        if (TryParseVariableExpression(out var variableExpression))
        {
            return new ForeachTargetVariableExpression(variableExpression!);
        }

        return new ForeachTargetClass(ConsumeExpectedToken<ClassToken>("Expected variable expression or \"class\" as foreach target"));
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
            var expression = ParseExpression();
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected for statement to end with ;");
            FileNameNode fileNameNode = new(expression);
            directiveNode = new FileNameDirectiveNode(fileNameNode);

            return true;
        }

        if (token is ForToken)
        {
            tokenizer.Consume(2);
            var rangeToken = ParseRangeNode();
            IdentifierNode? identifier = null;

            if (tokenizer.ConsumeIfNextIsOfType(TokenType.As))
            {
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
            var foreachTarget = ParseForeachTarget();
            IdentifierNode? identifierNode = null;

            ForeachConditionNode? conditionNode = null;

            if (tokenizer.ConsumeIfNextIsOfType(TokenType.Where))
            {
                conditionNode = ParseForeachConditionNode();
            }

            if (tokenizer.ConsumeIfNextIsOfType(TokenType.As))
            {
                var identifier = ConsumeExpectedToken<IdentifierToken>("Expected identifier after 'as'");
                identifierNode = new IdentifierNode(identifier);
            }

            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");

            var blocks = ParseBlocks();
            _ = ConsumeExpectedToken<CodeContextToken>("Expected ::end");
            _ = ConsumeExpectedToken<EndToken>("Expected ::end");
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");
            var foreachNode = new ForeachNode(foreachTarget, identifierNode, blocks, conditionNode);
            directiveNode = new ForeachDirectiveNode(foreachNode);
            return true;
        }

        directiveNode = null!;
        return false;
    }

    private ForeachConditionNode ParseForeachConditionNode()
    {
        if (TryParseAccessModifier(out var accessModifier))
        {
            return new AccessModifierForEachConditionNode(accessModifier!);
        }

        ConsumeExpectedToken<PartialToken>("Invalid where expression");
        return new PartialForEachConditionNode();
    }

    private bool TryParseAccessModifier(out AccessModifierNode? node)
    {
        if (!tokenizer.TryPeek(out var currentNode))
        {
            node = null;
            return false;
        }

        var tokenType = currentNode!.TokenType;

        switch (tokenType)
        {
            case TokenType.Public:
                return ConsumeAndReturn(AccessModifierType.Public, out node);
            case TokenType.Internal:
                return !IsNextTokenType(TokenType.Protected)
                    ? ConsumeAndReturn(AccessModifierType.Internal, out node)
                    : ConsumeTwoAndReturn(AccessModifierType.ProtectedInternal, out node);
            case TokenType.Protected when IsNextTokenType(TokenType.Internal):
                return ConsumeTwoAndReturn(AccessModifierType.ProtectedInternal, out node);
            case TokenType.Protected:
                return IsNextTokenType(TokenType.Private)
                    ? ConsumeTwoAndReturn(AccessModifierType.PrivateProtected, out node)
                    : ConsumeAndReturn(AccessModifierType.Private, out node);
            case TokenType.Private:
                return IsNextTokenType(TokenType.Protected)
                    ? ConsumeTwoAndReturn(AccessModifierType.PrivateProtected, out node)
                    : ConsumeAndReturn(AccessModifierType.Private, out node);
            default:
                node = null!;
                return false;
        }

        bool ConsumeTwoAndReturn(AccessModifierType type, out AccessModifierNode nodeReturn)
        {
            tokenizer.Consume();
            return ConsumeAndReturn(type, out nodeReturn);
        }

        bool ConsumeAndReturn(AccessModifierType type, out AccessModifierNode nodeReturn)
        {
            tokenizer.Consume();
            nodeReturn = new AccessModifierNode(type);
            return true;
        }

        bool IsNextTokenType(TokenType expectedTokenType)
        {
            return tokenizer.TryPeek(1, out var next) && next!.TokenType == expectedTokenType;
        }
    }

    private ExpressionNode ParseExpression()
    {
        var token = tokenizer.Consume();
        return token.TokenType switch
        {
            TokenType.String => new StringExpressionNode((StringToken)token),
            TokenType.Number => new NumberExpressionNode((NumberToken)token),
            TokenType.Identifier => new IdentifierExpressionNode((IdentifierToken)token),
            _ => throw new ParserException($"Token {token.TokenType} is not eligible for expressions", token)
        };
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
        if (!tokenizer.TryPeek(out var token))
        {
            throw new ParserException(errorMessage, tokenizer.Last);
        }

        tokenizer.Consume();

        if (token is not TToken expectedToken)
        {
            throw new ParserException(errorMessage, token!);
        }

        return expectedToken;
    }
}