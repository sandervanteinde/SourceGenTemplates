This page describes the grammar of the language. This is described in
the [EBNF](https://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_form) format.

```
(* Spaces are always ignored unless stated otherwise *)

file = { block }.

block =
      csharp
    | template_block.

csharp = ? any valid C# which is not parsed by the grammer and interpreted as is ?

template_block = template_instruction | template_expression.

template_instruction = filename | for_i | foreach | if_statement.
template_expression = context_start, expression, context_end.

filename = context_start, directive_start, "filename", " ", expression, context_end.

expression = variable_expression | string | number.

string = """, { character } , """. (* A string with double quotes, e.g. "text" *)

number = digit, { digit }.

for_i = context_start, directive_start, "for", "var", identifier, "in", range, context_end, 
    { block },
    context_start, directive_end, "for", context_end.

foreach = context_start, directive_start, "foreach", "var", identifier, "in", foreach_target, 
    [ "where", boolean_expression ], context_end, 
    { block },
    context_start, directive_end, "foreach", context_end.
    
if_statement = context_start, directive_start, "if", boolean_expression, context_end 
    { block },
    [ else_statement ]
    context_start, directive_end, "if", context_end.

else_statement = 
    ( context_start, directive_start, "else", context_end, { block } )
    | ( "else", "if", boolean_expression, context_end, { block }, [ else_statement ] ).

boolean_expression = (variable_expression, "is", predefined_conditions)
    | (predefined_conditions, "is", variable_expression) 
    | (variable expression, "is", variable_expression) 
    | (variable_expression, "has_attribute", string)
    | logical_operator.

foreach_target = "class" | variable_expression. (* variable_expression must be pointing to a valid collection *)

predefined_conditions = "partial" | access_modifier | "readonly".

logical_operator = or_operator | and_operator | not_operator.

or_operator = boolean_expression, "or", boolean_expression.
and_operator = boolean_expression, "and", boolean_expression.
not_operator = "not", boolean_expression.

range = (number | identifier), "..", (number | identifier).

identifier = letter, { letter | number }.

variable_expression = (identifier, [ mutator-expression ])
    | (identifier, property_access, [ mutator-expression ]).
    
mutator_expression = "to", mutator_operand, [ mutator_expression ].

mutator_operand = "pascalcase", "camelcase", "escape_keywords".

property_access = ".", identifier, [ property_access ].

access_modifier: "public" | "private" | "protected" | "internal" | ("protected", "internal") | ("internal", "protected"), ("private", "protected") | ("protected", "private")

context_start = "{{".
directive_start = "#".
directive_end = "/"

context_end = "}}".
```