The templating language knows variables just like any other language. Our variables always have a string representation.
This means that when you use it in a [variable insertion](../language/index.md#variable-insertions) you always get it's string representation.

The following variable types are currently supported in the language:

| Variable type                                  | Represents                                                                                                         |
|------------------------------------------------|--------------------------------------------------------------------------------------------------------------------|
| [Class](./class.md)                            | A class defined in the source assembly                                                                             |
| [Field](./field.md)                            | A field defined somewhere                                                                                          |
| [Integer](./integer.md)                        | A positive integer, currently only used for the [for range](../language/control-directives.md#for-range-directive) |
| [Namespace](./namespace.md)                    | A C# namespace                                                                                                     |
| [Property](./property.md)                      | A property defined somewhere                                                                                       |
| [String](./string.md)                          | A string of characters                                                                                             |
| [Type](./type.md)                              | The type of a property, field, method, or otherwise where types are applicable                                     |
| [VariableCollection](./variable-collection.md) | A collection of one of the aforementioned variables                                                                |

