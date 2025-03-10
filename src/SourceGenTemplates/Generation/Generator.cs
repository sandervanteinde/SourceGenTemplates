using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.ControlDirectives;
using SourceGenTemplates.Parsing.Expressions;
using SourceGenTemplates.Parsing.Foreach;
using SourceGenTemplates.Parsing.LogicalOperators;
using SourceGenTemplates.Parsing.TemplateBlocks;
using SourceGenTemplates.Parsing.TemplateInstructions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation;

public class Generator(string fileName, FileNode file, SourceProductionContext context, CompilationContext compilationContext)
{
    private readonly StringBuilder _sb = new();
    private readonly VariableContext _variables = new(compilationContext);

    public void AddToOutput()
    {
        GenerateBlocks(file.Blocks);
        OutputCurrentContentsToFile();
    }

    private void GenerateBlocks(IEnumerable<BlockNode> blocks)
    {
        foreach (var block in blocks)
        {
            Generate(block);
        }
    }

    private void Generate(BlockNode block)
    {
        _ = block.Type switch
        {
            BlockNodeType.CSharp => GenerateSourceText((CSharpBlockNode)block),
            BlockNodeType.Template => GenerateTemplateBlockNode(((TemplateBlockBlockNode)block).TemplateBlockNode)
        };
    }

    private bool GenerateSourceText(CSharpBlockNode node)
    {
        _sb.Append(node.SourceText.SourceText);
        return true;
    }

    private bool GenerateExpression(ExpressionNode expression)
    {
        var sourceText = ParseExpressionToSourceText(expression);
        _sb.Append(sourceText);
        return true;
    }

    private bool GenerateTemplateBlockNode(TemplateBlockNode templateBlockNode)
    {
        return templateBlockNode.Type switch
        {
            TemplateBlockNodeType.Instruction => GenerateTemplateInstructionNode(((TemplateInstructionBlockNode)templateBlockNode).TemplateInstruction),
            TemplateBlockNodeType.Expression => GenerateExpression(((TemplateExpressionBlockNode)templateBlockNode).Expression)
        };
    }

    private bool GenerateTemplateInstructionNode(TemplateInstructionNode directive)
    {
        return directive.Type switch
        {
            TemplateInstructionNodeType.Filename => GenerateFileNameDirectiveNode(((FilenameTemplateInstructionNode)directive).FileName),
            TemplateInstructionNodeType.ForI => GenerateForIDirectiveNode(((ForITemplateInstructionNode)directive).ForINode),
            TemplateInstructionNodeType.Foreach => GenerateForeachDirectiveNode(((ForeachTemplateInstructionNode)directive).ForeachNode),
            TemplateInstructionNodeType.If => GenerateIfDirectiveNode(((IfTemplateInstructionNode)directive).If)
        };
    }

    private bool GenerateIfDirectiveNode(IfNode @if)
    {
        if (IsBooleanExpressionTrue(@if.BooleanExpression))
        {
            GenerateBlocks(@if.Blocks);
            return true;
        }

        var elseExpression = @if.ElseExpression;

        while (elseExpression is not null)
        {
            elseExpression = elseExpression.Type switch
            {
                ElseNodeType.Else => ParseElse((ElseElseExpressionNode)elseExpression),
                ElseNodeType.ElseIf => ParseElseIf((ElseIfElseExpressionNode)elseExpression)
            };
        }

        return true;

        ElseExpressionNode? ParseElse(ElseElseExpressionNode elseElseExpression)
        {
            GenerateBlocks(elseElseExpression.Blocks);
            return null;
        }

        ElseExpressionNode? ParseElseIf(ElseIfElseExpressionNode elseIfExpression)
        {
            if (IsBooleanExpressionTrue(elseIfExpression.BooleanExpression))
            {
                GenerateBlocks(elseIfExpression.Blocks);
                return null;
            }

            return elseIfExpression.ElseExpression;
        }
    }

    private bool IsBooleanExpressionTrue(BooleanExpressionNode booleanExpressionNode)
    {
        return booleanExpressionNode.ExpressionType switch
        {
            BooleanExpressionType.Simple => IsSimpleBooleanExpressionTrue((SimpleComparisonBooleanExpressionNode)booleanExpressionNode),
            BooleanExpressionType.BooleanOperator => IsBooleanOperatorExpressionTrue((BooleanOperatorBooleanExpressionNode)booleanExpressionNode),
            BooleanExpressionType.HasAttribute => IsAttributeExpressionTrue((HasAttributeBooleanExpressionNode)booleanExpressionNode),
            BooleanExpressionType.VariableComparison => IsVariableComparisonTrue((VariableComparisonBooleanExpressionNode)booleanExpressionNode)
        };
    }

    private bool IsVariableComparisonTrue(VariableComparisonBooleanExpressionNode variableComparisonBooleanExpressionNode)
    {
        var leftValue = _variables.GetOrThrow(variableComparisonBooleanExpressionNode.Left);
        var rightValue = _variables.GetOrThrow(variableComparisonBooleanExpressionNode.Right);
        return leftValue.IsEqualToVariable(rightValue);
    }

