using System;
using System.Threading.Channels;
using SourceGenTemplates.Sample;

Console.WriteLine();

var tester = new Tester()
    .WithString("Hello")
    .WithInteger(123);

tester.Print();
