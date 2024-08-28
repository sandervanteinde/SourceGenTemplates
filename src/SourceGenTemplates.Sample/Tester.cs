using System;

namespace SourceGenTemplates.Sample;

[GenerateBuilder]
public partial class Tester
{
    private ClassInDifferentNamespace? _class;
    private int _integer;
    private string? _string;

    public void Print()
    {
        Console.WriteLine(new { _string, _integer, _class });
    }
}