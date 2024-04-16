# Mutators
Mutators can be used to modify the representation of a variable. This can be usefull in cases where you want your generated code to adhere to things like casing.

## Syntax
A mutator can be applied on variables using the `to` syntax. Not all variables accept all mutators, see the mutators below for examples.

## Available mutators
The following mutators are available:

### pascalcase
Retrieves the string representation of the variable in `PascalCase`

Syntax: `variable to pascalcase`

Examples:

| from     | to      |
|----------|---------|
| _myField | MyField |
| MyField  | MyField | 
| myField  | MyField |


### camelcase
Retrieves the string representation of the variable in `camelCase`. 

Syntax: `variable to camelcase`

Examples:

| from     | to      |
|----------|---------|
| _myField | MyField |
| MyField  | MyField | 
| myField  | MyField |

### escape_keywords

When the resulting string is a known C# keyword, this keyword is escaped using the `@` notation. Useful in a context where using keywords is not allowed.

Syntax: `variable to escape_keywords`

Examples:

| from    | to      |
|---------|---------|
| Class   | Class   |
| class   | @class  |