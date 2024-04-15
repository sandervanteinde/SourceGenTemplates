# Language concepts

The templating language has several constructs that allows you to create dynamic templates, this page describes those in a brief summary.

## C# Context
As long as the templating engine does not encounter a `::` the code written is simply C#. Any valid C# can be written.

## Control directives
The templating engine supports several [control directives](./control-directives.md) that allow you to dynamically adjust the template to your needs. These are often similar to those used in C# and will work the same way.

These control directives will always start with `::` and end with `;`. In case a control directive requires code blocks to be defined, a control directive should be ended with `::end;`

## Variable insertions
It is also possible to [insert variables](./insert-variables.md) in your template. This can be done using the `::var::` syntax where `var` is one of your earlier defined [variables](../variable-types/index.md).

## Grammar
The templating language defines a complete [grammar](./grammar.md) with all valid term. 