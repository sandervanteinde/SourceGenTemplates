using System;

namespace SourceGenTemplates.Sample;

public partial class Tester
{
    private string _string;
    private int _integer;
    private ClassInDifferentNamespace _class;

    public void Print()
    {
        Console.WriteLine(new { _string, _integer, _class } );
    }
}