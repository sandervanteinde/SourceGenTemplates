﻿namespace SourceGenTemplates.Sample;

public partial class Tester
{
    private readonly int _readonlyTest;
    private int _test;
    public int String { get; set; }
    internal string FirstName { get; set; }
    protected string LastName { get; set; }
    private string Test { get; set; }
}

public class Test
{
}

public partial class Tester2
{
    public int String { get; set; }
}