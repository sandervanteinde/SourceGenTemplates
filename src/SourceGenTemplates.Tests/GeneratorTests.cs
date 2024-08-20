using Microsoft.CodeAnalysis;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Tests;

public class GeneratorTests
{
    private string _sourceText = """
                                 {{#foreach var classRef in class
                                   where classRef is partial and classRef has_attribute "GenerateBuilder"}}
                                 {{#filename classRef}}
                                 using System;

                                 namespace {{classRef.Namespace}};

                                 partial class {{classRef}}
                                 {
                                 
                                     {{#foreach var field in classRef.Fields
                                             where not field is readonly and field is private}}
                                     public {{classRef}} With{{field to pascalcase}}({{field.Type}} {{field to camelcase to escape_keywords}})
                                     {
                                         {{field}} = {{field to camelcase to escape_keywords}};
                                         return this;
                                     }
                                 
                                     public {{classRef}} Without{{field to pascalcase}}()
                                     {
                                         {{field}} = default({{field.Type}});
                                         return this;
                                     }
                                     {{/foreach}}
                                 }
                                 {{/foreach}}
                                 """;

    [Fact]
    public void Test()
    {
        var parser = new Parser(new Tokenizer(_sourceText));
        var fileNode = parser.ParseFileNode();
    }
}