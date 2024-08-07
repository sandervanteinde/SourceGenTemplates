# Generate builder

The following template generates a builder-like pattern when partial class is annotated with the `[GenerateBuilder]` attribute.

It then scans the class for all private fields which are not `readonly` and generates a `WithX(Type var)` and `Without()` that can be used to chain in a builder-like fashion:

## Template
```csharp
{{#foreach var classRef in class
  where classRef is partial and classRef has_attribute "GenerateBuilder"}}
{{#filename classRef}}
using System;

namespace {{classRef.Namespace}};

partial class {{classRef}}
{

    {{#foreach var field in classRef.Fields
            where not field is readonly and field is private}}
    public {{classRef}} With{{field to pascalcase}}({{field.Type}} {{field to camelcase to escape_keywords}})
    {
        {{field}} = {{field to camelcase to escape_keywords}};
        return this;
    }
    
    public {{classRef}} Without{{field to pascalcase}}()
    {
        {{field}} = default({{field.Type}});
        return this;
    }
    {{/foreach}}
}
{{/foreach}}
```

## Example class

```csharp
namespace MyBuilders;

[GenerateBuilder]
public partial class PersonBuilder 
{
    private string _firstName;
    private string _lastName;
}
```

## Generated output

```csharp
namespace MyBuilders;

partial class PersonBuilder
{
    public PersonBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }
    
    public PersonBuilder WithoutFirstName()
    {
        _firstName = default(string);
        return this;
    }
    
    public PersonBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }
    
    public PersonBuilder WihtoutLastName()
    {
        _lastName = default(string);
        return this;
    }
}
```