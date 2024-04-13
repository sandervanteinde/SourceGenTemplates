# Grammar

The grammar of source generator template language is described
in [EBNF](https://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_form)

```ebnf
(* Spaces are always ignored unless stated otherwise *)

file = { block }.

block =
      csharp 
    | (context_switch, directive) 
    | (context_switch, variable_expression, context_switch).

csharp = ? any valid C# which is not parsed by the grammer and interpreted as is ?

directive = filename | for_i | foreach | if_statement.

filename = "filename", " ", expression, context_termination.

expression = identifier | string | number.

string = """, { character } , """. (* A string with double quotes, e.g. "text" *)

number = digit, { digit }.

for_i = "for", "var", identifier, "in", range, context_termination, 
    block, { block },
    context_switch, "end", context_termination.

foreach = "foreach", "var", identifier, "in", foreach_target, 
    [ "where", boolean_expression ],
    context_termination, 
    block, { block },
    context_switch, "end", context_termination.
    
if_statement = "if", boolean_expression, context_termination,
    { block },
    [ else_statement ]
    context_switch, "end", context_termination.

else_statement = 
    ( "else", context_termination, { block } )
    | ( "else", "if", boolean_expression, context_termination, { block }, [ else_statement ] ).

boolean_expression = (variable_expression, "is", predefined_conditions)
    | (predefined_conditions, "is", variable_expression) 
    | logical_operator.

foreach_target = "class" | variable_expression. (* variable_expression must be pointing to a valid collection *)

predefined_conditions = "partial" | access_modifier | "readonly".

logical_operator = or_operator | and_operator | not_operator.

or_operator = boolean_expression, "or", boolean_expression.
and_operator = boolean_expression, "and", boolean_expression.
not_operator = "not", boolean_expression.

range = number, "..", number.

identifier = letter, { letter | number }.

variable_expression = (identifier, [ mutator-expression ])
    | (identifier, property_access, [ mutator-expression ]).
    
mutator_expression = "to", mutator_operand, [ mutator_expression ].

mutator_operand = "pascalcase", "camelcase", "escape_keywords".

property_access = ".", identifier, [ property_access ].

context_switch = "::".

access_modifier: "public" | "private" | "protected" | "internal" | ("protected", "internal") | ("internal", "protected"), ("private", "protected") | ("protected", "private")

context_termination = ";".