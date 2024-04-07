# Language description

The template language accepts any C# as input. The language supports control directive to be more flexible with what you
want to do.

To use a directive, use `::` followed by the directive name. Each directive has an example with more explanation of how
to use it

## filename

With using `::filename Name` you can modify the file name of the current scope. This can also be used to output multiple
files in one template.

```CSharp
// Test.cstempl
::filename NotTest
namespace Testing;

public class Test 
{
    public static void DoSomething() { }
}
```

Will result in

```CSharp
// NotTest.g.cs
namespace Testing;

public class Test
{
    public static void DoSomething() { }
}
```

__WARNING__: This directive is accepted anywhere, however as soon as the parser encounters a token a new file is
generated. So this could lead to unwanted behavior. For instance, if you would declare this in the middle of a
block-scope (class declaration, method declaration, etc.)  you will have 2 incomplete files which cannot be parsed as a
result.