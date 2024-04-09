# Grammar

The grammar of source generator template language is described
in [EBNF](https://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_form)

```ebnf
file = { block }.

block =
      csharp 
    | (context-switch, directive) 
    | (context-switch, variable_insertion, context-switch).

csharp = ? any valid C# which is not parsed by the grammer and interpreted as is ?

directive = filename | for-i | foreach.

filename = "filename", " ", expression, context-termination.

expression = identifier | string | number.

string = """, { character } , """. (* A string with double quotes, e.g. "text" *)

number = digit, { digit }.

for-i = "for", range, [ "as", identifier ], context-termination, 
    block, { block },
    context-switch, "end", context-termination.

foreach = "foreach", foreach-type, "in", foreach-target, 
    [ "where", foreach-condition ],
    [ "as", identifier ],
    context-termination, 
    block, { block },
    context-switch, "end", context-termination.

foreach-type = "class".

foreach-target = "assembly".

foreach-condition = "partial"

range = number, "..", number.

identifier = letter, { letter | number }.

variable-expression = identifier
    | (identifier, property_access).

property-access = ".", identifier, [ property-access ]

context-switch = "::".

context-termination = ";".