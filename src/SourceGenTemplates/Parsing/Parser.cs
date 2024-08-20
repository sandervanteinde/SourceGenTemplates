using System;
using System.Collections.Generic;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.ControlDirectives;
using SourceGenTemplates.Parsing.Expressions;
using SourceGenTemplates.Parsing.Foreach;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.LogicalOperators;
using SourceGenTemplates.Parsing.Mutators;
using SourceGenTemplates.Parsing.TemplateBlocks;
using SourceGenTemplates.Parsing.TemplateInstructions;
using SourceGenTemplates.Parsing.VariableExpressions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class Parser(Tokenizer tokenizer)
{
    public FileNode ParseFileNode()
    {
        tokenizer.Tokenize();
        return new FileNode(ParseBlocks());
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

            if (TryParseTemplateBlock(out var templateBlockNode))
            {
                result.Add(new TemplateBlockBlockNode(templateBlockNode));
                continue;
            }

            break;
        }

        return result;
    }

    private bool TryParseTemplateBlock(out TemplateBlockNode templateBlockNode)
    {
        if (TryParseTemplateInstruction(out var templateInstructionNode))
        {
            templateBlockNode = new TemplateInstructionBlockNode(templateInstructionNode);
            return true;
        }

        if (TryParseExpression(out var expression))
        {

            templateBlockNode = new TemplateExpressionBlockNode(expression);
            return true;
        }

        templateBlockNode = null!;
        return false;
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

    private bool TryParseTemplateInstruction(out TemplateInstructionNode templateInstructionNode)
    {
        Token? secondToken = null;
        Token? token = null;
        var hasTokens = tokenizer.TryPeek(out var firstToken)
            && tokenizer.TryPeek(offset: 1, out secondToken)
            && tokenizer.TryPeek(offset: 2, out token);

        if (!hasTokens || firstToken!.Type != TokenType.StartCodeContext || secondToken!.Type != TokenType.StartDirective)
        {
            templateInstructionNode = null!;
            return false;
        }

        if (token is FileNameToken)
        {
            tokenizer.Consume(amount: 3);
            var expression = ParseExpression();
            ConsumeExpectedToken(TokenType.EndCodeContext, "Expected filename statement to end with }}");
            FileNameNode fileNameNode = new(expression);
            templateInstructionNode = new FilenameTemplateInstructionNode(fileNameNode);
            return true;
        }

        if (token is ForToken)
        {
            tokenizer.Consume(amount: 3);
            ConsumeExpectedToken(TokenType.Var, "Expected 'for' to be followed by 'var'");
            var identifier = ParseIdentifierNode();
            ConsumeExpectedToken(TokenType.In, "Expected identifier to be followed by 'in' statement");
            var rangeToken = ParseRangeNode();
            ConsumeExpectedToken(TokenType.EndCodeContext, "Expected for statement to end with }}");

            var blocks = ParseBlocks();
            const string expectedEndStatement = "Expected for loop to end with {{/for}}";
            ConsumeExpectedToken(TokenType.StartCodeContext, expectedEndStatement);
            ConsumeExpectedToken(TokenType.EndDirective, expectedEndStatement);
            ConsumeExpectedToken(TokenType.For, expectedEndStatement);
            ConsumeExpectedToken(TokenType.EndCodeContext, expectedEndStatement);
            ForINode forInode = new(blocks, rangeToken, identifier);
            templateInstructionNode = new ForITemplateInstructionNode(forInode);
            return true;
        }

        if (token is ForeachToken)
        {
            tokenizer.Consume(amount: 3);
            ConsumeExpectedToken(TokenType.Var, "Expected 'var' to be followed by 'for'");
            var identifierNode = ParseIdentifierNode();

            ConsumeExpectedToken(TokenType.In, "Expected 'in' to be followed by an identifier in a 'foreach'");
            var foreachTarget = ParseForeachTarget();
            var conditionNode = tokenizer.ConsumeIfNextIsOfType(TokenType.Where)
                ? ParseBooleanExpressionNode()
                : null;
            ConsumeExpectedToken(TokenType.EndCodeContext, "Expected foreach statement to end with }}");

            var blocks = ParseBlocks();

            const string expectedEndStatement = "Expected foreach loop to end with {{/foreach}}";
            ConsumeExpectedToken(TokenType.StartCodeContext, expectedEndStatement);
            ConsumeExpectedToken(TokenType.EndDirective, expectedEndStatement);
            ConsumeExpectedToken(TokenType.Foreach, expectedEndStatement);
            ConsumeExpectedToken(TokenType.EndCodeContext, expectedEndStatement);
            var foreachNode = new ForeachNode(foreachTarget, identifierNode, blocks, conditionNode);
            templateInstructionNode = new ForeachTemplateInstructionNode(foreachNode);
            return true;
        }

        if (token is IfToken)
        {
            tokenizer.Consume(amount: 3);
            var booleanExpressionNode = ParseBooleanExpressionNode();
            ConsumeExpectedToken(TokenType.EndCodeContext, "Expected if statement to be terminated with }}");
            var blocks = ParseBlocks();
            var elseExpression = TryParseElseExpressionNode();

            const string expectedEndStatement = "Expected foreach loop to end with {{/if}}";
            ConsumeExpectedToken(TokenType.StartCodeContext, expectedEndStatement);
            ConsumeExpectedToken(TokenType.EndDirective, expectedEndStatement);
            ConsumeExpectedToken(TokenType.If, expectedEndStatement);
            ConsumeExpectedToken(TokenType.EndCodeContext, expectedEndStatement);
            var ifNode = new IfNode(booleanExpressionNode, blocks, elseExpression);
            templateInstructionNode = new IfTemplateInstructionNode(ifNode);
            return true;
        }

        templateInstructionNode = null!;
        return false;
    }

    private ElseExpressionNode? TryParseElseExpressionNode()
    {
        Token? secondToken = null;
        Token? thirdToken = null;
        var hasTokens = tokenizer.TryPeek(out var firstToken)
            && tokenizer.TryPeek(offset: 1, out secondToken)
            && tokenizer.TryPeek(offset: 2, out thirdToken);

        if (!hasTokens || firstToken!.Type != TokenType.StartCodeContext || secondToken!.Type != TokenType.EndDirective || thirdToken!.Type != TokenType.Else)
        {
            return null;
        }

        tokenizer.Consume(amount: 3);

        if (tokenizer.ConsumeIfNextIsOfType(TokenType.If))
        {
            var booleanExpression = ParseBooleanExpressionNode();
            ConsumeExpectedToken(TokenType.EndCodeContext, "Expected else if statement to end with }}");
            var blocks = ParseBlocks();
            var elseExpression = TryParseElseExpressionNode();
            return new ElseIfElseExpressionNode(blocks, booleanExpression, elseExpression);
        }

        ConsumeExpectedToken(TokenType.EndCodeContext, "Expected else statement to end with }}");

        var elseBlocks = ParseBlocks();
        return new ElseElseExpressionNode(elseBlocks);
    }

    private BooleanExpressionNode ParseBooleanExpressionNode()
    {
        return ParseBooleanExpressionNode(GetRawNodeFromTokenizer(), precedence: 0);
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

            if (TryParseVariableExpression(out var secondVariableExpression))
            {
                return new VariableComparisonBooleanExpressionNode(variableExpressionNode!, secondVariableExpression!);
            }
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
            return tokenizer.TryPeek(offset: 1, out var next) && next!.Type == expectedTokenType;
        }
    }

    private bool TryParseExpression(out ExpressionNode? expressionNode)
    {
        Token? secondToken = null;
        var hasTokens = tokenizer.TryPeek(out var firstToken)
            && tokenizer.TryPeek(1, out secondToken);

        if (!hasTokens || firstToken!.Type != TokenType.StartCodeContext || secondToken!.Type is not TokenType.String and not TokenType.Number and not TokenType.Identifier)
        {
            expressionNode = null!;
            return false;
        }

        tokenizer.Consume();
        expressionNode = ParseExpression();
        ConsumeExpectedToken(TokenType.EndCodeContext, "Expected expression to end with }}");
        return true;
    }

    private ExpressionNode ParseExpression()
    {
        if (TryParseVariableExpression(out var variableExpression))
        {
            return new VariableExpressionExpressionNode(variableExpression!);
        }
        var token = tokenizer.Consume();
        return token.Type switch
        {
            TokenType.String => new StringExpressionNode((StringToken)token),
            TokenType.Number => new NumberExpressionNode((NumberToken)token),
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
        var startIndex = ConsumeRangeValueNode();
        _ = ConsumeExpectedToken<DoubleDotToken>("Expected number of for loop to be followed with a range indicator (..)");
        var endIndex = ConsumeRangeValueNode();
        return new RangeNode(startIndex, endIndex);
    }

    private RangeValueNode ConsumeRangeValueNode()
    {
        var token = tokenizer.Consume();
        return token.Type switch
        {
            TokenType.Number => new NumberRangeValueNode((NumberToken)token),
            TokenType.Identifier => new IdentifierRangeValueNode(new IdentifierNode((IdentifierToken)token)),
            _ => throw new ParserException("Unexpected value for range", token)
        };
    }

    private void ConsumeExpectedToken(TokenType tokenType, string errorMessage)
    {
        if (!tokenizer.TryPeek(out var token))
        {
            throw new ParserException(errorMessage, tokenizer.Last);
        }

        tokenizer.Consume();

        if (token!.Type != tokenType)
        {
            throw new ParserException(errorMessage, token);
        }
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