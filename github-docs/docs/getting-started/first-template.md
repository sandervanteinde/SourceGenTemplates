# First template

After installing the nuget package you can start by creating your first template.
To do this, create a `.cstempl` file. For instance: `test.cstempl`.


Make sure to add this file to your project as an `AdditonalFile`.
This tells the .NET compiler to pick up C# files which are not automatically picked up.
This can be done in most IDE's by right-clicking on the file, selecting `Properties`, and then select `AdditionalFiles` under the `Build actions` section.

Your .csproj should now contain the following XML
```xml
    <ItemGroup>
        <None Remove="GenerateBuilders.cstempl"/>
        <AdditionalFiles Include="GenerateBuilders.cstempl"/>
    </ItemGroup>
```

## Creating the first template
Add the following contents to your file as a test to see if it works:

```CSharp
namespace SourceGeneratorOutput;

public class HelloWorld
{
    public void SayHelloThreeTimes()
    {
        {{#for var i in 1..3}}
        Console.WriteLine("Hello {{i}}");
        {{/for}}
    }    
}
```

From your project, you should now be able to invoke `HelloWorld.SayHelloTenTimes()` which should print:
```
Hello 1
Hello 2
Hello 3
```

## Next steps!
You've successfully installed `{{nugetpackagename}}` and written your first template!

A good next step is to familiarize yourself with the [language constructs](../language/index.md) available in the templating engine.

If you prefer to see some samples then head for the [Samples](../samples/index.md) section instead!