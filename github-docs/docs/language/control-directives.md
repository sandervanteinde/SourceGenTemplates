# Control directives
The language supports multiple control directives which are explained here including examples.

## If directive
As with all programming languages, you can control your template with if statements. These work as you expect it to from other languages.

The syntax for a `If directive` is:
```csharp
::if variable is condition;

::endif;
```

Any [boolean expression](./boolean-expressions.md) can be used inside the if-statement


## For range directive
The `for range` directive is a directive that allows you to iterate over a range of integers.

The syntax for a `for range` is:
```
::for var i in 1..10;
  // Your code here
::end;
```

In this case `i` is a variable and `1..10` is a range that you can define yourself. This can be a range from any positive number to a higher positive number.

## Foreach directive

The `foreach` directive is similar to how a `foreach` in C# works. It iterates over a variable which contains multiple items.

The basic syntax for a `foreach` is:
```
::foreach var item in collection;
    // Your code here
::end;
```

Where `collection` can be any variable which contains multiple items

### The class predefined collection

The `class` keyword is a keyword in the templating engine that contains all classes in your current assembly. This can be used as something to iterate over in the templating engine:

```csharp
::foreach var classRef in class;
  // Your code for each class
::end;

```

### The where clause
The where clause can be added to filter your foreach before it reaching the templating engine. This performs better and reduces nesting of your file. This is equivalent to adding a [if](#if-directive) directly after the foreach.

An example of such where clause looks like this:

Note how the syntax of the where is a [boolean expression](./boolean-expressions.md) just like the if-statement.
Anything that an if-statement supports is also supported in a where clause.

```csharp
::foreach var classRef in class
    where classRef is partial;
    // Your code her
::end;
```