    private bool IsAttributeExpressionTrue(HasAttributeBooleanExpressionNode hasAttributeBooleanExpressionNode)
    {
        var variable = _variables.GetOrThrow(hasAttributeBooleanExpressionNode.VariableExpression);
        return variable.HasAttributeWithName(hasAttributeBooleanExpressionNode.String);
    }

    private bool IsBooleanOperatorExpressionTrue(BooleanOperatorBooleanExpressionNode booleanOperatorBooleanExpressionNode)
    {
        var logicalOperator = booleanOperatorBooleanExpressionNode.LogicalOperator;
        return logicalOperator.Type switch
        {
            LogicalOperatorType.Or => IsOrOperatorTrue((OrLogicalOperator)logicalOperator),
            LogicalOperatorType.And => IsAndOperatorTrue((AndLogicalOperator)logicalOperator),
            LogicalOperatorType.Not => IsNotOperatorTrue((NotLogicalOperator)logicalOperator)
        };

        bool IsOrOperatorTrue(OrLogicalOperator or)
        {
            return IsBooleanExpressionTrue(or.Left) || IsBooleanExpressionTrue(or.Right);
        }

        bool IsAndOperatorTrue(AndLogicalOperator and)
        {
            return IsBooleanExpressionTrue(and.Left) && IsBooleanExpressionTrue(and.Right);
        }

        bool IsNotOperatorTrue(NotLogicalOperator not)
        {
            return !IsBooleanExpressionTrue(not.Condition);
        }
    }

    private bool IsSimpleBooleanExpressionTrue(SimpleComparisonBooleanExpressionNode simpleComparisonBooleanExpressionNode)
    {
        var variable = _variables.GetOrThrow(simpleComparisonBooleanExpressionNode.VariableExpression);
        return variable.MatchesCondition(simpleComparisonBooleanExpressionNode.PredefinedCondition);
    }

    private bool GenerateFileNameDirectiveNode(FileNameNode node)
    {
        OutputCurrentContentsToFile();
        fileName = ParseExpressionToSourceText(node.Expression);
        return true;
    }

    private string ParseExpressionToSourceText(ExpressionNode expression)
    {
        return expression.Type switch
        {
            ExpressionType.VariableExpression => _variables.GetOrThrow(((VariableExpressionExpressionNode)expression).VariableExpression) is IVariableWithStringRepresentation variableWithStringRepresentation
                ? variableWithStringRepresentation.GetCodeRepresentation(compilationContext)
                : throw new ParserException("Value could not be represented as a string", ((VariableExpressionExpressionNode)expression).VariableExpression.Token),
            ExpressionType.String => ((StringExpressionNode)expression).Value.Value,
            ExpressionType.Number => throw new ParserException("Numbers are not valid file names", expression.Token)
        };
    }

    private bool GenerateForeachDirectiveNode(ForeachNode node)
    {
        var identifier = node.Identifier;

        var variable = node.ForeachTarget.GetVariableForType(compilationContext, _variables);

        if (variable.Kind is not VariableKind.Collection)
        {
            throw new ParserException("Foreach target points to a variable which is not a collection", node.ForeachTarget.Token);
        }

        var variableCollection = (VariableCollection)variable;

        foreach (var iteratorVariable in variableCollection.Variables)
        {
            using var variableContext = _variables.AddVariableToContext(identifier?.Identifier, iteratorVariable);

            if (node.Condition is null || IsBooleanExpressionTrue(node.Condition))
            {
                GenerateBlocks(node.Blocks);
            }
        }

        return true;
    }

    private bool GenerateForIDirectiveNode(ForINode node)
    {
        var start = node.Range.StartRange.GetNumericValue(_variables);
        var end = node.Range.EndRange.GetNumericValue(_variables);

        for (var i = start; i <= end; i++)
        {
            using var variableContext = _variables.AddVariableToContext(node.Identifier?.Identifier, new IntegerVariable(i));
            GenerateBlocks(node.Blocks);
        }

        return true;
    }

    private void OutputCurrentContentsToFile()
    {
        if (_sb.Length <= 0)
        {
            return;
        }

        var currentFileName = fileName;

        var syntaxTree = CSharpSyntaxTree.ParseText(_sb.ToString());
        var root = syntaxTree.GetRoot();
        var formatted = root.NormalizeWhitespace();

        var fileContents = SourceText.From(formatted.ToFullString(), Encoding.UTF8);
        var attempt = 0;

        while (true)
        {
            try
            {
                context.AddSource($"{currentFileName}.g.cs", fileContents);
                break;
            }
            catch (ArgumentException)
            {
                if (attempt++ >= 100)
                {
                    throw;
                }

                currentFileName = $"{currentFileName}.{attempt}";
            }
        }

        _sb.Clear();
    }
}