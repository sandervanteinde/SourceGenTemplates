using System.Collections.Generic;

namespace SourceGenTemplates.Parsing;

public class ParseResult
{
    private readonly Dictionary<string, string> _fileContentsByFileName = new();

    public void Add(string fileName, string fileContents)
    {
        var newLinesTrimmed = fileContents.Trim(['\n', '\r']);

        if (string.IsNullOrWhiteSpace(newLinesTrimmed))
        {
            return;
        }

        _fileContentsByFileName.Add(fileName, newLinesTrimmed);
    }

    public IEnumerable<string> GetFileNames()
    {
        return _fileContentsByFileName.Keys;
    }

    public string GetFileContents(string fileName)
    {
        return _fileContentsByFileName[fileName];
    }
}