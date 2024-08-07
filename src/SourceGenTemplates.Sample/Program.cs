using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SourceGenTemplates.Sample;

Console.WriteLine();

// var tester = new Tester()
//     .WithString("Hello")
//     .WithInteger(123);
//
// tester.Print();
//
// var test = (1, 2);
// var sw = Stopwatch.GetTimestamp();
// var (val1, val2) = await (GetAfter5Seconds(), GetAfter10Seconds());
// Console.WriteLine(new { val1, val2 });
// Console.WriteLine($"Elapsed {Stopwatch.GetElapsedTime(sw)}");
//
// async Task<int> GetAfter10Seconds()
// {
//     await Task.Delay(10_000);
//     return 10;
// }
//
// async Task<int> GetAfter5Seconds()
// {
//     await Task.Delay(5_000);
//     return 5;
// }