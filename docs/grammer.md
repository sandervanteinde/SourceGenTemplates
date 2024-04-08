# Language

Language described in [EBNF](https://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_form)

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

number = [-], digit, { digit }.

for-i = "for", range, [ "as", identifier ], context-termination, 
    block, { block },
    context-switch, "end", context-terminationl

foreach = "foreach", foreach-type, "in", foreach-target, [ "as", identifier ], context-termination,
    block, { block },
    context-switch, "end", context-termination.

foreach-type = "class".

foreach-target = "assembly".

range = number, "..", number.

identifier = letter, { letter | number }.

variable-insertion = identifier.

context-switch = "::".

context-termination = ";".