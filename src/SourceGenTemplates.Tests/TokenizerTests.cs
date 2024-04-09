using FluentAssertions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Tests;

public class TokenizerTests
{
    [Fact]
    public void ForEachWhileLoop()
    {
        // ARRANGE
        var code = """
                   ::foreach class
                       in assembly
                       where partial
                       as c;
                   """;

        // ACT
        var tokenizer = new Tokenizer(code);

        // ASSERT
        ConsumeAll(tokenizer)
            .Should()
            .SatisfyRespectively(
                TokenType<CodeContextToken>(),
                TokenType<ForeachToken>(),
                TokenType<ClassToken>(),
                TokenType<InToken>(),
                TokenType<AssemblyToken>(),
                TokenType<WhereToken>(),
                TokenType<PartialToken>(),
                TokenType<AsToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("c")
                ),
                TokenType<CodeContextEndToken>()
            );
    }

    [Fact]
    public void SemicolonAfterCodeContextInsertion()
    {
        // ARRANGE
        var code = "::c.Namespace::;";

        // ACT
        var tokenizer = new Tokenizer(code);

        // ASSERT
        ConsumeAll(tokenizer)
            .Should()
            .SatisfyRespectively(
                TokenType<CodeContextToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("c")
                ),
                TokenType<DotToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("Namespace")
                ),
                TokenType<CodeContextToken>(),
                TokenType<SourceTextToken>(
                    c => c.SourceText.Should()
                        .Be(";")
                )
            );
    }

    [Fact]
    public void PropertyAccessor()
    {
        // ARRANGE
        var code = "::c.Namespace::";

        // ACT
        var tokenizer = new Tokenizer(code);

        // ASSERT
        ConsumeAll(tokenizer)
            .Should()
            .SatisfyRespectively(
                TokenType<CodeContextToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("c")
                ),
                TokenType<DotToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("Namespace")
                ),
                TokenType<CodeContextToken>()
            );
    }

    private Action<Token> TokenType<TExpected>(Action<TExpected>? additionalChecks = null)
        where TExpected : Token
    {
        return token =>
        {
            var which = token.Should()
                .BeOfType<TExpected>()
                .Which;

            additionalChecks?.Invoke(which);
        };
    }

    private List<Token> ConsumeAll(Tokenizer tokenizer)
    {
        var tokens = new List<Token>();

        while (tokenizer.TryPeek(out _))
        {
            tokens.Add(tokenizer.Consume());
        }

        return tokens;
    }
}