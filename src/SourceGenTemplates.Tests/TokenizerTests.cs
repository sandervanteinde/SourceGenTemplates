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
                   {{#foreach var c in class where partial}}
                   partial class {{c}} 
                   {
                      public void Dummy() {} 
                   }
                   {{/foreach}}
                   """;

        // ACT
        var tokenizer = new Tokenizer(code);

        // ASSERT
        ConsumeAll(tokenizer)
            .Should()
            .SatisfyRespectively(
                TokenType<StartCodeContextToken>(),
                TokenType<StartDirectiveToken>(),
                TokenType<ForeachToken>(),
                TokenType<VarToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("c")
                ),
                TokenType<InToken>(),
                TokenType<ClassToken>(),
                TokenType<WhereToken>(),
                TokenType<PartialToken>(),
                TokenType<EndCodeContextToken>(),
                TokenType<SourceTextToken>(),
                TokenType<StartCodeContextToken>(),
                TokenType<IdentifierToken>(
                    c => c.Identifier.Should()
                        .Be("c")
                ),
                TokenType<EndCodeContextToken>(),
                TokenType<SourceTextToken>(),
                TokenType<StartCodeContextToken>(),
                TokenType<EndDirectiveToken>(),
                TokenType<ForeachToken>(),
                TokenType<EndCodeContextToken>()
            );
    }

    private static Action<Token> TokenType<TExpected>(Action<TExpected>? additionalChecks = null)
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

    private static IEnumerable<Token> ConsumeAll(Tokenizer tokenizer)
    {
        tokenizer.Tokenize();
        var tokens = new List<Token>();

        while (tokenizer.TryPeek(out _))
        {
            tokens.Add(tokenizer.Consume());
        }

        return tokens;
    }
}