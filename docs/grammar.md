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

directive = filename | for_i | foreach.

filename = "filename", " ", expression, context_termination.

expression = identifier | string | number.

string = """, { character } , """. (* A string with double quotes, e.g. "text" *)

number = digit, { digit }.

for_i = "for", range, [ "as", identifier ], context_termination, 
    block, { block },
    context_switch, "end", context_termination.

foreach = "foreach", foreach_target, 
    [ "where", foreach_condition ],
    [ "as", identifier ],
    context_termination, 
    block, { block },
    context_switch, "end", context_termination.

foreach_target = "assembly" | variable_expression. (* variable_expression must be pointing to a valid collection *)

foreach_condition = "partial" | access_modifier | logical_operator.

logical_operator = or_operator | and_operator | not_operator.

or_operator = foreach_condition, "or", foreach_condition.
and_operator = foreach_condition, "and", foreach_condition.
not_operator = "not", foreach_condition.

range = number, "..", number.

identifier = letter, { letter | number }.

variable_expression = identifier
    | (identifier, property_access).

property_access = ".", identifier, [ property_access ]

context_switch = "::".

access_modifier: "public" | "private" | "protected" | "internal" | ("protected", "internal") | ("private", "protected")

context_termination = ";".