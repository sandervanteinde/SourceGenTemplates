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

You can provide either a number or a variable which contains an integer as one of the ranges

The syntax for a `for range` is:
```
::for var i in 1..3;
    ::for var j in 1..i;
        // Do something with i and j    
    ::end;
::end;
```

In this case `i` is a variable and `1..10` is a range that you can define yourself. This can be a range from any positive number to a higher positive number.
`j' in this example is dynamicly based on i. This loop will iterate over the following values

| i   | j   |
|-----|-----|
| 1   | 1   |
| 2   | 1   |
| 2   | 2   |
| 3   | 1   |
| 3   | 2   |
| 3   | 3   |


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

## Filename directive
The filename directive can be utilized to modify the output of the source generator.
By default, the source generator emits a `<fileName>.g.cs` file, where `fileName` is the same name (without path) of the `.cstempl` file.

If you want to modify the name you can use the `::filename` directive to modify the output. 

The directive can also be utilized in the middle of your template. As soon as the parser encounters a `::filename` directive,
the output that was generated thus far is emitted to the previous file name.

Using this directive is merely controlling the output for debugging purposes. If your IDE supports viewing source generation output, you can easily identify mistakes in your template.

For example, giving that your assembly has two classes called `A` and `B`, then the following template generated the output below.
```csharp
::foreach var classRef in class;
    ::filename classRef;
    using System;

    namespace SourceGenerators;

    public class HelloFrom::classRef:: 
    {
        public static void HelloFromTemplates()
        {
            Console.WriteLine("::classRef:: was generated in a template!");
        }
    }
::end;
```
Generates:
```csharp
// file A.g.cs
using System;

namespace SourceGenerators;

public class HelloFromA
{
    public static void HelloFromTemplates()
    {
        Console.WriteLine("A was generated in a template!");
    }
}

// file B.g.cs
using System;

namespace SourceGenerators;

public class HelloFromB
{
    public static void HelloFromTemplates()
    {
        Console.WriteLine("B was generated in a template!");
    }
}
```