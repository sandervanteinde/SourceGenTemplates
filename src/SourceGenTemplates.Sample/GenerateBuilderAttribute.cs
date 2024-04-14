using System;

namespace SourceGenTemplates.Sample;

[AttributeUsage(AttributeTargets.Class)]
public class GenerateBuilderAttribute : Attribute
{
}