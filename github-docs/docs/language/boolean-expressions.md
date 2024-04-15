# Boolean operations
The `is` keyword is used to determine a comparison should happen (equivalent to the `==` in C#).

Currently one side of the `is` **must** be a variable and the other side **must** be one of the [Predefined keywords](#predefined-keywords-for-boolean-operations).

Multiple boolean operations can be combined with the `not`, `or` and `and` keywords performing their boolean expression logic as its name suggests.

*_Brackets are currently not supported, the normal precedence of boolean operations is respected_

## Available keywords
| Keyword                       | Filtering                                                                                                                                              |
|-------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| is partial                    | Checks if the current variable has a C# partial keyword (can only return `true` for [class](../variable-types/class.md))                               |
| is _[access modifier]_        | Uses any C# _access modifier_ (private / public / protected / internal) and checks if the current variable is defined in that scope                    |  
| is readonly                   | Checks if the current variable has a C# readonly keyword.                                                                                              |
| has_attribute "AttributeName" | Checks if the current variable has the attribute with the name defined between the double quotes.<br/>Note that this is a comparison on string level.  |