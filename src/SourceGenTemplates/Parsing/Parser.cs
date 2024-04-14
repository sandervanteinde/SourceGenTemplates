using System;
using System.Collections.Generic;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.ControlDirectives;
using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Parsing.Expressions;
using SourceGenTemplates.Parsing.Foreach;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.LogicalOperators;
using SourceGenTemplates.Parsing.Mutators;
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
        if (!tokenizer.TryPeek(out var identifierTokenTypeCheck) || identifierTokenTypeCheck!.Type != TokenType.Identifier)
        {
            variableExpressionNode = null;
            return false;
        }

        var identifierToken = ConsumeExpectedToken<IdentifierToken>("Expected identifier token");
        var propertyAccess = TryParsePropertyAccessNode();
        var mutator = TryParseMutatorExpression();

        variableExpressionNode = propertyAccess switch
        {
            null => new VariableExpressionNodeVariableAccess(identifierToken, mutator),
            not null => new VariableExpressionNodePropertyAccess(identifierToken, propertyAccess, mutator)
        };
        return true;
    }

    private MutatorExpressionNode? TryParseMutatorExpression()
    {
        if (!tokenizer.ConsumeIfNextIsOfType(TokenType.To))
        {
            return null;
        }

        var currentToken = tokenizer.Consume();
        var operand = currentToken.Type switch
        {
            TokenType.PascalCase => MutatorOperand.PascalCase,
            TokenType.CamelCase => MutatorOperand.CamelCase,
            TokenType.EscapeKeyword => MutatorOperand.EscapeKeyword,
            _ => throw new ParserException("Unknown mutator operand", currentToken)
        };

        return new MutatorExpressionNode(operand, currentToken, TryParseMutatorExpression());
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
            _ = ConsumeExpectedToken<VarToken>("Expected 'var' to be followed by 'for'");
            var identifier = ParseIdentifierNode();
            _ = ConsumeExpectedToken<InToken>("Expected 'in' to be followed by identifier in 'for' statement");
            var rangeToken = ParseRangeNode();
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
            ConsumeExpectedToken<VarToken>("Expected 'var' to be followed by 'foreach'");
            var identifierNode = ParseIdentifierNode();
            ConsumeExpectedToken<InToken>("Expected 'in' to be followed by an identifier in a 'foreach'");
            var foreachTarget = ParseForeachTarget();
            var conditionNode = tokenizer.ConsumeIfNextIsOfType(TokenType.Where)
                ? ParseBooleanExpressionNode()
                : null;

            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");

            var blocks = ParseBlocks();
            _ = ConsumeExpectedToken<CodeContextToken>("Expected ::end");
            _ = ConsumeExpectedToken<EndToken>("Expected ::end");
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");
            var foreachNode = new ForeachNode(foreachTarget, identifierNode, blocks, conditionNode);
            directiveNode = new ForeachDirectiveNode(foreachNode);
            return true;
        }

        if (token is IfToken)
        {
            tokenizer.Consume(2);
            var booleanExpressionNode = ParseBooleanExpressionNode();
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected if statement to be terminated with a ;");
            var blocks = ParseBlocks();
            var elseExpression = TryParseElseExpressionNode();
            _ = ConsumeExpectedToken<CodeContextToken>("Expected ::end");
            _ = ConsumeExpectedToken<EndToken>("Expected ::end");
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected end statement to end with ;");
            var ifNode = new IfNode(booleanExpressionNode, blocks, elseExpression);
            directiveNode = new IfDirectiveNode(ifNode);
            return true;
        }

        directiveNode = null!;
        return false;
    }

    private ElseExpressionNode? TryParseElseExpressionNode()
    {
        if (!tokenizer.TryPeek(out var possibleCodeContextSwitch) || possibleCodeContextSwitch!.Type != TokenType.CodeContextSwitch
            || !tokenizer.TryPeek(1, out var afterCodeContext) || afterCodeContext!.Type is not TokenType.Else)
        {
            return null;
        }

        tokenizer.Consume(2);

        if (tokenizer.ConsumeIfNextIsOfType(TokenType.If))
        {
            var booleanExpression = ParseBooleanExpressionNode();
            _ = ConsumeExpectedToken<CodeContextEndToken>("Expected else if statement to end with a ;");
            var blocks = ParseBlocks();
            var elseExpression = TryParseElseExpressionNode();
            return new ElseIfElseExpressionNode(blocks, booleanExpression, elseExpression);
        }

        _ = ConsumeExpectedToken<CodeContextEndToken>("Expected else if statement to end with a ;");

        var elseBlocks = ParseBlocks();
        return new ElseElseExpressionNode(elseBlocks);
    }

    private BooleanExpressionNode ParseBooleanExpressionNode()
    {
        return ParseBooleanExpressionNode(GetRawNodeFromTokenizer(), 0);
    }

    private BooleanExpressionNode ParseBooleanExpressionNode(BooleanExpressionNode left, int precedence)
    {
        while (tokenizer.TryPeek(out var lookahead) && lookahead!.Type is TokenType.Or or TokenType.And
               && GetPrecedenceForTokenType(lookahead.Type) >= precedence)
        {
            var op = tokenizer.Consume();
            var right = GetRawNodeFromTokenizer();

            while (tokenizer.TryPeek(out lookahead) && lookahead!.Type is TokenType.Or or TokenType.And
                   && GetPrecedenceForTokenType(lookahead.Type) > GetPrecedenceForTokenType(op.Type))
            {
                right = ParseBooleanExpressionNode(
                    right, +(GetPrecedenceForTokenType(lookahead.Type) > GetPrecedenceForTokenType(op.Type)
                        ? 1
                        : 0)
                );
            }

            left = new BooleanOperatorBooleanExpressionNode(
                op.Type switch
                {
                    TokenType.Or => new OrLogicalOperator(left, right),
                    TokenType.And => new AndLogicalOperator(left, right),
                    _ => throw new InvalidOperationException("The while only allows or/and")
                }
            );
        }

        return left;

        static int GetPrecedenceForTokenType(TokenType type)
        {
            return type switch
            {
                TokenType.And => 2,
                TokenType.Or => 1,
                _ => 0
            };
        }
    }

    private BooleanExpressionNode GetRawNodeFromTokenizer()
    {
        if (tokenizer.ConsumeIfNextIsOfType(TokenType.Not))
        {
            var logicalOperator = new NotLogicalOperator(GetRawNodeFromTokenizer());
            return new BooleanOperatorBooleanExpressionNode(logicalOperator);
        }

        var statementStartsWithVariableExpression = TryParseVariableExpression(out var variableExpressionNode);

        if (statementStartsWithVariableExpression)
        {
            if (tokenizer.ConsumeIfNextIsOfType(TokenType.HasAttribute))
            {
                var stringToken = ConsumeExpectedToken<StringToken>("Expected has_attribute to be followed by a double-quoted string");
                return new HasAttributeBooleanExpressionNode(variableExpressionNode!, stringToken);
            }

            _ = ConsumeExpectedToken<IsToken>("Expected 'is'");
        }

        PredefinedConditionNode predefinedConditionNode;

        if (TryParseAccessModifier(out var accessModifier, out var accessModifierToken))
        {
            predefinedConditionNode = new AccessModifierPredefinedConditionNode(accessModifier!, accessModifierToken);
        }
        else if (tokenizer.ConsumeIfNextIsOfType(TokenType.Readonly, out var readonlyToken))
        {
            predefinedConditionNode = new ReadonlyPredefinedConditionNode(readonlyToken);
        }
        else
        {
            var partialToken = ConsumeExpectedToken<PartialToken>("Invalid boolean expression");
            predefinedConditionNode = new PartialPredefinedConditionNode(partialToken);
        }

        if (!statementStartsWithVariableExpression)
        {
            _ = ConsumeExpectedToken<IsToken>("Expected 'is'");
            TryParseVariableExpression(out variableExpressionNode);

            if (variableExpressionNode is null)
            {
                tokenizer.TryPeek(out var next);
                throw new ParserException("Expected variable expression", next!);
            }
        }

        return new SimpleComparisonBooleanExpressionNode(variableExpressionNode!, predefinedConditionNode);
    }

    private bool TryParseAccessModifier(out AccessModifierNode? node, out Token token)
    {
        if (!tokenizer.TryPeek(out var currentNode))
        {
            node = null;
            token = null!;
            return false;
        }

        var tokenType = currentNode!.Type;

        return tokenType switch
        {
            TokenType.Protected when IsNextTokenType(TokenType.Internal) => ConsumeTwoAndReturn(AccessModifierType.ProtectedInternal, out node, out token),
            TokenType.Internal when IsNextTokenType(TokenType.Protected) => ConsumeTwoAndReturn(AccessModifierType.ProtectedInternal, out node, out token),
            TokenType.Protected when IsNextTokenType(TokenType.Private) => ConsumeTwoAndReturn(AccessModifierType.PrivateProtected, out node, out token),
            TokenType.Private when IsNextTokenType(TokenType.Protected) => ConsumeTwoAndReturn(AccessModifierType.PrivateProtected, out node, out token),
            TokenType.Public => ConsumeAndReturn(AccessModifierType.Public, out node, out token),
            TokenType.Internal => ConsumeAndReturn(AccessModifierType.Internal, out node, out token),
            TokenType.Protected => ConsumeAndReturn(AccessModifierType.Protected, out node, out token),
            TokenType.Private => ConsumeAndReturn(AccessModifierType.Private, out node, out token),
            _ => ReturnNothing(out node, out token)
        };

        bool ReturnNothing(out AccessModifierNode nodeReturn, out Token token)
        {
            nodeReturn = null!;
            token = null!;
            return false;
        }

        bool ConsumeTwoAndReturn(AccessModifierType type, out AccessModifierNode nodeReturn, out Token token)
        {
            tokenizer.Consume();
            return ConsumeAndReturn(type, out nodeReturn, out token);
        }

        bool ConsumeAndReturn(AccessModifierType type, out AccessModifierNode nodeReturn, out Token token)
        {
            token = tokenizer.Consume();
            nodeReturn = new AccessModifierNode(type);
            return true;
        }

        bool IsNextTokenType(TokenType expectedTokenType)
        {
            return tokenizer.TryPeek(1, out var next) && next!.Type == expectedTokenType;
        }
    }

    private ExpressionNode ParseExpression()
    {
        var token = tokenizer.Consume();
        return token.Type switch
        {
            TokenType.String => new StringExpressionNode((StringToken)token),
            TokenType.Number => new NumberExpressionNode((NumberToken)token),
            TokenType.Identifier => new IdentifierExpressionNode((IdentifierToken)token),
            _ => throw new ParserException($"Token {token.Type} is not eligible for expressions", token)
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